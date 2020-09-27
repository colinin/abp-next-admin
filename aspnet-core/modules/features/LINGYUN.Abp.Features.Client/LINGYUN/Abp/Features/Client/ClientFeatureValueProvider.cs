using System.Threading.Tasks;
using Volo.Abp.Clients;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Features.Client
{
    public class ClientFeatureValueProvider : FeatureValueProvider
    {
        public const string ProviderName = "C";

        public override string Name => ProviderName;

        protected ICurrentClient CurrentClient;

        public ClientFeatureValueProvider(
            IFeatureStore featureStore,
            ICurrentClient currentClient)
            : base(featureStore)
        {
            CurrentClient = currentClient;
        }

        public override async Task<string> GetOrNullAsync(FeatureDefinition feature)
        {
            if (!CurrentClient.IsAuthenticated)
            {
                return null;
            }

            return await FeatureStore.GetOrNullAsync(feature.Name, Name, CurrentClient.Id);
        }
    }
}
