using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Chat
{
    public class ChatDataSeeder : IChatDataSeeder, ITransientDependency
    {
        protected IClock Clock { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IUserChatCardRepository UserChatCardRepository { get; }
        protected IUserChatSettingRepository UserChatSettingRepository { get; }
        public ChatDataSeeder(
            IClock clock,
            ICurrentTenant currentTenant,
            IUserChatCardRepository userChatCardRepository,
            IUserChatSettingRepository userChatSettingRepository)
        {
            Clock = clock;
            CurrentTenant = currentTenant;
            UserChatCardRepository = userChatCardRepository;
            UserChatSettingRepository = userChatSettingRepository;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(IUserData user)
        {
            using (CurrentTenant.Change(user.TenantId))
            {
                var userHasOpendIm = await UserChatSettingRepository.UserHasOpendImAsync(user.Id);
                if (!userHasOpendIm)
                {
                    var userChatSetting = new UserChatSetting(user.Id, user.TenantId);

                    await UserChatSettingRepository.InsertAsync(userChatSetting);

                    var userChatCard = new UserChatCard(user.Id, user.UserName, IM.Sex.Male, user.UserName, tenantId: user.TenantId)
                    {
                        CreationTime = Clock.Now,
                        CreatorId = user.Id
                    };

                    await UserChatCardRepository.InsertAsync(userChatCard);
                }
            }
        }
    }
}
