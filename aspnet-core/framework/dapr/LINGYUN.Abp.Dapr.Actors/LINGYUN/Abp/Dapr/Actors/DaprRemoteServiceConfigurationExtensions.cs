using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Http.Client;

namespace LINGYUN.Abp.Dapr.Actors;

public static class DaprRemoteServiceConfigurationExtensions
{
    public const string RequestTimeOut = "DaprTimeout";
    public const string DaprApiToken = "DaprApiToken";

    [CanBeNull]
    public static string GetApiToken([NotNull] this RemoteServiceConfiguration configuration)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));

        return configuration.GetOrDefault(DaprApiToken);
    }

    public static RemoteServiceConfiguration SetApiToken([NotNull] this RemoteServiceConfiguration configuration, [NotNull] string value)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));
        Check.NotNullOrEmpty(value, nameof(value));

        configuration[DaprApiToken] = value;
        return configuration;
    }

    [NotNull]
    public static int GetRequestTimeOut([NotNull] this RemoteServiceConfiguration configuration)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));

        configuration.TryGetValue(RequestTimeOut, out var timeOutValue);
        if (!int.TryParse(timeOutValue ?? "30000", out var timeOut))
        {
            timeOut = 30000;
        }

        return timeOut;
    }

    public static RemoteServiceConfiguration SetRequestTimeOut([NotNull] this RemoteServiceConfiguration configuration, [NotNull] string value)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));
        Check.NotNullOrEmpty(value, nameof(value));

        configuration[RequestTimeOut] = value;
        return configuration;
    }
}
