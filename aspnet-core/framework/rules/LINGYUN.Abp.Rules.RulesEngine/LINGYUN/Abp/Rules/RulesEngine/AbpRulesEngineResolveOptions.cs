using JetBrains.Annotations;
using System.Collections.Generic;

namespace LINGYUN.Abp.Rules.RulesEngine
{
    public class AbpRulesEngineResolveOptions
    {
        /// <summary>
        /// 合并规则  
        /// 如果为 true,在上一个提供者解析规则之后继续执行下一个提供者  
        /// 如果为 false,在上一个提供者解析规则之后立即执行规则  
        /// 默认：false
        /// </summary>
        public bool MergingWorkflows { get; set; }
        /// <summary>
        /// 规则解析提供者
        /// </summary>
        [NotNull]
        public List<IWorkflowsResolveContributor> WorkflowsResolvers { get; }

        public AbpRulesEngineResolveOptions()
        {
            MergingWorkflows = false;
            WorkflowsResolvers = new List<IWorkflowsResolveContributor>();
        }
    }
}
