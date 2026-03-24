using LINGYUN.Abp.Notifications.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Notifications;

[Authorize(NotificationsPermissions.Notification.SendRecord.Default)]
public class NotificationSendRecordAppService : AbpNotificationsApplicationServiceBase, INotificationSendRecordAppService
{
    private readonly INotificationSendRecordRepository _repository;

    protected INotificationStore NotificationStore => LazyServiceProvider.LazyGetRequiredService<INotificationStore>();

    protected INotificationSender NotificationSender => LazyServiceProvider.LazyGetRequiredService<INotificationSender>();

    protected INotificationDefinitionManager NotificationDefinitionManager => LazyServiceProvider.LazyGetRequiredService<INotificationDefinitionManager>();

    public NotificationSendRecordAppService(INotificationSendRecordRepository repository)
    {
        _repository = repository;
    }

    [Authorize(NotificationsPermissions.Notification.SendRecord.Delete)]
    public async virtual Task DeleteAsync(long id)
    {
        var sendRecord = await _repository.GetAsync(id);

        await _repository.DeleteAsync(sendRecord);
    }

    [Authorize(NotificationsPermissions.Notification.SendRecord.ReSend)]
    public async virtual Task ReSendAsync(long id)
    {
        var sendRecord = await _repository.GetAsync(id);
        var notificationInfo = await NotificationStore.GetNotificationOrNullAsync(sendRecord.TenantId, sendRecord.NotificationId);
        var notificationDefine = await NotificationDefinitionManager.GetOrNullAsync(notificationInfo.Name);

        if (notificationDefine?.Template != null)
        {
            var template = new NotificationTemplate(
                notificationInfo.Name,
                data: notificationInfo.Data.ExtraProperties);

            await NotificationSender.SendNofiterAsync(
                notificationInfo.Name,
                template,
                [new UserIdentifier(sendRecord.UserId, sendRecord.UserName)],
                sendRecord.TenantId,
                notificationInfo.Severity,
                [sendRecord.Provider]);
        }
        else
        {
            await NotificationSender.SendNofiterAsync(
                notificationInfo.Name,
                notificationInfo.Data,
                [new UserIdentifier(sendRecord.UserId, sendRecord.UserName)],
                sendRecord.TenantId,
                notificationInfo.Severity,
                [sendRecord.Provider]);
        }
    }

    public async virtual Task<PagedResultDto<NotificationSendRecordDto>> GetListAsync(NotificationSendRecordGetPagedListInput input)
    {
        Expression<Func<NotificationSendRecordInfo, bool>> expression = _ => true;

        if (input.State.HasValue)
        {
            expression = expression.And(x => x.State == input.State);
        }
        if (!input.Provider.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Provider == input.Provider);
        }
        if (!input.NotificationName.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.NotificationInfo.Name == input.NotificationName);
        }
        if (input.UserId.HasValue)
        {
            expression = expression.And(x => x.UserId == input.UserId);
        }
        if (input.BeginSendTime.HasValue)
        {
            expression = expression.And(x => x.SendTime >= input.BeginSendTime);
        }
        if (input.EndSendTime.HasValue)
        {
            expression = expression.And(x => x.SendTime <= input.EndSendTime);
        }

        var specification = new ExpressionSpecification<NotificationSendRecordInfo>(expression);

        var totalCount = await _repository.GetCountAsync(specification);
        var list = await _repository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<NotificationSendRecordDto>(totalCount,
            ObjectMapper.Map<List<NotificationSendRecordInfo>, List<NotificationSendRecordDto>>(list));
    }
}
