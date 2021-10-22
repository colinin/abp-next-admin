using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    internal class LocalizationResetSynchronizer :
        IDistributedEventHandler<LocalizedStringCacheResetEventData>,
        ITransientDependency
    {
        private readonly AbpLocalizationDynamicOptions _options;

        public LocalizationResetSynchronizer(
            IOptions<AbpLocalizationDynamicOptions> options)
        {
            _options = options.Value;
        }
        public virtual Task HandleEventAsync(LocalizedStringCacheResetEventData eventData)
        {
            var dictionaries = GetDictionaries(eventData.ResourceName);
            if (!dictionaries.ContainsKey(eventData.CultureName))
            {
                // TODO: 需要处理 data.Key data.Value 空引用
                var dictionary = new Dictionary<string, LocalizedString>();
                dictionary.Add(eventData.Key, new LocalizedString(eventData.Key, eventData.Value.NormalizeLineEndings()));
                var newLocalizationDictionary = new StaticLocalizationDictionary(eventData.CultureName, dictionary);

                dictionaries.Add(eventData.CultureName, newLocalizationDictionary);
            }
            else
            {
                // 取出当前的缓存写入到新字典进行处理
                var nowLocalizationDictionary = dictionaries[eventData.CultureName];
                var dictionary = new Dictionary<string, LocalizedString>();
                nowLocalizationDictionary.Fill(dictionary);

                var existsKey = dictionary.ContainsKey(eventData.Key);
                if (!existsKey)
                {
                    // 如果不存在,则新增
                    dictionary.Add(eventData.Key, new LocalizedString(eventData.Key, eventData.Value.NormalizeLineEndings()));
                }
                else if (existsKey && eventData.IsDeleted)
                {
                    // 如果删掉了本地化的节点,删掉当前的缓存
                    dictionary.Remove(eventData.Key);
                }

                var newLocalizationDictionary = new StaticLocalizationDictionary(eventData.CultureName, dictionary);

                if (newLocalizationDictionary != null)
                {
                    // 重新赋值变更过的缓存
                    dictionaries[eventData.CultureName] = newLocalizationDictionary;
                }
            }

            return Task.CompletedTask;
        }

        protected virtual Dictionary<string, ILocalizationDictionary> GetDictionaries(string resourceName)
        {
            return _options.LocalizationDictionary
                .GetOrAdd(resourceName, () => new Dictionary<string, ILocalizationDictionary>());
        }
    }
}
