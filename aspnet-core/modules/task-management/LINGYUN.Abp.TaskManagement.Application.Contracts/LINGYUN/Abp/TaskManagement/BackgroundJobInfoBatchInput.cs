using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobInfoBatchInput
{
    public List<Guid> JobIds { get; set; } = new List<Guid>();
}
