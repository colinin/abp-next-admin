using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
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
        protected ILanguageRepository LanguageRepository { get; }
        protected ITextRepository TextRepository { get; }
        protected IResourceRepository ResourceRepository { get; }

        public LocalizationStore(
            ILanguageRepository languageRepository,
            ITextRepository textRepository,
            IResourceRepository resourceRepository)
        {
            TextRepository = textRepository;
            LanguageRepository = languageRepository;
            ResourceRepository = resourceRepository;
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<List<LanguageInfo>> GetLanguageListAsync(
            CancellationToken cancellationToken = default)
        {
            var languages = await LanguageRepository.GetActivedListAsync(cancellationToken);

            return languages
                .Select(x => new LanguageInfo(x.CultureName, x.UiCultureName, x.DisplayName, x.FlagIcon))
                .ToList();
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<Dictionary<string, ILocalizationDictionary>> GetLocalizationDictionaryAsync(
            string resourceName,
            CancellationToken cancellationToken = default)
        {
            // TODO: 引用缓存?
            var dictionaries = new Dictionary<string, ILocalizationDictionary>();
            var resource = await ResourceRepository.FindByNameAsync(resourceName, cancellationToken);
            if (resource == null || !resource.Enable)
            {
                // 资源不存在或未启用返回空
                return dictionaries;
            }

            var texts = await TextRepository.GetListAsync(resourceName, null, cancellationToken);

            foreach (var textGroup in texts.GroupBy(x => x.CultureName))
            {
                var cultureTextDictionaires = new Dictionary<string, LocalizedString>();
                foreach (var text in textGroup)
                {
                    // 本地化名称去重
                    if (!cultureTextDictionaires.ContainsKey(text.Key))
                    {
                        cultureTextDictionaires[text.Key] = new LocalizedString(text.Key, text.Value.NormalizeLineEndings());
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
            var textList = await TextRepository.GetListAsync(resourceName: null, cancellationToken: cancellationToken);

            foreach (var resourcesGroup in textList.GroupBy(x => x.ResourceName))
            {
                var dictionaries = new Dictionary<string, ILocalizationDictionary>();
                foreach (var textGroup in resourcesGroup.GroupBy(x => x.CultureName))
                {
                    var cultureTextDictionaires = new Dictionary<string, LocalizedString>();
                    foreach (var text in textGroup)
                    {
                        // 本地化名称去重
                        if (!cultureTextDictionaires.ContainsKey(text.Key))
                        {
                            cultureTextDictionaires[text.Key] = new LocalizedString(text.Key, text.Value.NormalizeLineEndings());
                        }
                    }

                    // 本地化语言去重
                    if (!dictionaries.ContainsKey(textGroup.Key))
                    {
                        dictionaries[textGroup.Key] = new StaticLocalizationDictionary(textGroup.Key, cultureTextDictionaires);
                    }
                }

                result.Add(resourcesGroup.Key, dictionaries);
            }

            return result;
        }

        [Obsolete("The framework already supports dynamic languages and will be deprecated in the next release")]
        public async virtual Task<bool> ResourceExistsAsync(string resourceName, CancellationToken cancellationToken = default)
        {
            return await ResourceRepository.ExistsAsync(resourceName, cancellationToken);
        }

        public LocalizationResourceBase GetResourceOrNull(string resourceName)
        {
            return GetResourceOrNullAsync(resourceName)
                .ConfigureAwait(continueOnCapturedContext: false)
                .GetAwaiter()
                .GetResult();
        }

        public async virtual Task<LocalizationResourceBase> GetResourceOrNullAsync(string resourceName)
        {
            var resource = await ResourceRepository.FindByNameAsync(resourceName);
            if (resource == null)
            {
                return null;
            }

            return new NonTypedLocalizationResource(resource.Name);
        }

        public async virtual Task<string[]> GetResourceNamesAsync()
        {
            var resources = await ResourceRepository.GetListAsync();

            return resources.Select(r => r.Name).ToArray();
        }

        public async virtual Task<LocalizationResourceBase[]> GetResourcesAsync()
        {
            var resources = await ResourceRepository.GetListAsync();

            return resources.Select(r => new NonTypedLocalizationResource(r.Name)).ToArray();
        }
    }
}
