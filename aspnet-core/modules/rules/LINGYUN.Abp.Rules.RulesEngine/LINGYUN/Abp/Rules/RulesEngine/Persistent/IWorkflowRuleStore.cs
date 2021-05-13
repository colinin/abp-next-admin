using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Rules.RulesEngine.Persistent
{
    /// <summary>
    /// 实现此接口以用于从其他持久化存储中获取规则
    /// </summary>
    public interface IWorkflowRuleStore
    {
        Task<IEnumerable<WorkflowRules>> GetRulesAsync(Type inputType, CancellationToken cancellationToken = default);
    }
}
