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
    public interface IWorkflowStore
    {
        Task<IEnumerable<Workflow>> GetWorkflowsAsync(Type inputType, CancellationToken cancellationToken = default);

        Task<Workflow> GetWorkflowAsync(string workflowName, CancellationToken cancellationToken = default);
    }
}
