using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class DynamicLocalizationResourceContributor : ILocalizationResourceContributor
    {
        private ILogger _logger;
        private ILocalizationSubscriber _subscriber;

        private ILocalizationStore _store;
        protected ILocalizationStore Store => _store;

        private Dictionary<string, ILocalizationDictionary> _dictionaries;

        private readonly string _resourceName;
        private readonly AsyncLock _asyncLock = new AsyncLock();

        public DynamicLocalizationResourceContributor(string resourceName)
        {
            _resourceName = resourceName;
        }

        public virtual void Initialize(LocalizationResourceInitializationContext context)
        {
            _logger = context.ServiceProvider.GetService<ILogger<DynamicLocalizationResourceContributor>>();

            _store = context.ServiceProvider.GetRequiredService<ILocalizationStore>();
            _subscriber = context.ServiceProvider.GetRequiredService<ILocalizationSubscriber>();
            _subscriber.Subscribe(OnChanged);
        }

        public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
        {
            GetDictionaries().GetOrDefault(cultureName)?.Fill(dictionary);
        }

        public virtual LocalizedString GetOrNull(string cultureName, string name)
        {
            return GetDictionaries().GetOrDefault(cultureName)?.GetOrNull(name);
        }

        protected virtual Dictionary<string, ILocalizationDictionary> GetDictionaries()
        {
            var dictionaries = _dictionaries;
            if (dictionaries != null)
            {
                return dictionaries;
            }

            try
            {
                using (_asyncLock.Lock())
                {
                    dictionaries = _dictionaries = AsyncHelper.RunSync(async () =>
                        await Store.GetLocalizationDictionaryAsync(_resourceName));
                }
            }
            catch(Exception ex)
            {
                // 错误不应该影响到应用程序,而是记录到日志
                _logger?.LogWarning("Failed to get localized text, error: ", ex.Message);
            }

            return dictionaries;
        }

        private Task OnChanged(LocalizedStringCacheResetEventData data)
        {
            if (string.Equals(_resourceName, data.ResourceName))
            {
                if (!_dictionaries.ContainsKey(data.CultureName))
                {
                    // TODO: 需要处理 data.Key data.Value 空引用
                    var dictionary = new Dictionary<string, LocalizedString>();
                    dictionary.Add(data.Key, new LocalizedString(data.Key, data.Value.NormalizeLineEndings()));
                    var newLocalizationDictionary = new StaticLocalizationDictionary(data.CultureName, dictionary);

                    _dictionaries.Add(data.CultureName, newLocalizationDictionary);
                }
                else
                {
                    // 取出当前的缓存写入到新字典进行处理
                    var nowLocalizationDictionary = _dictionaries[data.CultureName];
                    var dictionary = new Dictionary<string, LocalizedString>();
                    nowLocalizationDictionary.Fill(dictionary);

                    var existsKey = dictionary.ContainsKey(data.Key);
                    if (!existsKey)
                    {
                        // 如果不存在,则新增
                        dictionary.Add(data.Key, new LocalizedString(data.Key, data.Value.NormalizeLineEndings()));
                    }
                    else if (existsKey && data.IsDeleted)
                    {
                        // 如果删掉了本地化的节点,删掉当前的缓存
                        dictionary.Remove(data.Key);
                    }

                    var newLocalizationDictionary = new StaticLocalizationDictionary(data.CultureName, dictionary);

                    if (newLocalizationDictionary != null)
                    {
                        // 重新赋值变更过的缓存
                        _dictionaries[data.CultureName] = newLocalizationDictionary;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
