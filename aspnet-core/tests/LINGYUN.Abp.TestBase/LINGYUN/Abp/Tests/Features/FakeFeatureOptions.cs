using System;
using System.Collections.Generic;
using Volo.Abp.Features;

namespace LINGYUN.Abp.Tests.Features
{
    public class FakeFeatureOptions
    {
        public IDictionary<string, Func<FeatureDefinition, string>> FeatureMaps { get; }
        public FakeFeatureOptions()
        {
            FeatureMaps = new Dictionary<string, Func<FeatureDefinition, string>>();
        }

        public void Map(string featureName, Func<FeatureDefinition, string> func)
        {
            FeatureMaps.AddIfNotContains(new KeyValuePair<string, Func<FeatureDefinition, string>>(featureName, func));
        }
    }
}
