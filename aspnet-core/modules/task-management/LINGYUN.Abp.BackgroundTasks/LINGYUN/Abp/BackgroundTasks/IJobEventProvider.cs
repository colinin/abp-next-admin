using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks;

/// <summary>
/// 任务事件提供者
/// </summary>
public interface IJobEventProvider
{
    /// <summary>
    /// 返回所有任务事件注册接口
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<IJobEvent> GetAll();
}
