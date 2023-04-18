using LINGYUN.Abp.Rules.RulesEngine.Persistent;
using Microsoft.Extensions.Caching.Memory;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.RulesEngineManagement;

[Dependency(ReplaceServices = true)]
public class WorkflowStore : IWorkflowStore, ITransientDependency
{
    protected IMemoryCache WorkflowCache { get; }
    protected IWorkflowRecordRepository WorkflowRepository { get; }
    protected IRuleRecordRepository RuleRepository { get; }

    public WorkflowStore(
        IMemoryCache workflowCache,
        IWorkflowRecordRepository workflowRepository, 
        IRuleRecordRepository ruleRepository)
    {
        WorkflowCache = workflowCache;
        WorkflowRepository = workflowRepository;
        RuleRepository = ruleRepository;
    }

    public async virtual Task<Workflow> GetWorkflowAsync(string workflowName, CancellationToken cancellationToken = default)
    {
        var workflowRecord = await WorkflowRepository.FindByNameAsync(workflowName, cancellationToken);

        var workflow = await ConvertToWorkflow(workflowRecord);

        return workflow;
    }

    public async virtual Task<IEnumerable<Workflow>> GetWorkflowsAsync(Type inputType, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"workflow_{inputType.GetFullNameWithAssemblyName()}";
        var workflows = await WorkflowCache.GetOrCreateAsync(
            cacheKey,
            async (cache) =>
            {
                var workflows = new List<Workflow>();
                var workflowRecords = await WorkflowRepository.GetListAsync(inputType.FullName, cancellationToken);

                foreach (var workflowRecord in workflowRecords)
                {
                    var workflow = await ConvertToWorkflow(workflowRecord);
                    workflows.Add(workflow);
                }

                cache.SetAbsoluteExpiration(TimeSpan.FromHours(2));

                return workflows;
            });

        return workflows;
    }

    protected async virtual Task<Workflow> ConvertToWorkflow(WorkflowRecord workflowRecord)
    {
        var workflow = new Workflow
        {
            WorkflowName = workflowRecord.Name,
        };

        if (workflowRecord.GlobalParams != null && workflowRecord.GlobalParams.Any())
        {
            var workflowParams = new List<ScopedParam>();
            var globalParams = await WorkflowRepository.GetParamsAsync(workflowRecord.Id);
            foreach (var globalParam in globalParams)
            {
                workflowParams.Add(new ScopedParam
                {
                    Name = globalParam.Name,
                    Expression = globalParam.Expression,
                });
            }
            workflow.GlobalParams = workflowParams;
        }

        if (workflowRecord.Rules != null && workflowRecord.Rules.Any())
        {
            var rules = new List<Rule>();

            foreach (var workflowRule in workflowRecord.Rules)
            {
                var ruleRecord = await RuleRepository.FindAsync(workflowRule.RuleId);
                if (ruleRecord == null)
                {
                    continue;
                }

                await AddRuleToListRecursively(rules, ruleRecord);
            }

            workflow.Rules = rules;
        }

        if (!workflowRecord.InjectWorkflows.IsNullOrWhiteSpace())
        {
            workflow.WorkflowsToInject = workflowRecord.InjectWorkflows.Split(',');
        }

        return workflow;
    }

    protected async virtual Task AddRuleToListRecursively(List<Rule> rules, RuleRecord ruleRecord)
    {
        var rule = new Rule
        {
            RuleName = ruleRecord.Name,
            Enabled = ruleRecord.Enabled,
            ErrorMessage = ruleRecord.ErrorMessage,
            Expression = ruleRecord.Expression,
            RuleExpressionType = ruleRecord.RuleExpressionType,
            SuccessEvent = ruleRecord.SuccessEvent,
            Properties = ruleRecord.ExtraProperties,
            Operator = ruleRecord.Operator,
            Actions = new RuleActions(),
        };
        if (!ruleRecord.InjectWorkflows.IsNullOrWhiteSpace())
        {
            rule.WorkflowsToInject = ruleRecord.InjectWorkflows.Split(',');
        }
        if (ruleRecord.OnSuccess != null)
        {
            var action = await RuleRepository.GetSuccessActionAsync(ruleRecord.OnSuccess.RuleId);
            if (action != null)
            {
                rule.Actions.OnSuccess = new ActionInfo
                {
                    Name = action.Name,
                    Context = action.ExtraProperties,
                };
            }
        }
        if (ruleRecord.OnFailure != null)
        {
            var action = await RuleRepository.GetFailureActionAsync(ruleRecord.OnFailure.RuleId);
            if (action != null)
            {
                rule.Actions.OnFailure = new ActionInfo
                {
                    Name = action.Name,
                    Context = action.ExtraProperties,
                };
            }
        }
        if (ruleRecord.LocalParams != null && ruleRecord.LocalParams.Any())
        {
            var ruleParams = new List<ScopedParam>();
            var localParams = await RuleRepository.GetParamsAsync(ruleRecord.Id);
            foreach (var localParam in localParams)
            {
                ruleParams.Add(new ScopedParam
                { 
                    Name = localParam.Name,
                    Expression = localParam.Expression,
                });
            }
            rule.LocalParams = ruleParams;
        }

        var ruleChildren = new List<Rule>();
        var childrenRules = await RuleRepository.GetChildrenRulesAsync(ruleRecord.Id);
        foreach (var childrenRule in childrenRules)
        {
            await AddRuleToListRecursively(ruleChildren, childrenRule);
        }
        rule.Rules = ruleChildren;

        rules.Add(rule);
    }
}
