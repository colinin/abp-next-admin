using LINGYUN.Abp.MessageService.Chat;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    public class UserCreateJoinIMEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly IChatDataSeeder _chatDataSeeder;
        public UserCreateJoinIMEventHandler(
            IChatDataSeeder chatDataSeeder)
        {
            _chatDataSeeder = chatDataSeeder;
        }
        /// <summary>
        /// 接收添加用户事件,初始化IM用户种子
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            await _chatDataSeeder.SeedAsync(eventData.Entity);
        }
    }
}
