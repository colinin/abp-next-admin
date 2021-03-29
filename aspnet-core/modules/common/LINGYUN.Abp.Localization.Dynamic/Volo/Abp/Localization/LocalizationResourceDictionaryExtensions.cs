using LINGYUN.Abp.Localization.Dynamic;
using System;
using System.Linq;

namespace Volo.Abp.Localization
{
    public static class LocalizationResourceDictionaryExtensions
    {
        public static LocalizationResourceDictionary AddDynamic(
            this LocalizationResourceDictionary resources,
            params Type[] ignoreResourceTypes)
        {
            foreach (var resource in resources)
            {
                if (ShouldIgnoreType(resource.Key, ignoreResourceTypes))
                {
                    continue;
                }
                if (ShouldIgnoreType(resource.Value))
                {
                    continue;
                }
                resource.Value.AddDynamic();
            }
            return resources;
        }

        private static bool ShouldIgnoreType(Type resourceType, params Type[] ignoreResourceTypes)
        {
            if (ignoreResourceTypes == null)
            {
                return false;
            }
            return ignoreResourceTypes.Any(x => x == resourceType);
        }

        private static bool ShouldIgnoreType(LocalizationResource resource)
        {
            return resource.Contributors.Exists(x => x is DynamicLocalizationResourceContributor);
        }
    }
}
