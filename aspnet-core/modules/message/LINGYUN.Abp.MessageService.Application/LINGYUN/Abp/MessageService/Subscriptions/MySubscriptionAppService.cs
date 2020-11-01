using LINGYUN.Abp.Notifications;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    [Authorize]
    public class MySubscriptionAppService : ApplicationService, IMySubscriptionAppService
    {
        protected IUserSubscribeRepository UserSubscribeRepository { get; }
        protected INotificationSubscriptionManager NotificationSubscriptionManager { get; }

        public MySubscriptionAppService(
            IUserSubscribeRepository userSubscribeRepository,
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            UserSubscribeRepository = userSubscribeRepository;
            NotificationSubscriptionManager = notificationSubscriptionManager;
        }

        public virtual async Task<PagedResultDto<UserSubscreNotificationDto>> GetListAsync(SubscriptionsGetByPagedDto input)
        {
            var userSubscribedCount = await UserSubscribeRepository.GetCountAsync(CurrentUser.GetId());

            var userSubscribes = await UserSubscribeRepository
                .GetUserSubscribesAsync(CurrentUser.GetId(), input.Sorting,
                        input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<UserSubscreNotificationDto>(userSubscribedCount,
                userSubscribes.Select(us => new UserSubscreNotificationDto { Name = us.NotificationName }).ToList());
        }

        public virtual async Task<ListResultDto<UserSubscreNotificationDto>> GetAllListAsync()
        {
            var userSubscribes = await NotificationSubscriptionManager
                .GetUserSubscriptionsAsync(
                    CurrentTenant.Id,
                    CurrentUser.GetId());

            return new ListResultDto<UserSubscreNotificationDto>(
                userSubscribes.Select(msn => new UserSubscreNotificationDto { Name = msn.NotificationName }).ToList());
        }

        public virtual async Task<UserSubscriptionsResult> IsSubscribedAsync(SubscriptionsGetByNameDto input)
        {
            var isSubscribed = await NotificationSubscriptionManager
                .IsSubscribedAsync(CurrentTenant.Id, CurrentUser.GetId(), input.Name);

            return isSubscribed 
                ? UserSubscriptionsResult.Subscribed()
                : UserSubscriptionsResult.UnSubscribed();
        }

        public virtual async Task SubscribeAsync(SubscriptionsGetByNameDto input)
        {
            await NotificationSubscriptionManager
                .SubscribeAsync(
                    CurrentTenant.Id, 
                    new UserIdentifier(CurrentUser.GetId(), CurrentUser.UserName),
                    input.Name);
        }

        public virtual async Task UnSubscribeAsync(SubscriptionsGetByNameDto input)
        {
            await NotificationSubscriptionManager
                .UnsubscribeAsync(
                    CurrentTenant.Id,
                    new UserIdentifier(CurrentUser.GetId(), CurrentUser.UserName),
                    input.Name);
        }
    }
}
