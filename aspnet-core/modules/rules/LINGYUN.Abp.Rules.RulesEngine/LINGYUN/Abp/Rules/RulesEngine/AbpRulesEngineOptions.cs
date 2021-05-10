using Volo.Abp.Collections;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class AbpRulesEngineOptions
    {
        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string PhysicalPath { get; set; }
        /// <summary>
        /// 是否忽略租户
        /// </summary>
        public bool IgnoreMultiTenancy { get; set; }
        /// <summary>
        /// 规则提供者类型
        /// </summary>
        public ITypeList<IWorkflowRulesContributor> Contributors { get; }

        public AbpRulesEngineOptions()
        {
            Contributors = new TypeList<IWorkflowRulesContributor>();
        }
    }
}
