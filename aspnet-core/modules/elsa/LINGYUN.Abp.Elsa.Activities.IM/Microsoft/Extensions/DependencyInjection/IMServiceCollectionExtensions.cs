using Elsa.Options;
using LINGYUN.Abp.Elsa.Activities.IM;

namespace Microsoft.Extensions.DependencyInjection;

public static class IMServiceCollectionExtensions
{
    public static ElsaOptionsBuilder AddIMActivities(this ElsaOptionsBuilder options)
    {
        options
            .AddActivity<SendMessage>();

        return options;
    }
}
