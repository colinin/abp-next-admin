using JetBrains.Annotations;
using LINGYUN.Abp.Features.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.FeatureManagement;

namespace LINGYUN.Abp.FeatureManagement
{
    public static class ClientFeatureManagerExtensions
    {
        public static Task<string> GetOrNullForClientAsync(this IFeatureManager featureManager, [NotNull] string name, string clientId, bool fallback = true)
        {
            return featureManager.GetOrNullAsync(name, ClientFeatureValueProvider.ProviderName, clientId, fallback);
        }

        public static Task<List<FeatureNameValue>> GetAllForClientAsync(this IFeatureManager featureManager, string clientId, bool fallback = true)
        {
            return featureManager.GetAllAsync(ClientFeatureValueProvider.ProviderName, clientId, fallback);
        }

        public static Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderForClientAsync(this IFeatureManager featureManager, [NotNull] string name, string clientId, bool fallback = true)
        {
            return featureManager.GetOrNullWithProviderAsync(name, ClientFeatureValueProvider.ProviderName, clientId, fallback);
        }

        public static Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderForClientAsync(this IFeatureManager featureManager, string clientId, bool fallback = true)
        {
            return featureManager.GetAllWithProviderAsync(ClientFeatureValueProvider.ProviderName, clientId, fallback);
        }

        public static Task SetForEditionAsync(this IFeatureManager featureManager, string clientId, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return featureManager.SetAsync(name, value, ClientFeatureValueProvider.ProviderName, clientId, forceToSet);
        }
    }
}
