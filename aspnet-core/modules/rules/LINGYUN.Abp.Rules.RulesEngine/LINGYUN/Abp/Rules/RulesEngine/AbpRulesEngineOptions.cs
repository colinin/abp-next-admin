using Volo.Abp.Collections;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class AbpRulesEngineOptions
    {
        /// <summary>
        /// 是否忽略租户
        /// </summary>
        public bool IgnoreMultiTenancy { get; set; }

        public AbpRulesEngineOptions()
        {
        }
    }
}
