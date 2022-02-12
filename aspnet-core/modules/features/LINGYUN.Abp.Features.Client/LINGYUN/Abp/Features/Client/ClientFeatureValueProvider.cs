using System.Threading.Tasks;
using Volo.Abp.Clients;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Features.Client
{
    public class ClientFeatureValueProvider : FeatureValueProvider
    {
        public const string ProviderName = "C";

        public override string Name => ProviderName;

        private readonly ICurrentClient _currentClient;

        public ClientFeatureValueProvider(
            IFeatureStore featureStore,
            ICurrentClient currentClient)
            : base(featureStore)
        {
            _currentClient = currentClient;
        }

        public override async Task<string> GetOrNullAsync(FeatureDefinition feature)
        {
            if (!_currentClient.IsAuthenticated)
            {
                return null;
            }

            return await FeatureStore.GetOrNullAsync(feature.Name, Name, _currentClient.Id);
        }
    }
}
