namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class DefaultJobDefinitionProvider : JobDefinitionProvider
{
    public override void Define(IJobDefinitionContext context)
    {
        context.Add(CreateDefaultJobs());
    }

    private static JobDefinition[] CreateDefaultJobs()
    {
        return new[]
        {
            new JobDefinition(
                DefaultJobNames.SleepJob,
                typeof(SleepJob),
                LocalizableStatic.Create("SleepJob"),
                SleepJob.Paramters),

            new JobDefinition(
                DefaultJobNames.ConsoleJob,
                typeof(ConsoleJob),
                LocalizableStatic.Create("ConsoleJob"),
                ConsoleJob.Paramters),

            new JobDefinition(
                DefaultJobNames.SendSmsJob,
                typeof(SendSmsJob),
                LocalizableStatic.Create("SendSmsJob"),
                SendSmsJob.Paramters),

            new JobDefinition(
                DefaultJobNames.SendEmailJob,
                typeof(SendEmailJob),
                LocalizableStatic.Create("SendEmailJob"),
                SendEmailJob.Paramters),

            new JobDefinition(
                DefaultJobNames.HttpRequestJob,
                typeof(HttpRequestJob),
                LocalizableStatic.Create("HttpRequestJob"),
                HttpRequestJob.Paramters),

            new JobDefinition(
                DefaultJobNames.ServiceInvocationJob,
                typeof(ServiceInvocationJob),
                LocalizableStatic.Create("ServiceInvocationJob"),
                ServiceInvocationJob.Paramters),
        };
    }
}
