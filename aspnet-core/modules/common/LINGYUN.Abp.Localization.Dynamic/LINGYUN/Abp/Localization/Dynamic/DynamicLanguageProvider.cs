using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(
        typeof(ILanguageProvider),
        typeof(DynamicLanguageProvider))]
    public class DynamicLanguageProvider : ILanguageProvider
    {
        protected ILocalizationStore Store { get; }
        protected AbpLocalizationOptions Options { get; }

        public DynamicLanguageProvider(
            ILocalizationStore store,
            IOptions<AbpLocalizationOptions> options)
        {
            Store = store;
            Options = options.Value;
        }

        public virtual async Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
        {
            var languages = await Store.GetLanguageListAsync();

            if (!languages.Any())
            {
                return Options.Languages;
            }

            return languages
                .Distinct(new LanguageInfoComparer())
                .ToList();
        }
    }
}
