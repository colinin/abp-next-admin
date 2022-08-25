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

            if (FakeFeatureOptions.FeatureMaps.TryGetValue(name, out var featureFunc))
            {
                return Task.FromResult(featureFunc(feature));
            }

            return Task.FromResult(feature.DefaultValue);
        }
    }
}
