using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
public class SecurityLogManager : ISecurityLogManager, ITransientDependency
{
    protected IObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule> ObjectMapper { get; }
    protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public SecurityLogManager(
        IObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule> objectMapper,
        IIdentitySecurityLogRepository identitySecurityLogRepository,
        IUnitOfWorkManager unitOfWorkManager)
    {
        ObjectMapper = objectMapper;
        IdentitySecurityLogRepository = identitySecurityLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
    }

    public async virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(requiresNew: true))
        {
            await IdentitySecurityLogRepository.DeleteManyAsync(ids,
                cancellationToken: cancellationToken);
            await uow.CompleteAsync();
        }
    }

    public async virtual Task<SecurityLog> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var securityLog = await IdentitySecurityLogRepository.GetAsync(id, includeDetails, cancellationToken);

        return ObjectMapper.Map<IdentitySecurityLog, SecurityLog>(securityLog);
    }

    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            await IdentitySecurityLogRepository.DeleteAsync(id);
            await uow.CompleteAsync();
        }
    }

    public async virtual Task<List<SecurityLog>> GetListAsync(
        string sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string applicationName = null,
        string identity = null,
        string action = null,
        Guid? userId = null,
        string userName = null,
        string clientId = null,
        string clientIpAddress = null,
        string correlationId = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var securityLogs = await IdentitySecurityLogRepository.GetListAsync(
            sorting,
            maxResultCount,
            skipCount,
            startTime,
            endTime,
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            correlationId,
            clientIpAddress,
            includeDetails,
            cancellationToken);

        return ObjectMapper.Map<List<IdentitySecurityLog>, List<SecurityLog>>(securityLogs);
    }


    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string applicationName = null,
        string identity = null,
        string action = null,
        Guid? userId = null,
        string userName = null,
        string clientId = null,
        string clientIpAddress = null,
        string correlationId = null,
        CancellationToken cancellationToken = default)
    {
        return await IdentitySecurityLogRepository.GetCountAsync(
            startTime,
            endTime,
            applicationName,
            identity,
            action,
            userId,
            userName,
            clientId,
            correlationId,
            clientIpAddress,
            cancellationToken);
    }
}
