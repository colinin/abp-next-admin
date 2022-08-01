using Elsa.Options;
using LINGYUN.Abp.Elsa.Activities.Sms;

namespace Microsoft.Extensions.DependencyInjection;

public static class SmsServiceCollectionExtensions
{
    public static ElsaOptionsBuilder AddSmsActivities(this ElsaOptionsBuilder options)
    {
        options
            .AddActivity<SendSms>();

        return options;
    }
}
