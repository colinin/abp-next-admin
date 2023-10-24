using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.Localization.Persistence;

[Dependency(ReplaceServices = true)]
public class DefaultStaticLocalizationSaver : IStaticLocalizationSaver, ITransientDependency
{
    protected ILocalizationPersistenceWriter LocalizationPersistenceWriter { get; }

    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected AbpLocalizationPersistenceOptions LocalizationPersistenceOptions { get; }

    public DefaultStaticLocalizationSaver(
        IServiceProvider serviceProvider,
        ILocalizationPersistenceWriter localizationPersistenceWriter,
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IOptions<AbpLocalizationPersistenceOptions> localizationPersistenceOptions)
    {
        ServiceProvider = serviceProvider;
        LocalizationPersistenceWriter = localizationPersistenceWriter;
        LocalizationOptions = localizationOptions.Value;
        LocalizationPersistenceOptions = localizationPersistenceOptions.Value;
    }

    [UnitOfWork]
    public async virtual Task SaveAsync()
    {
        if (!LocalizationPersistenceOptions.SaveStaticLocalizationsToPersistence)
        {
            return;
        }

        var canWriterTexts = new List<LocalizableStringText>();

        foreach (var localizationResource in LocalizationOptions.Resources)
        {
            if (ShouldSaveToPersistence(localizationResource.Value))
            {
                if (!await LocalizationPersistenceWriter.WriteResourceAsync(localizationResource.Value))
                {
                    continue;
                }

                foreach (var language in LocalizationOptions.Languages)
                {
                    if (!await LocalizationPersistenceWriter.WriteLanguageAsync(language))
                    {
                        continue;
                    }

                    using (CultureHelper.Use(language.CultureName, language.UiCultureName))
                    {
                        await FillCanWriterTextxAsync(localizationResource.Value, language, canWriterTexts);
                    }
                }
            }
        }

        if (canWriterTexts.Any())
        {
            await LocalizationPersistenceWriter.WriteTextsAsync(canWriterTexts);
        }
    }

    protected virtual bool ShouldSaveToPersistence(LocalizationResourceBase localizationResource)
    {
        var saveResource = false;
        if (localizationResource.Contributors.Exists(IsMatchSaveToPersistenceContributor))
        {
            saveResource = true;
        }
        if (!saveResource)
        {
            saveResource = LocalizationPersistenceOptions
                .SaveToPersistenceResources
                .Contains(localizationResource.ResourceName);
        }

        return saveResource;
    }

    protected virtual bool IsMatchSaveToPersistenceContributor(ILocalizationResourceContributor contributor)
    {
        return typeof(LocalizationSaveToPersistenceContributor).IsAssignableFrom(contributor.GetType());
    }

    protected async virtual Task FillCanWriterTextxAsync(
        LocalizationResourceBase localizationResource,
        LanguageInfo language,
        List<LocalizableStringText> canWriterTexts)
    {
        var fillTexts = new Dictionary<string, LocalizedString>();
        var context = new LocalizationResourceInitializationContext(localizationResource, ServiceProvider);
        foreach (var contributor in localizationResource.Contributors)
        {
            if (contributor.IsDynamic)
            {
                continue;
            }

            contributor.Initialize(context);

            await contributor.FillAsync(language.CultureName, fillTexts);
        }

        var existsKeys = await LocalizationPersistenceWriter.GetExistsTextsAsync(
                localizationResource.ResourceName,
                language.CultureName,
                fillTexts.Values.Select(x => x.Name));

        var notExistsKeys = fillTexts.Values.Where(x => !existsKeys.Contains(x.Name));

        foreach (var notExistsKey in notExistsKeys)
        {
            if (!canWriterTexts.Any(x => x.CultureName == language.CultureName && x.Name == notExistsKey.Name))
            {
                canWriterTexts.Add(
                    new LocalizableStringText(
                        localizationResource.ResourceName,
                        language.CultureName,
                        notExistsKey.Name,
                        notExistsKey.Value));
            }
        }
    }
}
