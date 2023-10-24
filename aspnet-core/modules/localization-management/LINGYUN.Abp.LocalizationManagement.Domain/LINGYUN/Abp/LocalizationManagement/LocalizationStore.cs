using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.LocalizationManagement
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(
        typeof(IExternalLocalizationStore),
        typeof(LocalizationStore))]
    public class LocalizationStore : IExternalLocalizationStore
    {
        protected IServiceProvider ServiceProvider { get; }
        protected ILocalizationStoreCache LocalizationStoreCache { get; }

        public LocalizationStore(
            IServiceProvider serviceProvider,
            ILocalizationStoreCache localizationStoreCache)
        {
            ServiceProvider = serviceProvider;
            LocalizationStoreCache = localizationStoreCache;
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<List<LanguageInfo>> GetLanguageListAsync(
            CancellationToken cancellationToken = default)
        {
            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            return LocalizationStoreCache.GetLanguages().ToList();
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<Dictionary<string, ILocalizationDictionary>> GetLocalizationDictionaryAsync(
            string resourceName,
            CancellationToken cancellationToken = default)
        {
            var dictionaries = new Dictionary<string, ILocalizationDictionary>();

            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            var resource = LocalizationStoreCache.GetResourceOrNull(resourceName);
            
            if (resource == null)
            {
                // 资源不存在或未启用返回空
                return dictionaries;
            }

            var texts = LocalizationStoreCache.GetAllLocalizedStrings(CultureInfo.CurrentCulture.Name);

            foreach (var textGroup in texts)
            {
                var cultureTextDictionaires = new Dictionary<string, LocalizedString>();

                foreach (var text in textGroup.Value)
                {
                    // 本地化名称去重
                    if (!cultureTextDictionaires.ContainsKey(text.Key))
                    {
                        cultureTextDictionaires[text.Key] = new LocalizedString(text.Key, text.Value.Value.NormalizeLineEndings());
                    }
                }

                // 本地化语言去重
                if (!dictionaries.ContainsKey(textGroup.Key))
                {
                    dictionaries[textGroup.Key] = new StaticLocalizationDictionary(textGroup.Key, cultureTextDictionaires);
                }
            }

            return dictionaries;
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<Dictionary<string, Dictionary<string, ILocalizationDictionary>>> GetAllLocalizationDictionaryAsync(CancellationToken cancellationToken = default)
        {
            var result = new Dictionary<string, Dictionary<string, ILocalizationDictionary>>();

            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            var textList = LocalizationStoreCache.GetAllLocalizedStrings(CultureInfo.CurrentCulture.Name);

            foreach (var resourcesGroup in textList)
            {
                var dictionaries = new Dictionary<string, ILocalizationDictionary>();
                foreach (var text in resourcesGroup.Value)
                {
                    var cultureTextDictionaires = new Dictionary<string, LocalizedString>();
                    // 本地化名称去重
                    if (!cultureTextDictionaires.ContainsKey(text.Key))
                    {
                        cultureTextDictionaires[text.Key] = new LocalizedString(text.Key, text.Value.Value.NormalizeLineEndings());
                    }

                    // 本地化语言去重
                    if (!dictionaries.ContainsKey(text.Key))
                    {
                        dictionaries[text.Key] = new StaticLocalizationDictionary(text.Key, cultureTextDictionaires);
                    }
                }

                result.Add(resourcesGroup.Key, dictionaries);
            }

            return result;
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<bool> ResourceExistsAsync(string resourceName, CancellationToken cancellationToken = default)
        {
            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            return LocalizationStoreCache.GetResourceOrNull(resourceName) != null;
        }

        public LocalizationResourceBase GetResourceOrNull(string resourceName)
        {
            return AsyncHelper.RunSync(async () => await GetResourceOrNullAsync(resourceName));
        }

        public async virtual Task<LocalizationResourceBase> GetResourceOrNullAsync(string resourceName)
        {
            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            return LocalizationStoreCache.GetResourceOrNull(resourceName);
        }

        public async virtual Task<string[]> GetResourceNamesAsync()
        {
            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            return LocalizationStoreCache.GetResources()
                .Select(x => x.ResourceName)
                .ToArray();
        }

        public async virtual Task<LocalizationResourceBase[]> GetResourcesAsync()
        {
            var context = new LocalizationStoreCacheInitializeContext(ServiceProvider);
            await LocalizationStoreCache.InitializeAsync(context);

            return LocalizationStoreCache.GetResources().ToArray();
        }
    }
}
