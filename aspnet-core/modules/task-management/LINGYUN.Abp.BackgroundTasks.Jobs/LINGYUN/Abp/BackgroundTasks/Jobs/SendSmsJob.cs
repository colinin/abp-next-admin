using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Json;
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
        if (context.TryGetString(PropertyProperties, out var data))
        {
            var properties = new Dictionary<string, object>();

            try
            {
                var jsonSerializer = context.GetRequiredService<IJsonSerializer>();
                properties = jsonSerializer.Deserialize<Dictionary<string, object>>(data);
            }
            catch { }

            smsMessage.Properties.AddIfNotContains(properties);
        }

        var smsSender = context.GetRequiredService<ISmsSender>();

        await smsSender.SendAsync(smsMessage);
    }
}
