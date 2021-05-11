using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RulesEngine;
using RulesEngine.Interfaces;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Engine = RulesEngine.RulesEngine;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class RulesEngineContributor : RuleContributorBase, ISingletonDependency
    {
        private IRulesEngine _ruleEngine;
        private readonly IEnumerable<IWorkflowRulesContributor> _workflowRulesContributors;

        public RulesEngineContributor(
            IServiceProvider serviceProvider,
            IOptions<AbpRulesEngineOptions> options)
        {
            _workflowRulesContributors = options.Value
                .Contributors
                .Select(serviceProvider.GetRequiredService)
                .Cast<IWorkflowRulesContributor>()
                .ToArray();
        }

        public override void Initialize(RulesInitializationContext context)
        {
            _ruleEngine = CreateRulesEngine();

            foreach (var contributor in _workflowRulesContributors)
            {
                contributor.Initialize();
            }
        }

        public override async Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            List<WorkflowRules> workflowRules = new();

            foreach (var contributor in _workflowRulesContributors)
            {
                workflowRules.AddRange(await contributor.LoadAsync<T>(cancellationToken));
            }

            if (workflowRules.Any())
            {
                await ExecuteRulesAsync(input, workflowRules.ToArray(), @params);
            }
        }

        public override void Shutdown()
        {
            foreach (var contributor in _workflowRulesContributors)
            {
                contributor.Shutdown();
            }
        }
        /// <summary>
        /// 重写自行构建规则引擎
        /// </summary>
        /// <returns></returns>
        protected virtual Engine CreateRulesEngine()
        {
            var reSetting = new ReSettings
            {
                NestedRuleExecutionMode = NestedRuleExecutionMode.Performance
            };

            return new Engine(Logger, reSetting);
        }

        protected virtual async Task ExecuteRulesAsync<T>(T input, WorkflowRules[] workflowRules, object[] @params = null)
        {
            _ruleEngine.AddWorkflow(workflowRules);

            // 传入参与验证的实体参数
            var inputs = new List<object>()
            {
                input
            };
            if (@params != null && @params.Any())
            {
                inputs.AddRange(@params);
            }
            // 其他参数以此类推

            foreach (var workflowRule in workflowRules)
            {
                // 执行当前的规则
                var ruleResult = await _ruleEngine.ExecuteAllRulesAsync(workflowRule.WorkflowName, inputs.ToArray());
                // 用户自定义扩展方法,规则校验错误抛出异常
                ruleResult.ThrowOfFaildExecute();
            }
        }
    }
}
