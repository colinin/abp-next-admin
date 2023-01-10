using LINGYUN.Abp.Localization.Persistence;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.LocalizationManagement;

[Dependency(ReplaceServices = true)]
public class StaticLocalizationSaver : IStaticLocalizationSaver, ITransientDependency
{
    protected ILanguageRepository LanguageRepository { get; }
    protected ITextRepository TextRepository { get; }
    protected IResourceRepository ResourceRepository { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected AbpLocalizationPersistenceOptions LocalizationPersistenceOptions { get; }

    public StaticLocalizationSaver(
        IServiceProvider serviceProvider,
        ILanguageRepository languageRepository, 
        ITextRepository textRepository, 
        IResourceRepository resourceRepository, 
        IStringLocalizerFactory stringLocalizerFactory, 
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IOptions<AbpLocalizationPersistenceOptions> localizationPersistenceOptions)
    {
        ServiceProvider = serviceProvider;
        LanguageRepository = languageRepository;
        TextRepository = textRepository;
        ResourceRepository = resourceRepository;
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationOptions = localizationOptions.Value;
        LocalizationPersistenceOptions = localizationPersistenceOptions.Value;
    }

    [UnitOfWork]
    public async virtual Task SaveAsync()
    {
        var insertNewTexts = new List<Text>();

        foreach (var language in LocalizationOptions.Languages)
        {
            if (await LanguageRepository.FindByCultureNameAsync(language.CultureName) == null)
            {
                await LanguageRepository.InsertAsync(
                    new Language(
                        language.CultureName, 
                        language.UiCultureName, 
                        language.DisplayName, 
                        language.FlagIcon));
            }

            foreach (var resource in LocalizationPersistenceOptions.SaveToPersistenceResources)
            {
                using (CultureHelper.Use(language.CultureName, language.UiCultureName))
                {
                    var localizationResource = LocalizationOptions.Resources.GetOrDefault(resource);
                    if (localizationResource == null)
                    {
                        continue;
                    }

                    var context = new LocalizationResourceInitializationContext(localizationResource, ServiceProvider);

                    if (await ResourceRepository.FindByNameAsync(localizationResource.ResourceName) == null)
                    {
                        await ResourceRepository.InsertAsync(
                            new Resource(
                                localizationResource.ResourceName,
                                localizationResource.ResourceName,
                                localizationResource.ResourceName,
                                localizationResource.DefaultCultureName));
                    }

                    foreach (var contributor in localizationResource.Contributors)
                    {
                        if (contributor.IsDynamic)
                        {
                            continue;
                        }

                        contributor.Initialize(context);
                        var fillTexts = new Dictionary<string, LocalizedString>();

                        await contributor.FillAsync(language.CultureName, fillTexts);

                        var existsKeys = await TextRepository.GetExistsKeysAsync(
                            localizationResource.ResourceName,
                            language.CultureName,
                            fillTexts.Values.Select(x => x.Name));

                        var notExistsKeys = fillTexts.Values.Where(x => !existsKeys.Contains(x.Name));
                        notExistsKeys = notExistsKeys.Where(x => !insertNewTexts.Any(t => t.Key == x.Name));

                        foreach (var notExistsKey in notExistsKeys)
                        {
                            if (!insertNewTexts.Any(x => x.CultureName == language.CultureName && x.Key == notExistsKey.Name))
                            {
                                insertNewTexts.Add(
                                    new Text(
                                        localizationResource.ResourceName,
                                        language.CultureName,
                                        notExistsKey.Name,
                                        notExistsKey.Value));
                            }
                        }
                    }
                }
            }
        }

        if (insertNewTexts.Any())
        {
            await TextRepository.InsertManyAsync(insertNewTexts);
        }
    }
}
