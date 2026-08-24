using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Platform.Jobs.Messages;

namespace LINGYUN.Platform.Jobs;
public class IdentityJobDefinitionProvider : JobDefinitionProvider
{
    public override void Define(IJobDefinitionContext context)
    {
        context.Add(
            new JobDefinition(
                EmailMessageRetrySendJob.Name,
                typeof(EmailMessageRetrySendJob),
                LocalizableStatic.Create("EmailMessageRetrySendJob"),
                EmailMessageRetrySendJob.Paramters),
            new JobDefinition(
                SmsMessageRetrySendJob.Name,
                typeof(SmsMessageRetrySendJob),
                LocalizableStatic.Create("SmsMessageRetrySendJob"),
                SmsMessageRetrySendJob.Paramters)
            );
    }
}
