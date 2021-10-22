using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Localization.Dynamic
{
    public class DynamicLocalizationInitializeService : IHostedService
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

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var resource in LocalizationOptions.Resources)
            {
                foreach (var contributor in resource.Value.Contributors)
                {
                    if (contributor.GetType().IsAssignableFrom(typeof(DynamicLocalizationResourceContributor)))
                    {
                        var resourceLocalizationDic = await Store.GetLocalizationDictionaryAsync(resource.Value.ResourceName);
                        DynamicOptions.AddOrUpdate(resource.Value.ResourceName, resourceLocalizationDic);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
