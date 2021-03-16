using LINGYUN.Abp.Features.Client;
using System.Threading.Tasks;
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


        protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
        {
            if (providerKey != null)
            {
                return base.NormalizeProviderKeyAsync(providerKey);
            }

            return Task.FromResult(CurrentClient.Id);
        }
    }
}
