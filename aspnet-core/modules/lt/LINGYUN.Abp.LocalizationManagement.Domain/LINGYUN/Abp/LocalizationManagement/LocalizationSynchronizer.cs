using LINGYUN.Abp.Localization.Dynamic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class LocalizationSynchronizer :
        ILocalEventHandler<EntityCreatedEventData<Text>>,
        ILocalEventHandler<EntityUpdatedEventData<Text>>,
        ILocalEventHandler<EntityDeletedEventData<Text>>,
        ITransientDependency
    {
        private readonly IDistributedEventBus _eventBus;

        public LocalizationSynchronizer(
            IDistributedEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public virtual async Task HandleEventAsync(EntityCreatedEventData<Text> eventData)
        {
            await HandleEventAsync(BuildResetEventData(eventData.Entity));
        }

        public virtual async Task HandleEventAsync(EntityUpdatedEventData<Text> eventData)
        {
            await HandleEventAsync(BuildResetEventData(eventData.Entity));
        }

        public virtual async Task HandleEventAsync(EntityDeletedEventData<Text> eventData)
        {
            var data = BuildResetEventData(eventData.Entity);
            data.IsDeleted = true;

            await HandleEventAsync(data);
        }

        private LocalizedStringCacheResetEventData BuildResetEventData(Text text)
        {
            return new LocalizedStringCacheResetEventData(
                text.ResourceName, text.CultureName, text.Key, text.Value);
        }

        private async Task HandleEventAsync(LocalizedStringCacheResetEventData eventData)
        {
            await _eventBus.PublishAsync(eventData);
        }
    }
}
