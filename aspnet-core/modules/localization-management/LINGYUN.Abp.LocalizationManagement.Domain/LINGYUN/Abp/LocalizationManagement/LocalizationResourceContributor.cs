using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class LocalizationResourceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;
    protected LocalizationResourceBase Resource { get; private set; }
    protected ILocalizationTextStoreCache LocalizationTextStoreCache { get; private set; }
    protected ILocalizationLanguageStoreCache LocalizationLanguageStoreCache { get; private set; }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        LocalizationTextStoreCache.Fill(Resource, cultureName, dictionary);
    }

    public async virtual Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        await LocalizationTextStoreCache.FillAsync(Resource, cultureName, dictionary);
    }

    public virtual LocalizedString GetOrNull(string cultureName, string name)
    {
        return LocalizationTextStoreCache.GetOrNull(Resource, cultureName, name);
    }

    public async virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var languageInfos = await LocalizationLanguageStoreCache.GetLanguagesAsync();

        return languageInfos.Select(x => x.CultureName);
    }

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        Resource = context.Resource;
        LocalizationTextStoreCache = context.ServiceProvider.GetRequiredService<ILocalizationTextStoreCache>();
        LocalizationLanguageStoreCache = context.ServiceProvider.GetRequiredService<ILocalizationLanguageStoreCache>();
    }
}
