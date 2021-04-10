using JetBrains.Annotations;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.Dapr.Actors
{
    public static class DaprActorConfigurationExtensions
    {
        public const string IdentityClientName = "IdentityClient";
        public const string UseCurrentAccessTokenName = "UseCurrentAccessToken";

        [CanBeNull]
        public static string GetIdentityClient([NotNull] this DaprActorConfiguration configuration)
        {
            Check.NotNullOrEmpty(configuration, nameof(configuration));

            return configuration.GetOrDefault(IdentityClientName);
        }

        public static DaprActorConfiguration SetIdentityClient([NotNull] this DaprActorConfiguration configuration, [CanBeNull] string value)
        {
            configuration[IdentityClientName] = value;
            return configuration;
        }

        [CanBeNull]
        public static bool? GetUseCurrentAccessToken([NotNull] this DaprActorConfiguration configuration)
        {
            Check.NotNullOrEmpty(configuration, nameof(configuration));

            var value = configuration.GetOrDefault(UseCurrentAccessTokenName);
            if (value == null)
            {
                return null;
            }

            return bool.Parse(value);
        }

        public static DaprActorConfiguration SetUseCurrentAccessToken([NotNull] this DaprActorConfiguration configuration, [CanBeNull] bool? value)
        {
            if (value == null)
            {
                configuration.Remove(UseCurrentAccessTokenName);
            }
            else
            {
                configuration[UseCurrentAccessTokenName] = value.Value.ToString().ToLowerInvariant();
            }

            return configuration;
        }
    }
}
