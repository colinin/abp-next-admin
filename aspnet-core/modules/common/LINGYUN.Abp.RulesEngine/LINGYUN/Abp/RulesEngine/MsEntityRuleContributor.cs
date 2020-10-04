using LINGYUN.Abp.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RulesEngine.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using MsRulesEngine = RulesEngine.RulesEngine;
using MsWorkflowRules = RulesEngine.Models.WorkflowRules;

namespace LINGYUN.Abp.RulesEngine
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IEntityRuleContributor))]
    public class MsEntityRuleContributor : IEntityRuleContributor
    {
        protected ILogger Logger { get; }
        protected IObjectMapper ObjectMapper { get; }
        public MsEntityRuleContributor(
            IObjectMapper objectMapper,
            ILogger<MsEntityRuleContributor> logger)
        {
            Logger = logger;
            ObjectMapper = objectMapper;
        }

        public Task ApplyAsync(EntityRuleContext context)
        {
            var workflowRules = ObjectMapper.Map<List<RuleGroup>, List<MsWorkflowRules>>(context.Groups);

            var rulesEngine = new MsRulesEngine(workflowRules.ToArray(), Logger);

            foreach(var workflow in workflowRules)
            {
                var ruleRsults = rulesEngine.ExecuteRule(workflow.WorkflowName, context.Entity);
                ruleRsults.OnSuccess((eventName) =>
                {
                    Logger.LogDebug($"{workflow.WorkflowName} evaluation resulted in succees - {eventName}");
                });
                ruleRsults.OnFail(() =>
                {
                    Logger.LogWarning($"{workflow.WorkflowName} evaluation resulted in failure");
                });
            }

            return Task.CompletedTask;
        }
    }
}
