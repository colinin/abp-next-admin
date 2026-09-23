using System.Collections.Generic;

namespace LINGYUN.Abp.BackgroundTasks.Activities;

public class JobAction
{
    public string Name { get; set; } = default!;
    public Dictionary<string, object> Paramters { get; set; } = default!;
}
