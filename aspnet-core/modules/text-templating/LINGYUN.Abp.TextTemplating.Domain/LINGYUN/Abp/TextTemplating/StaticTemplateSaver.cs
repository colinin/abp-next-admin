using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.TextTemplating;

public class StaticTemplateSaver : IStaticTemplateSaver, ITransientDependency
{
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected ITextTemplateRepository TextTemplateRepository { get; }
    protected ITextTemplateDefinitionRepository TemplateDefinitionRepository { get; }
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IStaticTemplateDefinitionStore StaticTemplateDefinitionStore { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }
    protected ITemplateContentProvider TemplateContentProvider { get; }

    public StaticTemplateSaver(
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions,
        IGuidGenerator guidGenerator, 
        IAbpDistributedLock distributedLock,
        ITextTemplateRepository textTemplateRepository,
        ITextTemplateDefinitionRepository templateDefinitionRepository,
        IStaticTemplateDefinitionStore staticTemplateDefinitionStore, 
        ILocalizableStringSerializer localizableStringSerializer,
        ITemplateContentProvider templateContentProvider)
    {
        CacheOptions = cacheOptions.Value;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        TextTemplateRepository = textTemplateRepository;
        TemplateDefinitionRepository = templateDefinitionRepository;
        TemplatingCachingOptions = templatingCachingOptions.Value;
        StaticTemplateDefinitionStore = staticTemplateDefinitionStore;
        LocalizableStringSerializer = localizableStringSerializer;
        TemplateContentProvider = templateContentProvider;
    }

    [UnitOfWork]
    public async virtual Task SaveDefinitionTemplateAsync()
    {
        if (TemplatingCachingOptions.SaveStaticTemplateDefinitionToDatabase)
        {
            await using var commonLockHandle = await DistributedLock
                .TryAcquireAsync(GetCommonDistributedLockKey(), TemplatingCachingOptions.TemplateDefinitionsCacheStampTimeOut);

            if (commonLockHandle == null)
            {
                return;
            }

            var templateDefinitions = await StaticTemplateDefinitionStore.GetAllAsync();

            var saveNewTemplateDefinitionRecords = new List<TextTemplateDefinition>();

            foreach (var templateDefinition in templateDefinitions)
            {
                if (await TemplateDefinitionRepository.FindByNameAsync(templateDefinition.Name) != null)
                {
                    continue;
                }

                var templateDefinitionRecord = new TextTemplateDefinition(
                    GuidGenerator.Create(),
                    templateDefinition.Name,
                    LocalizableStringSerializer.Serialize(templateDefinition.DisplayName),
                    templateDefinition.IsLayout,
                    templateDefinition.Layout,
                    templateDefinition.IsInlineLocalized,
                    templateDefinition.DefaultCultureName,
                    templateDefinition.LocalizationResourceName,
                    templateDefinition.RenderEngine)
                {
                    IsStatic = true
                };

                foreach (var property in templateDefinition.Properties)
                {
                    templateDefinitionRecord.SetProperty(property.Key, property.Value);
                }

                saveNewTemplateDefinitionRecords.Add(templateDefinitionRecord);
            }

            await TemplateDefinitionRepository.InsertManyAsync(saveNewTemplateDefinitionRecords);
        }
    }

    [UnitOfWork]
    public async virtual Task SaveTemplateContentAsync()
    {
        var saveNewTemplates = new List<TextTemplate>();
        var templateDefinitions = await StaticTemplateDefinitionStore.GetAllAsync();
        foreach (var templateDefinition in templateDefinitions)
        {
            var culture = templateDefinition.IsInlineLocalized ? null : templateDefinition.DefaultCultureName;
            if (await TextTemplateRepository.FindByNameAsync(templateDefinition.Name, culture) != null)
            {
                continue;
            }

            var content = await TemplateContentProvider.GetContentOrNullAsync(templateDefinition, culture);
            if (content.IsNullOrWhiteSpace())
            {
                continue;
            }
            var textTemplate = new TextTemplate(
                GuidGenerator.Create(),
                templateDefinition.Name,
                LocalizableStringSerializer.Serialize(templateDefinition.DisplayName),
                content,
                culture);
            saveNewTemplates.Add(textTemplate);
        }

        await TextTemplateRepository.InsertManyAsync(saveNewTemplates);
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpTemplateDefinitionStaticSaverLock";
    }
}
