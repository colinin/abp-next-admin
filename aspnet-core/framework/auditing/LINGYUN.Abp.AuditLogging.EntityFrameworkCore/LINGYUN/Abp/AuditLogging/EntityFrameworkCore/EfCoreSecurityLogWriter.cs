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
    protected IGuidGenerator GuidGenerator { get; }

    public EfCoreSecurityLogWriter(
        IIdentitySecurityLogRepository identitySecurityLogRepository, 
        IGuidGenerator guidGenerator)
    {
        IdentitySecurityLogRepository = identitySecurityLogRepository;
        GuidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async virtual Task BulkWriteAsync(IEnumerable<SecurityLogInfo> securityLogInfos, CancellationToken cancellationToken = default)
    {
        var securityLogs = securityLogInfos.Select(securityLogInfo =>
                new IdentitySecurityLog(GuidGenerator, securityLogInfo));

        await IdentitySecurityLogRepository.InsertManyAsync(
            securityLogs,
            false,
            cancellationToken);
    }

    [UnitOfWork]
    public async virtual Task WriteAsync(SecurityLogInfo securityLogInfo, CancellationToken cancellationToken = default)
    {
        await IdentitySecurityLogRepository.InsertAsync(
            new IdentitySecurityLog(GuidGenerator, securityLogInfo),
            false,
            cancellationToken);
    }
}
