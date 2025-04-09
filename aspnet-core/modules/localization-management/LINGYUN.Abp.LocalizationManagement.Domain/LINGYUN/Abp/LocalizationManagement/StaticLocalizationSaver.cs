using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.LocalizationManagement;

public class StaticLocalizationSaver : IStaticLocalizationSaver, ITransientDependency
{
    protected ILogger<StaticLocalizationSaver> Logger { get; }

    protected IDistributedCache Cache { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected AbpLocalizationManagementOptions LocalizationManagementOptions { get; }

    protected ILanguageRepository LanguageRepository { get; }
    protected IResourceRepository ResourceRepository { get; }
    protected ITextRepository TextRepository { get; }

    public StaticLocalizationSaver(
        ILogger<StaticLocalizationSaver> logger,
        IDistributedCache cache, 
        IGuidGenerator guidGenerator, 
        IAbpDistributedLock distributedLock,
        IUnitOfWorkManager unitOfWorkManager,
        IApplicationInfoAccessor applicationInfoAccessor,
        ICancellationTokenProvider cancellationTokenProvider,
        IStringLocalizerFactory stringLocalizerFactory,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<AbpLocalizationOptions> localizationOptions, 
        IOptions<AbpLocalizationManagementOptions> localizationManagementOptions, 
        ILanguageRepository languageRepository, 
        IResourceRepository resourceRepository, 
        ITextRepository textRepository)
    {
        Logger = logger;
        Cache = cache;
        GuidGenerator = guidGenerator;
        DistributedLock = distributedLock;
        UnitOfWorkManager = unitOfWorkManager;
        CacheOptions = cacheOptions.Value;
        ApplicationInfoAccessor = applicationInfoAccessor;
        CancellationTokenProvider = cancellationTokenProvider;
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationOptions = localizationOptions.Value;
        LocalizationManagementOptions = localizationManagementOptions.Value;
        LanguageRepository = languageRepository;
        ResourceRepository = resourceRepository;
        TextRepository = textRepository;
    }

    public async virtual Task SaveAsync()
    {
        if (!LocalizationManagementOptions.SaveStaticLocalizationsToDatabase)
        {
            return;
        }

        Logger.LogDebug("Waiting to acquire the distributed lock for saving static localizations...");

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(GetApplicationDistributedLockKey());
        if (applicationLockHandle == null)
        {
            return;
        }

        using var unitOfWork = UnitOfWorkManager.Begin(true, true);
        try
        {
            await SaveLanguagesAsync();
            await SaveResourcesAsync();
            await SaveTextsAsync();
        }
        catch (Exception ex)
        {
            Logger.LogInformation("Filed to save static localizations.");
            Logger.LogWarning(ex.ToString());
            try
            {
                await unitOfWork.RollbackAsync();
            }
            catch
            {
                Logger.LogInformation("Filed to rollback saving static localizations.");
            }
            throw;
        }

        await unitOfWork.CompleteAsync();

        Logger.LogInformation("Completed to save static localizations.");
    }

    private async Task SaveLanguagesAsync()
    {
        var languageHashKey = GetApplicationLanguageHashKey();
        var languageHashCache = await Cache.GetStringAsync(languageHashKey, CancellationTokenProvider.Token);
        var languageHash = CalculateLanguagesHash(LocalizationOptions.Languages);
        if (languageHashCache != languageHash)
        {
            var newLanguages = new List<Language>();
            foreach (var language in LocalizationOptions.Languages)
            {
                if (await LanguageRepository.FindByCultureNameAsync(language.CultureName, cancellationToken: CancellationTokenProvider.Token) == null)
                {
                    newLanguages.Add(
                        new Language(
                            GuidGenerator.Create(),
                            language.CultureName,
                            language.UiCultureName,
                            language.DisplayName,
                            language.TwoLetterISOLanguageName));
                }
            }

            if (newLanguages.Any())
            {
                Logger.LogInformation("Saved {0} new languages.", newLanguages.Count);

                await LanguageRepository.InsertManyAsync(newLanguages, cancellationToken: CancellationTokenProvider.Token);
            }

            await Cache.SetStringAsync(languageHashKey, languageHash, CancellationTokenProvider.Token);
        }
    }

    private async Task SaveResourcesAsync()
    {
        var resourceHashKey = GetApplicationResourceHashKey();
        var resourceHashCache = await Cache.GetStringAsync(resourceHashKey, CancellationTokenProvider.Token);
        var resourceHash = CalculateResourceHash(LocalizationOptions.Resources.Select(x => x.Value).ToList());
        if (resourceHashCache != resourceHash)
        {
            var newResources = new List<Resource>();
            foreach (var resource in LocalizationOptions.Resources)
            {
                if (await ResourceRepository.FindByNameAsync(resource.Key, cancellationToken: CancellationTokenProvider.Token) == null)
                {
                    newResources.Add(
                        new Resource(
                            GuidGenerator.Create(),
                            resource.Value.ResourceName,
                            resource.Value.ResourceName,
                            resource.Value.ResourceName,
                            resource.Value.DefaultCultureName));
                }
            }
            if (newResources.Any())
            {
                Logger.LogInformation("Saved {0} new resources.", newResources.Count);

                await ResourceRepository.InsertManyAsync(newResources, cancellationToken: CancellationTokenProvider.Token);
            }

            await Cache.SetStringAsync(resourceHashKey, resourceHash, CancellationTokenProvider.Token);
        }
    }

    private async Task SaveTextsAsync()
    {
        var createTexts = new List<Text>();
        var updateTexts = new List<Text>();

        var localizationResources = LocalizationOptions.Resources.Values.OfType<LocalizationResource>().ToArray();

        var languageResourceTexts = new Dictionary<string, Dictionary<string, Dictionary<string, LocalizedString>>>();

        foreach (var language in LocalizationOptions.Languages)
        {
            var resourceTexts = new Dictionary<string, Dictionary<string, LocalizedString>>();
            foreach (var resource in localizationResources)
            {
                using (CultureHelper.Use(language.CultureName, language.UiCultureName))
                {
                    var stringLocalizer = StringLocalizerFactory.Create(resource.ResourceType);

                    var localizedStrings = await stringLocalizer.GetAllStringsAsync(false, false, false);

                    var textHashKey = GetApplicationTextHashKey(resource.ResourceName, language.CultureName);
                    var textHashCache = await Cache.GetStringAsync(textHashKey, CancellationTokenProvider.Token);
                    var textHash = CalculateTextsHash(localizedStrings);
                    if (textHashCache == textHash)
                    {
                        continue;
                    }

                    var savedLocalizedStrings = await TextRepository.GetListAsync(
                        resource.ResourceName,
                        language.CultureName);

                    foreach (var localizedString in localizedStrings)
                    {
                        var findLocalizedString = savedLocalizedStrings.FirstOrDefault(x => x.Key == localizedString.Name);
                        if (findLocalizedString == null)
                        {
                            createTexts.Add(
                                new Text(
                                    resource.ResourceName,
                                    language.CultureName,
                                    localizedString.Name,
                                    localizedString.Value));
                            continue;
                        }

                        if (!string.Equals(findLocalizedString.Value, localizedString.Value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            findLocalizedString.SetValue(localizedString.Value);
                            updateTexts.Add(findLocalizedString);
                        }
                    }

                    await Cache.SetStringAsync(textHashKey, textHash, CancellationTokenProvider.Token);
                }
            }

            languageResourceTexts[language.CultureName] = resourceTexts;
        }

        if (createTexts.Any())
        {
            Logger.LogInformation("Saved {0} new texts.", createTexts.Count);

            await TextRepository.InsertManyAsync(createTexts, cancellationToken: CancellationTokenProvider.Token);
        }
        if (updateTexts.Any())
        {
            Logger.LogInformation("Update {0} changed texts.", updateTexts.Count);

            await TextRepository.UpdateManyAsync(updateTexts, cancellationToken: CancellationTokenProvider.Token);
        }
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpLocalizationsUpdateLock";
    }

    private string GetApplicationResourceHashKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpLocalizationResourcesHash";
    }

    private string GetApplicationLanguageHashKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpLocalizationLanguagesHash";
    }
    private string GetApplicationTextHashKey(string resourceName, string cultureName)
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_{resourceName}_{cultureName}_AbpLocalizationTextsHash";
    }

    private static string CalculateResourceHash(List<LocalizationResourceBase> localizationResources)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("LocalizationResources:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(localizationResources));

        return stringBuilder
            .ToString()
            .ToMd5();
    }

    private static string CalculateLanguagesHash(List<LanguageInfo> languageInfos)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("LocalizationLanguages:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(languageInfos));

        return stringBuilder
            .ToString()
            .ToMd5();
    }

    private static string CalculateTextsHash(IEnumerable<LocalizedString> localizers)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("LocalizationTexts:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(
            localizers.Select(x => new NameValue(x.Name, x.Value)).ToList()));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
