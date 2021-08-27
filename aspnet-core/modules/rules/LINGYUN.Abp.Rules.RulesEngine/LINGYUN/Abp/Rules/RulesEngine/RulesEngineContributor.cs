using Microsoft.Extensions.Options;
using RulesEngine;
using RulesEngine.Interfaces;
using RulesEngine.Models;
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
        private readonly AbpRulesEngineOptions _options;
        private readonly IWorkflowRulesResolver _workflowRulesResolver;

        public RulesEngineContributor(
            IWorkflowRulesResolver workflowRulesResolver,
            IOptions<AbpRulesEngineOptions> options)
        {
            _options = options.Value;
            _workflowRulesResolver = workflowRulesResolver;
        }

        public override void Initialize(RulesInitializationContext context)
        {
            _ruleEngine = CreateRulesEngine();
            _workflowRulesResolver.Initialize(context);
        }

        public override async Task ExecuteAsync<T>(T input, object[] @params = null, CancellationToken cancellationToken = default)
        {
            var result = await _workflowRulesResolver.ResolveWorkflowRulesAsync(typeof(T));

            if (result.WorkflowRules.Any())
            {
                await ExecuteRulesAsync(input, result.WorkflowRules.ToArray(), @params);
            }
        }

        public override void Shutdown()
        {
        }
        /// <summary>
        /// 重写自行构建规则引擎
        /// </summary>
        /// <returns></returns>
        protected virtual Engine CreateRulesEngine()
        {
            return new Engine(Logger, _options.Settings);
        }

        protected virtual async Task ExecuteRulesAsync<T>(T input, WorkflowRules[] workflowRules, object[] @params = null)
        {
            // TODO: 性能缺陷 规则文件每一次调用都会重复编译
            _ruleEngine.AddOrUpdateWorkflow(workflowRules);

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
