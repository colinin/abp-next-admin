using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

[Dependency(ReplaceServices = true)]
public class LocalizationLanguageProvider : ILanguageProvider, ITransientDependency
{
    protected ILocalizationLanguageStoreCache LocalizationLanguageStore { get; }
    public LocalizationLanguageProvider(ILocalizationLanguageStoreCache localizationLanguageStore)
    {
        LocalizationLanguageStore = localizationLanguageStore;
    }

    public async virtual Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        return await LocalizationLanguageStore.GetLanguagesAsync();
    }
}
