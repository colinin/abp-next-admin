using LINGYUN.Abp.Localization.Persistence;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.LocalizationManagement;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(ILocalizationPersistenceWriter),
    typeof(LocalizationManagementPersistenceWriter))]
public class LocalizationManagementPersistenceWriter : ILocalizationPersistenceWriter, ITransientDependency
{
    protected IDistributedCache Cache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }

    protected IGuidGenerator GuidGenerator { get; }
    protected ILanguageRepository LanguageRepository { get; }
    protected ITextRepository TextRepository { get; }
    protected IResourceRepository ResourceRepository { get; }

    public LocalizationManagementPersistenceWriter(
        IDistributedCache cache,
        IGuidGenerator guidGenerator,
        IAbpDistributedLock distributedLock,
        ITextRepository textRepository,
        ILanguageRepository languageRepository, 
        IResourceRepository resourceRepository,
        IApplicationInfoAccessor applicationInfoAccessor,
        IOptions<AbpDistributedCacheOptions> cacheOptions)
    {
        Cache = cache;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        LanguageRepository = languageRepository;
        TextRepository = textRepository;
        ResourceRepository = resourceRepository;
        ApplicationInfoAccessor = applicationInfoAccessor;
        CacheOptions = cacheOptions.Value;
    }

    public async virtual Task<IEnumerable<string>> GetExistsTextsAsync(
        string resourceName, 
        string cultureName, 
        IEnumerable<string> keys, 
        CancellationToken cancellationToken = default)
    {
        if (!await ShouldCalculateTextsHash(keys, cancellationToken))
        {
            return keys;
        }

        return await TextRepository.GetExistsKeysAsync(
            resourceName,
            cultureName,
            keys,
            cancellationToken);
    }



    public async virtual Task<bool> WriteLanguageAsync(
        LanguageInfo language, 
        CancellationToken cancellationToken = default)
    {
        var commonDistributedLockKey = GetCommonDistributedLockKey("Language", language.CultureName);
        await using var lockHandle = await DistributedLock.TryAcquireAsync(commonDistributedLockKey);
        if (lockHandle == null)
        {
            return false;
        }

        if (await LanguageRepository.FindByCultureNameAsync(language.CultureName, cancellationToken) == null)
        {
            await LanguageRepository.InsertAsync(
                new Language(
                    GuidGenerator.Create(),
                    language.CultureName,
                    language.UiCultureName,
                    language.DisplayName,
                    language.FlagIcon),
                autoSave: true,
                cancellationToken: cancellationToken);
        }

        return true;
    }

    public async virtual Task<bool> WriteResourceAsync(
        LocalizationResourceBase resource, 
        CancellationToken cancellationToken = default)
    {
        var commonDistributedLockKey = GetCommonDistributedLockKey("Resource", resource.ResourceName);
        await using var lockHandle = await DistributedLock.TryAcquireAsync(commonDistributedLockKey);
        if (lockHandle == null)
        {
            return false;
        }

        if (await ResourceRepository.FindByNameAsync(resource.ResourceName, cancellationToken) == null)
        {
            await ResourceRepository.InsertAsync(
                new Resource(
                    GuidGenerator.Create(),
                    resource.ResourceName,
                    resource.ResourceName,
                    resource.ResourceName,
                    resource.DefaultCultureName),
                autoSave: true,
                cancellationToken: cancellationToken);
        }

        return true;
    }

    public async virtual Task<bool> WriteTextsAsync(
        IEnumerable<LocalizableStringText> texts, 
        CancellationToken cancellationToken = default)
    {
        if (!await ShouldCalculateTextsHash(texts.Select(x => x.Name), cancellationToken))
        {
            return false;
        }

        var cacheKey = GetApplicationHashCacheKey();
        var currentHash = CalculateTextsHash(texts.Select(x => x.Name));

        var savedTexts = texts.Select(text =>
            new Text(
                text.ResourceName,
                text.CultureName,
                text.Name,
                text.Value));

        await TextRepository.InsertManyAsync(
            savedTexts,
            autoSave: true, 
            cancellationToken: cancellationToken);

        await Cache.SetStringAsync(
            cacheKey,
            currentHash,
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            },
            cancellationToken);

        return true;
    }

    private async Task<bool> ShouldCalculateTextsHash(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, cancellationToken);

        var currentHash = CalculateTextsHash(texts);

        if (cachedHash == currentHash)
        {
            return false;
        }

        return cachedHash != currentHash;
    }

    private static string CalculateTextsHash(IEnumerable<string> texts)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("LocalizableStrings:");
        stringBuilder.Append(texts.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }

    private string GetCommonDistributedLockKey(
        string lockResourceName,
        string lockResourceKey)
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpLocalizationWriter_{lockResourceName}_{lockResourceKey}_Lock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpLocalizationHash";
    }
}
