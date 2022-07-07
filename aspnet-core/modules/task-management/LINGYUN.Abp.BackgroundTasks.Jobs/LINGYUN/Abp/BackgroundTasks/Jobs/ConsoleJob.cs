using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class ConsoleJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(PropertyMessage, LocalizableStatic.Create("Console:Message"))
        };

    #endregion

    public const string PropertyMessage = "message";
    public Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetString(PropertyMessage, out var message);
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - This message: {message ?? "None"} comes from the job: {GetType()}");
        return Task.CompletedTask;
    }
}
