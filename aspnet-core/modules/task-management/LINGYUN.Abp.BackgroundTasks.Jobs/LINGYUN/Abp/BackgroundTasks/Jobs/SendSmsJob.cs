using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SendSmsJob : IJobRunnable
{
    public const string PropertyPhoneNumber = "phoneNumber";
    public const string PropertyMessage = "message";
    public const string PropertyProperties = "properties";

    public virtual async Task ExecuteAsync(JobRunnableContext context)
    {
        var phoneNumber = context.GetString(PropertyPhoneNumber);
        var message = context.GetString(PropertyMessage);

        var smsMessage = new SmsMessage(phoneNumber, message);
        if (context.TryGetJobData(PropertyProperties, out var data) &&
            data is IDictionary<string, object> properties)
        {
            smsMessage.Properties.AddIfNotContains(properties);
        }

        var smsSender = context.GetRequiredService<ISmsSender>();

        await smsSender.SendAsync(smsMessage);
    }
}
