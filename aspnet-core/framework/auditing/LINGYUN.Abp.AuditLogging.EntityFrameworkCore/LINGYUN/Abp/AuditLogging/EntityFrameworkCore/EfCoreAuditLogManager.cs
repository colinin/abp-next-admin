using LINGYUN.Linq.Dynamic.Queryable;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Specifications;
using Volo.Abp.Uow;

using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Dependency(ReplaceServices = true)]
public class EfCoreAuditLogManager : IAuditLogManager, ITransientDependency
{
    private readonly static Dictionary<Type, Type> _defaultTypeMap = new Dictionary<Type, Type>
    {
        [typeof(AuditLog)] = typeof(VoloAuditLog),
        [typeof(AuditLogAction)] = typeof(Volo.Abp.AuditLogging.AuditLogAction),
        [typeof(EntityChange)] = typeof(Volo.Abp.AuditLogging.EntityChange),
        [typeof(EntityPropertyChange)] = typeof(Volo.Abp.AuditLogging.EntityPropertyChange),
    };

    protected IObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule> ObjectMapper { get; }
    protected IEfCoreAuditLogRepository AuditLogRepository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public EfCoreAuditLogManager(
        IUnitOfWorkManager unitOfWorkManager,
        IEfCoreAuditLogRepository auditLogRepository,
        IObjectMapper<AbpAuditLoggingEntityFrameworkCoreModule> objectMapper)
    {
        ObjectMapper = objectMapper;
        AuditLogRepository = auditLogRepository;
        UnitOfWorkManager = unitOfWorkManager;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<AuditLog> specification,
        CancellationToken cancellationToken = default)
    {
        var converter = new ExpressionQueryConverter<AuditLog, VoloAuditLog>(_defaultTypeMap);
        var resetSpec = new ExpressionSpecification<VoloAuditLog>(
            converter.Convert(specification.ToExpression()));

        return await AuditLogRepository.GetCountAsync(resetSpec, cancellationToken);
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        ISpecification<AuditLog> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var converter = new ExpressionQueryConverter<AuditLog, VoloAuditLog>(_defaultTypeMap);
        var resetSpec = new ExpressionSpecification<VoloAuditLog>(
            converter.Convert(specification.ToExpression()));

        var auditLogs = await AuditLogRepository.GetListAsync(
            resetSpec,
            sorting,
            maxResultCount,
            skipCount,
            includeDetails,
            cancellationToken);

        return ObjectMapper.Map<List<VoloAuditLog>, List<AuditLog>>(auditLogs);
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null,
        string? userName = null,
        string? applicationName = null,
        string? correlationId = null,
        string? clientId = null,
        string? clientIpAddress = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        CancellationToken cancellationToken = default)
    {
        return await AuditLogRepository.GetCountAsync(
            startTime,
            endTime,
            httpMethod,
            url,
            clientId,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode,
            cancellationToken);
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? httpMethod = null,
        string? url = null,
        Guid? userId = null,
        string? userName = null,
        string? applicationName = null,
        string? correlationId = null,
        string? clientId = null,
        string? clientIpAddress = null,
        int? maxExecutionDuration = null,
        int? minExecutionDuration = null,
        bool? hasException = null,
        HttpStatusCode? httpStatusCode = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var auditLogs = await AuditLogRepository.GetListAsync(
            sorting,
            maxResultCount,
            skipCount,
            startTime,
            endTime,
            httpMethod,
            url,
            clientId,
            userId,
            userName,
            applicationName,
            clientIpAddress,
            correlationId,
            maxExecutionDuration,
            minExecutionDuration,
            hasException,
            httpStatusCode,
            includeDetails,
            cancellationToken);

        return ObjectMapper.Map<List<VoloAuditLog>, List<AuditLog>>(auditLogs);
    }

    public async virtual Task<AuditLog?> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var auditLog = await AuditLogRepository.GetAsync(id, includeDetails, cancellationToken);

        return ObjectMapper.Map<VoloAuditLog, AuditLog>(auditLog);
    }

    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            await AuditLogRepository.DeleteAsync(id);
            await uow.CompleteAsync();
        }
    }

    public async virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        using (var uow = UnitOfWorkManager.Begin(true))
        {
            await AuditLogRepository.DeleteManyAsync(ids);
            await uow.CompleteAsync();
        }
    }
}
