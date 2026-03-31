using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Notifications.EntityFrameworkCore;
public class EfCoreNotificationSendRecordRepository : 
    EfCoreRepository<INotificationsDbContext, NotificationSendRecord, long>, 
    INotificationSendRecordRepository
{
    public EfCoreNotificationSendRecordRepository(
        IDbContextProvider<INotificationsDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }

    public async virtual Task<int> GetCountAsync(
        ISpecification<NotificationSendRecordInfo> specification,
        CancellationToken cancellationToken = default)
    {
        return await (await GetSendRecordInfoAsync())
            .Where(specification.ToExpression())
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public async virtual Task<List<NotificationSendRecordInfo>> GetListAsync(
        ISpecification<NotificationSendRecordInfo> specification,
        string sorting = $"{nameof(NotificationSendRecordInfo.SendTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        return await (await GetSendRecordInfoAsync())
            .Where(specification.ToExpression())
            .OrderBy(!sorting.IsNullOrWhiteSpace() ? sorting : $"{nameof(NotificationSendRecordInfo.SendTime)} DESC")
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected async virtual Task<IQueryable<NotificationSendRecordInfo>> GetSendRecordInfoAsync()
    {
        var dbContext = await GetDbContextAsync();

        return dbContext.Set<Notification>()
            .Join(
                dbContext.Set<UserNotification>(),
                n => n.NotificationId,
                un => un.NotificationId,
                (n, un) => new
                {
                    NotificationId = n.NotificationId,
                    NotificationName = n.NotificationName,
                    NotificationTypeName = n.NotificationTypeName,
                    TenantId = n.TenantId,
                    Type = n.Type,
                    Severity = n.Severity,
                    ContentType = n.ContentType,
                    CreationTime = n.CreationTime,
                    ExtraProperties = n.ExtraProperties,
                    State = un.ReadStatus,
                    UserId = un.UserId,
                    Id = un.Id,
                })
            .Join(
                dbContext.Set<NotificationSendRecord>(),
                un => new { un.UserId, un.NotificationId },
                nsr => new { nsr.UserId, nsr.NotificationId },
                (un, nsr) => new NotificationSendRecordInfo
                {
                    Id = nsr.Id,
                    UserId = nsr.UserId,
                    UserName = nsr.UserName,
                    Provider = nsr.Provider,
                    State = nsr.State,
                    Reason = nsr.Reason,
                    SendTime = nsr.SendTime,
                    NotificationInfo = new UserNotificationInfo
                    {
                        TenantId = un.TenantId,
                        Type = un.Type,
                        Severity = un.Severity,
                        ContentType = un.ContentType,
                        CreationTime = un.CreationTime,
                        ExtraProperties = un.ExtraProperties,
                        NotificationId = un.NotificationId,
                        NotificationTypeName = un.NotificationTypeName,
                        Name = un.NotificationName,
                        State = un.State,
                        Id = un.Id,
                    },
                });
    }
}
