using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement.External;

public class ExternalLocalizationResourceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => false;

    protected LocalizationResourceBase Resource { get; private set; }
    protected IExternalLocalizationStoreCache StoreCache { get; private set; }
    protected IExternalLocalizationTextStoreCache TextStoreCache { get; private set; }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var texts = TextStoreCache.GetTexts(Resource, cultureName);

        foreach (var text in texts)
        {
            dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
        }
    }

    public async virtual Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var texts = await TextStoreCache.GetTextsAsync(Resource, cultureName);

        foreach (var text in texts)
        {
            dictionary[text.Key] = new LocalizedString(text.Key, text.Value);
        }
    }

    public virtual LocalizedString GetOrNull(string cultureName, string name)
    {
        var texts = TextStoreCache.GetTexts(Resource, cultureName);

        var text = texts.GetOrDefault(name);
        if (text == null)
        {
            return null;
        }

        return new LocalizedString(name, text);
    }

    public async virtual Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var cacheItem = await StoreCache.GetResourceOrNullAsync(Resource.ResourceName);

        if (cacheItem == null || !cacheItem.IsEnabled)
        {
            return Array.Empty<string>();
        }

        return cacheItem.SupportedCultures;
    }

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        Resource = context.Resource;
        StoreCache = context.ServiceProvider.GetRequiredService<IExternalLocalizationStoreCache>();
        TextStoreCache = context.ServiceProvider.GetRequiredService<IExternalLocalizationTextStoreCache>();
    }
}
