using System;

namespace LINGYUN.Abp.BackgroundTasks;

[Flags]
public enum JobExceptionType
{
    Business = 0,
    Application = 2,
    Network = 4,
    System = 8,
    All = Business | Application | Network | System,
}

