using RulesEngine.Models;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class AbpRulesEngineOptions
    {
        /// <summary>
        /// 是否忽略租户
        /// </summary>
        public bool IgnoreMultiTenancy { get; set; }
        /// <summary>
        /// 规则引擎可配置
        /// </summary>
        public ReSettings Settings { get; set; }

        public AbpRulesEngineOptions()
        {
            Settings = new ReSettings
            {
                NestedRuleExecutionMode = NestedRuleExecutionMode.Performance
            };
        }
    }
}
