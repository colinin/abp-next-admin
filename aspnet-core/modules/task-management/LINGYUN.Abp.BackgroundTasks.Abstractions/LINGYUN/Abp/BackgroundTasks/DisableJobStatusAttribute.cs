using System;

namespace LINGYUN.Abp.BackgroundTasks;
/// <summary>
/// 禁用作业调度状态
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DisableJobStatusAttribute : Attribute
{
}
