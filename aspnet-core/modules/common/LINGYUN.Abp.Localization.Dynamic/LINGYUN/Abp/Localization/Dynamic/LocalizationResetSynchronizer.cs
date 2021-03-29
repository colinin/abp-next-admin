using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.Localization.Dynamic
{
    internal class LocalizationResetSynchronizer :
        IDistributedEventHandler<LocalizedStringCacheResetEventData>,
        ITransientDependency
    {
        private readonly ILocalizationDispatcher _dispatcher;

        public LocalizationResetSynchronizer(
            ILocalizationDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        public async Task HandleEventAsync(LocalizedStringCacheResetEventData eventData)
        {
            await _dispatcher.DispatchAsync(eventData);
        }
    }
}
