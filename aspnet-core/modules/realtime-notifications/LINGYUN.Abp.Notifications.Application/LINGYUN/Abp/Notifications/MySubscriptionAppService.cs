using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications;

[Authorize]
public class MySubscriptionAppService : AbpNotificationsApplicationServiceBase, IMySubscriptionAppService
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

    public async virtual Task<PagedResultDto<UserSubscreNotificationDto>> GetListAsync(SubscriptionsGetByPagedDto input)
    {
        var userSubscribedCount = await UserSubscribeRepository.GetCountAsync(CurrentUser.GetId());

        var userSubscribes = await UserSubscribeRepository
            .GetUserSubscribesAsync(CurrentUser.GetId(), input.Sorting,
                    input.SkipCount, input.MaxResultCount);

        return new PagedResultDto<UserSubscreNotificationDto>(userSubscribedCount,
            userSubscribes.Select(us => new UserSubscreNotificationDto { Name = us.NotificationName }).ToList());
    }

    public async virtual Task<ListResultDto<UserSubscreNotificationDto>> GetAllListAsync()
    {
        var userSubscribes = await NotificationSubscriptionManager
            .GetUserSubscriptionsAsync(
                CurrentTenant.Id,
                CurrentUser.GetId());

        return new ListResultDto<UserSubscreNotificationDto>(
            userSubscribes.Select(msn => new UserSubscreNotificationDto { Name = msn.NotificationName }).ToList());
    }

    public async virtual Task<UserSubscriptionsResult> IsSubscribedAsync(SubscriptionsGetByNameDto input)
    {
        var isSubscribed = await NotificationSubscriptionManager
            .IsSubscribedAsync(CurrentTenant.Id, CurrentUser.GetId(), input.Name);

        return isSubscribed
            ? UserSubscriptionsResult.Subscribed()
            : UserSubscriptionsResult.UnSubscribed();
    }

    public async virtual Task SubscribeAsync(SubscriptionsGetByNameDto input)
    {
        await NotificationSubscriptionManager
            .SubscribeAsync(
                CurrentTenant.Id,
                new UserIdentifier(CurrentUser.GetId(), CurrentUser.UserName),
                input.Name);
    }

    public async virtual Task UnSubscribeAsync(SubscriptionsGetByNameDto input)
    {
        await NotificationSubscriptionManager
            .UnsubscribeAsync(
                CurrentTenant.Id,
                new UserIdentifier(CurrentUser.GetId(), CurrentUser.UserName),
                input.Name);
    }
}
