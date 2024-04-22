using Microsoft.Extensions.Options;
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

public class StaticTemplateDefinitionSaver : IStaticTemplateDefinitionSaver, ITransientDependency
{
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected ITextTemplateDefinitionRepository TemplateDefinitionRepository { get; }
    protected AbpTextTemplatingCachingOptions TemplatingCachingOptions { get; }
    protected IStaticTemplateDefinitionStore StaticTemplateDefinitionStore { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public StaticTemplateDefinitionSaver(
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AbpTextTemplatingCachingOptions> templatingCachingOptions,
        IGuidGenerator guidGenerator, 
        IAbpDistributedLock distributedLock,
        ITextTemplateDefinitionRepository templateDefinitionRepository,
        IStaticTemplateDefinitionStore staticTemplateDefinitionStore, 
        ILocalizableStringSerializer localizableStringSerializer)
    {
        CacheOptions = cacheOptions.Value;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        TemplateDefinitionRepository = templateDefinitionRepository;
        TemplatingCachingOptions = templatingCachingOptions.Value;
        StaticTemplateDefinitionStore = staticTemplateDefinitionStore;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    [UnitOfWork]
    public async virtual Task SaveAsync()
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

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpTemplateDefinitionStaticSaverLock";
    }
}
