using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class DynamicLocalizationInitializeService : BackgroundService
    {
        protected ILocalizationStore Store { get; }
        protected AbpLocalizationOptions LocalizationOptions { get; }
        protected AbpLocalizationDynamicOptions DynamicOptions { get; }

        public DynamicLocalizationInitializeService(
            ILocalizationStore store,
            IOptions<AbpLocalizationOptions> localizationOptions,
            IOptions<AbpLocalizationDynamicOptions> dynamicOptions)
        {
            Store = store;
            DynamicOptions = dynamicOptions.Value;
            LocalizationOptions = localizationOptions.Value;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var resourceTexts = await Store.GetAllLocalizationDictionaryAsync(stoppingToken);

                foreach (var resource in LocalizationOptions.Resources)
                {
                    foreach (var contributor in resource.Value.Contributors)
                    {
                        if (contributor.GetType().IsAssignableFrom(typeof(DynamicLocalizationResourceContributor)))
                        {
                            if (resourceTexts.TryGetValue(resource.Value.ResourceName, out var resourceLocalizationDic))
                            {
                                DynamicOptions.AddOrUpdate(resource.Value.ResourceName, resourceLocalizationDic);
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException) { } // 忽略此异常
        }
    }
}
