using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Primitives;

public class EmailingJob : IJobRunnable
{
    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {

    }
}
