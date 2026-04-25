using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using IVoloAuditLogInfoToAuditLogConverter = Volo.Abp.AuditLogging.IAuditLogInfoToAuditLogConverter;
using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
public class EfCoreAuditLogWriter : IAuditLogWriter, ITransientDependency
{
    protected IVoloAuditLogInfoToAuditLogConverter AuditLogConverter { get; }
    protected IAuditLogRepository AuditLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public EfCoreAuditLogWriter(
        IVoloAuditLogInfoToAuditLogConverter auditLogConverter,
        IAuditLogRepository auditLogRepository,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
    {
        AuditLogConverter = auditLogConverter;
        AuditLogRepository = auditLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
    }

    public async virtual Task WriteAsync(AuditLogInfo auditLogInfo, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            var auditLog = await AuditLogConverter.ConvertAsync(auditLogInfo);

            await AuditLogRepository.InsertAsync(auditLog);

            await uow.CompleteAsync();
        }
    }

    public async virtual Task BulkWriteAsync(IEnumerable<AuditLogInfo> auditLogInfos, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            var auditLogs = new List<VoloAuditLog>();
            foreach (var auditLogInfo in auditLogInfos)
            {
                var auditLog = await AuditLogConverter.ConvertAsync(auditLogInfo);
                auditLogs.Add(auditLog);
            }

            await AuditLogRepository.InsertManyAsync(auditLogs);

            await uow.CompleteAsync();
        }
    }
}
