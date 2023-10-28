using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Notifications;

[Authorize]
public class MyNotificationAppService : AbpNotificationsApplicationServiceBase, IMyNotificationAppService
{
    protected INotificationSender NotificationSender { get; }

    protected INotificationStore NotificationStore { get; }

    protected IUserNotificationRepository UserNotificationRepository { get; }

    protected INotificationDefinitionManager NotificationDefinitionManager { get; }

    public MyNotificationAppService(
        INotificationStore notificationStore,
        INotificationSender notificationSender,
        IUserNotificationRepository userNotificationRepository,
        INotificationDefinitionManager notificationDefinitionManager)
    {
        NotificationStore = notificationStore;
        NotificationSender = notificationSender;
        UserNotificationRepository = userNotificationRepository;
        NotificationDefinitionManager = notificationDefinitionManager;
    }

    public async virtual Task MarkReadStateAsync(NotificationMarkReadStateInput input)
    {
        await NotificationStore.ChangeUserNotificationsReadStateAsync(
            CurrentTenant.Id,
            CurrentUser.GetId(),
            input.IdList,
            input.State);
    }

    public async virtual Task DeleteAsync(long id)
    {
        await NotificationStore
            .DeleteUserNotificationAsync(
                CurrentTenant.Id,
                CurrentUser.GetId(),
                id);
    }

    public async virtual Task<UserNotificationDto> GetAsync(long id)
    {
        var notification = await UserNotificationRepository.GetByIdAsync(CurrentUser.GetId(), id);

        return ObjectMapper.Map<UserNotificationInfo, UserNotificationDto>(notification);
    }

    public async virtual Task<PagedResultDto<UserNotificationDto>> GetListAsync(UserNotificationGetByPagedDto input)
    {
        var totalCount = await UserNotificationRepository
            .GetCountAsync(
                CurrentUser.GetId(),
                input.Filter,
                input.ReadState);

        var notifications = await UserNotificationRepository
            .GetListAsync(
                CurrentUser.GetId(),
                input.Filter, input.Sorting,
                input.ReadState, input.SkipCount, input.MaxResultCount);

        return new PagedResultDto<UserNotificationDto>(totalCount,
            ObjectMapper.Map<List<UserNotificationInfo>, List<UserNotificationDto>>(notifications));
    }
}
