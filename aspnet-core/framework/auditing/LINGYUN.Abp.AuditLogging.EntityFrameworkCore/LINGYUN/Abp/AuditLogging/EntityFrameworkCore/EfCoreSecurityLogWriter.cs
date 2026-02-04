using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
public class EfCoreSecurityLogWriter : ISecurityLogWriter, ITransientDependency
{
    protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public EfCoreSecurityLogWriter(
        IIdentitySecurityLogRepository identitySecurityLogRepository, 
        IUnitOfWorkManager unitOfWorkManager, 
        IGuidGenerator guidGenerator)
    {
        IdentitySecurityLogRepository = identitySecurityLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
    }

    public async virtual Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            var securityLogs = securityLogInfos.Select(securityLogInfo => 
                new IdentitySecurityLog(GuidGenerator, securityLogInfo));

            await IdentitySecurityLogRepository.InsertManyAsync(
                securityLogs,
                false,
                cancellationToken);

            await uow.CompleteAsync();
        }
    }

    public async virtual Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentitySecurityLogRepository.InsertAsync(
                new IdentitySecurityLog(GuidGenerator, securityLogInfo),
                false,
                cancellationToken);
            await uow.CompleteAsync();
        }
    }
}
