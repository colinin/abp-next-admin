using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Tests.Features
{
    public class FakeFeatureStore : IFeatureStore
    {
        protected FakeFeatureOptions FakeFeatureOptions { get; }
        protected IFeatureDefinitionManager FeatureDefinitionManager { get; }

        public FakeFeatureStore(
            IOptions<FakeFeatureOptions> options,
            IFeatureDefinitionManager featureDefinitionManager)
        {
            FakeFeatureOptions = options.Value;
            FeatureDefinitionManager = featureDefinitionManager;
        }

        public Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            var feature = FeatureDefinitionManager.Get(name);

            var featureFunc = FakeFeatureOptions.FeatureMaps[name];

            return Task.FromResult(featureFunc(feature));
        }
    }
}
