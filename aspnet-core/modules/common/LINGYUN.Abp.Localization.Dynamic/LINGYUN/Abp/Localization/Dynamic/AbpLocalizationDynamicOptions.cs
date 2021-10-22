using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class AbpLocalizationDynamicOptions
    {
        internal LocalizationDictionary LocalizationDictionary { get; }

        public AbpLocalizationDynamicOptions()
        {
            LocalizationDictionary = new LocalizationDictionary();
        }

        internal void AddOrUpdate(string resourceName, Dictionary<string, ILocalizationDictionary> dictionares)
        {
            var _dictionares = LocalizationDictionary
                .GetOrAdd(resourceName, () => new Dictionary<string, ILocalizationDictionary>());
            _dictionares.AddIfNotContains(dictionares);
        }
    }
}
