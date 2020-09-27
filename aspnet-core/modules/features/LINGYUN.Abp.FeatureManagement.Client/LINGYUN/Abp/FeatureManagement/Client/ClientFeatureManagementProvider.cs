using LINGYUN.Abp.Features.Client;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.FeatureManagement;

namespace LINGYUN.Abp.FeatureManagement.Client
{

    public class ClientFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
    {
        public override string Name => ClientFeatureValueProvider.ProviderName;

        protected ICurrentClient CurrentClient;

        public ClientFeatureManagementProvider(
            ICurrentClient currentClient,
            IFeatureManagementStore store)
            : base(store)
        {
            CurrentClient = currentClient;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentClient.Id;
        }
    }
}
