using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Users;

namespace LY.MicroService.RealtimeMessage.EventBus.Distributed
{
    public class UserCreateEventHandler : IDistributedEventHandler<EntityCreatedEto<UserEto>>, ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;
        public UserCreateEventHandler(
            ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }
        /// <summary>
        /// 接收添加用户事件,发布本地事件
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
        {
            var localUserCreateEventData = new EntityCreatedEventData<UserEto>(eventData.Entity);

            await _localEventBus.PublishAsync(localUserCreateEventData);
        }
    }
}
