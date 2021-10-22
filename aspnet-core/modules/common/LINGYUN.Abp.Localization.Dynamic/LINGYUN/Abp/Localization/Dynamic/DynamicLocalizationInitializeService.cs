using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var resource in LocalizationOptions.Resources)
            {
                foreach (var contributor in resource.Value.Contributors)
                {
                    if (contributor.GetType().IsAssignableFrom(typeof(DynamicLocalizationResourceContributor)))
                    {
                        var resourceLocalizationDic = await Store
                            .GetLocalizationDictionaryAsync(
                                resource.Value.ResourceName,
                                stoppingToken);
                        DynamicOptions.AddOrUpdate(resource.Value.ResourceName, resourceLocalizationDic);
                    }
                }
            }
        }
    }
}
