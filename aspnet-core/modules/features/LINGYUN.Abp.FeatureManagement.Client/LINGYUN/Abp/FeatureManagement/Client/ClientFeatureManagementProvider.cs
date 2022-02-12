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

        private readonly ICurrentClient _currentClient;

        public ClientFeatureManagementProvider(
            ICurrentClient currentClient,
            IFeatureManagementStore store)
            : base(store)
        {
            _currentClient = currentClient;
        }


        protected override Task<string> NormalizeProviderKeyAsync(string providerKey)
        {
            if (providerKey != null)
            {
                return base.NormalizeProviderKeyAsync(providerKey);
            }

            return Task.FromResult(_currentClient.Id);
        }
    }
}
