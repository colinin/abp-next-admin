using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Notifications;
public interface INotificationSendRecordRepository : IBasicRepository<NotificationSendRecord, long>
{
    Task<int> GetCountAsync(
        ISpecification<NotificationSendRecordInfo> specification,
        CancellationToken cancellationToken = default);

    Task<List<NotificationSendRecordInfo>> GetListAsync(
        ISpecification<NotificationSendRecordInfo> specification,
        string sorting = $"{nameof(NotificationSendRecordInfo.SendTime)} DESC",
        int maxResultCount = 10,
        int skipCount = 0,
        CancellationToken cancellationToken = default);
}
