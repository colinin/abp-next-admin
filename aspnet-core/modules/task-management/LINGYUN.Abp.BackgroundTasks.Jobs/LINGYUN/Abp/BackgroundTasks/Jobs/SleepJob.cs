using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SleepJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(PropertyDelay, LocalizableStatic.Create("Sleep:Delay"))
        };

    #endregion

    public const string PropertyDelay = "delay";

    public async Task ExecuteAsync(JobRunnableContext context)
    {
        context.JobData.TryGetValue(PropertyDelay, out var sleep);

        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - Sleep {sleep ?? 20000} milliseconds.");

        await Task.Delay(sleep?.To<int>() ?? 20000);
    }
}
