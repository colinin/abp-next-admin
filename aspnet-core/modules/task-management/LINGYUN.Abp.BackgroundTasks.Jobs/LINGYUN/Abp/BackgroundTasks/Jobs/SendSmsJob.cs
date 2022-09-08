using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.BackgroundTasks.Jobs;

public class SendSmsJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(PropertyPhoneNumber, LocalizableStatic.Create("Sms:PhoneNumber"), required: true),
            new JobDefinitionParamter(PropertyMessage, LocalizableStatic.Create("Sms:Message"), required: true),

            new JobDefinitionParamter(PropertyProperties, LocalizableStatic.Create("Sms:Properties")),
        };

    #endregion

    public const string PropertyPhoneNumber = "phoneNumber";
    public const string PropertyMessage = "message";
    public const string PropertyProperties = "properties";

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var phoneNumber = context.GetString(PropertyPhoneNumber);
        var message = context.GetString(PropertyMessage);

        var smsMessage = new SmsMessage(phoneNumber, message);
        if (context.TryGetString(PropertyProperties, out var data))
        {
            var properties = new Dictionary<string, object>();

            try
            {
                properties = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            }
            catch { }

            smsMessage.Properties.AddIfNotContains(properties);
        }

        var smsSender = context.GetRequiredService<ISmsSender>();

        await smsSender.SendAsync(smsMessage);
    }
}
