using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Http.Client;

namespace LINGYUN.Abp.Dapr.Client;

public static class DaprRemoteServiceConfigurationExtensions
{
    public const string AppId = "AppId";

    [NotNull]
    public static string GetAppId([NotNull] this RemoteServiceConfiguration configuration)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));

        return configuration.GetOrDefault(AppId) ?? throw new AbpException($"Could not get AppId for RemoteServices Configuration.");
    }

    public static RemoteServiceConfiguration SetAppId([NotNull] this RemoteServiceConfiguration configuration, [NotNull] string value)
    {
        Check.NotNullOrEmpty(configuration, nameof(configuration));
        Check.NotNullOrEmpty(value, nameof(value));

        configuration[AppId] = value;
        return configuration;
    }
}
