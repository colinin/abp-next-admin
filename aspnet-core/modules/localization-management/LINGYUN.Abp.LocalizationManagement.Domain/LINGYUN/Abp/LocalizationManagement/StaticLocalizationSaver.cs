using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

public class StaticLocalizationSaver : IStaticLocalizationSaver, ITransientDependency
{
    protected ILogger<StaticLocalizationSaver> Logger { get; }
    protected IDistributedEventBus DistributedEventBus { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected AbpLocalizationManagementOptions LocalizationManagementOptions { get; }
    protected IDistributedCache<DynamicLanguageInitializerCacheItem> DynamicLanguageCache { get; }
    protected IDistributedCache<DynamicResourceInitializerCacheItem> DynamicResourceCache { get; }
    protected IDistributedCache<DynamicTextInitializerCacheItem> DynamicTextCache { get; }

    public StaticLocalizationSaver(
        ILogger<StaticLocalizationSaver> logger,
        IDistributedEventBus distributedEventBus, 
        IStringLocalizerFactory stringLocalizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IOptions<AbpLocalizationManagementOptions> localizationManagementOptions, 
        IDistributedCache<DynamicLanguageInitializerCacheItem> dynamicLanguageCache,
        IDistributedCache<DynamicResourceInitializerCacheItem> dynamicResourceCache,
        IDistributedCache<DynamicTextInitializerCacheItem> dynamicTextCache)
    {
        Logger = logger;
        DistributedEventBus = distributedEventBus;
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationOptions = localizationOptions.Value;
        LocalizationManagementOptions = localizationManagementOptions.Value;
        DynamicLanguageCache = dynamicLanguageCache;
        DynamicResourceCache = dynamicResourceCache;
        DynamicTextCache = dynamicTextCache;
    }

    public async virtual Task SaveAsync()
    {
        if (!LocalizationManagementOptions.SaveStaticLocalizationsToDatabase)
        {
            return;
        }


        var languageCacheItems = new Dictionary<string, DynamicLanguageInitializerCacheItem>();
        var resourceCacheItems = new Dictionary<string, DynamicResourceInitializerCacheItem>();
        var textCacheItems = new Dictionary<string, DynamicTextInitializerCacheItem>();

        var localizationResources = LocalizationOptions.Resources.Values.OfType<LocalizationResource>().ToArray();
        foreach (var language in LocalizationOptions.Languages)
        {
            if (!languageCacheItems.Any(x => x.Value.CultureName == language.CultureName))
            {
                languageCacheItems.Add(
                    Guid.NewGuid().ToString(),
                    new DynamicLanguageInitializerCacheItem(
                        language.CultureName,
                        language.UiCultureName,
                        language.DisplayName,
                        language.TwoLetterISOLanguageName));
            }

            foreach (var resource in localizationResources)
            {
                if (!resourceCacheItems.Any(x => x.Value.ResourceName == resource.ResourceName))
                {
                    resourceCacheItems.Add(
                        Guid.NewGuid().ToString(),
                        new DynamicResourceInitializerCacheItem(
                            resource.ResourceName,
                            resource.DefaultCultureName));
                }

                using (CultureHelper.Use(language.CultureName, language.UiCultureName))
                {
                    var stringLocalizer = StringLocalizerFactory.Create(resource.ResourceType);

                    var localizedStrings = stringLocalizer.GetAllStrings(false, false, false);

                    if (!localizedStrings.Any())
                    {
                        continue;
                    }
                    var localizedStringsDic = new Dictionary<string, string>();

                    foreach (var localizedString in localizedStrings)
                    {
                        localizedStringsDic.TryAdd(localizedString.Name, localizedString.Value);
                    }

                    textCacheItems.Add(
                        Guid.NewGuid().ToString(),
                        new DynamicTextInitializerCacheItem(
                            resource.ResourceName,
                            language.CultureName,
                            localizedStringsDic));
                }
            }
        }

        await DynamicLanguageCache.SetManyAsync(languageCacheItems);
        await DynamicResourceCache.SetManyAsync(resourceCacheItems);
        await DynamicTextCache.SetManyAsync(textCacheItems);

        await DistributedEventBus.PublishAsync(
            new DynamicLanguageInitializerEto(languageCacheItems.Keys.ToArray()));
        await DistributedEventBus.PublishAsync(
            new DynamicResourceInitializerEto(resourceCacheItems.Keys.ToArray()));
        await DistributedEventBus.PublishAsync(
            new DynamicTextInitializerEto(textCacheItems.Keys.ToArray()));
    }
}
