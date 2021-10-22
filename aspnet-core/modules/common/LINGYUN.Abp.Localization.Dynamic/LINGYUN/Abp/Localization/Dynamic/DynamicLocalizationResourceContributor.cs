using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class DynamicLocalizationResourceContributor : ILocalizationResourceContributor
    {
        private readonly string _resourceName;

        private AbpLocalizationDynamicOptions _options;

        public DynamicLocalizationResourceContributor(string resourceName)
        {
            _resourceName = resourceName;
        }

        public virtual void Initialize(LocalizationResourceInitializationContext context)
        {
            _options = context.ServiceProvider.GetService<IOptions<AbpLocalizationDynamicOptions>>().Value;
        }

        public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
        {
            GetDictionaries().GetOrDefault(cultureName)?.Fill(dictionary);
        }

        public virtual LocalizedString GetOrNull(string cultureName, string name)
        {
            return GetDictionaries().GetOrDefault(cultureName)?.GetOrNull(name);
        }

        protected virtual Dictionary<string, ILocalizationDictionary> GetDictionaries()
        {
            return _options.LocalizationDictionary
                .GetOrAdd(_resourceName, () => new Dictionary<string, ILocalizationDictionary>());
        }
    }
}
