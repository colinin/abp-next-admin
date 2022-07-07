using System;

namespace LINGYUN.Abp.BackgroundTasks;

[AttributeUsage(AttributeTargets.Class)]
public class DisableJobActionAttribute : Attribute
{
}
