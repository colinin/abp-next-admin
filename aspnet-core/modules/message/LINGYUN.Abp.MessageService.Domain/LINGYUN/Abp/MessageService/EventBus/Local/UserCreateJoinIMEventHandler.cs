using LINGYUN.Abp.MessageService.Messages;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.EventBus.Distributed
{
    public class UserCreateJoinIMEventHandler : ILocalEventHandler<EntityCreatedEventData<UserEto>>, ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUserChatSettingRepository _userChatSettingRepository;
        public UserCreateJoinIMEventHandler(
            ICurrentTenant currentTenant,
            IUnitOfWorkManager unitOfWorkManager,
            IUserChatSettingRepository userChatSettingRepository)
        {
            _currentTenant = currentTenant;
            _unitOfWorkManager = unitOfWorkManager;
            _userChatSettingRepository = userChatSettingRepository;
        }
        /// <summary>
        /// 接收添加用户事件,启用IM模块时增加用户配置
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public async Task HandleEventAsync(EntityCreatedEventData<UserEto> eventData)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                using (_currentTenant.Change(eventData.Entity.TenantId))
                {
                    var userHasOpendIm = await _userChatSettingRepository.UserHasOpendImAsync(eventData.Entity.Id);
                    if (!userHasOpendIm)
                    {
                        var userChatSetting = new UserChatSetting(eventData.Entity.Id, eventData.Entity.TenantId);
                        await _userChatSettingRepository.InsertAsync(userChatSetting);

                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
