using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class ConsoleJob : IJobRunnable
{
    public const string PropertyMessage = "message";
    public Task ExecuteAsync(JobRunnableContext context)
    {
        context.TryGetString(PropertyMessage, out var message);
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] - This message: {message ?? "None"} comes from the job: {GetType()}");
        return Task.CompletedTask;
    }
}
