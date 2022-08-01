using Elsa.Options;
using LINGYUN.Abp.Elsa.Activities.Emailing;

namespace Microsoft.Extensions.DependencyInjection;

public static class EmailingServiceCollectionExtensions
{
    public static ElsaOptionsBuilder AddEmailingActivities(this ElsaOptionsBuilder options)
    {
        options
            .AddActivity<SendEmailing>();

        return options;
    }
}
