using RulesEngine;
using RulesEngine.Interfaces;
using RulesEngine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class RulesEngineContributor : RuleContributorBase, ISingletonDependency
    {
        private readonly IRulesEngine _ruleEngine;
        private readonly IWorkflowsResolver _workflowRulesResolver;

        public RulesEngineContributor(IWorkflowsResolver workflowRulesResolver)
        {
            _workflowRulesResolver = workflowRulesResolver;
        }

        public override void Initialize(RulesInitializationContext context)
        {
            _workflowRulesResolver.Initialize(context);
        }

        public override async Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            var result = await _workflowRulesResolver.ResolveWorkflowsAsync(typeof(T));

            if (result.Workflows.Any())
            {
                await ExecuteRulesAsync(input, result.Workflows.ToArray(), @params);
            }
        }

        public override void Shutdown()
        {
            _workflowRulesResolver.Shutdown();
        }

        protected async virtual Task ExecuteRulesAsync<T>(T input, Workflow[] workflows, object[] @params = null)
        {
            // TODO: 性能缺陷 规则文件每一次调用都会重复编译
            _ruleEngine.AddOrUpdateWorkflow(workflows);

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

            foreach (var workflow in workflows)
            {
                // 执行当前的规则
                var ruleResult = await _ruleEngine.ExecuteAllRulesAsync(workflow.WorkflowName, inputs.ToArray());
                // 用户自定义扩展方法,规则校验错误抛出异常
                ruleResult.ThrowOfFaildExecute();
            }
        }
    }
}
