using Elastic.Clients.Elasticsearch;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchEntityChangeStore : IEntityChangeStore, ITransientDependency
{
    private readonly IClock _clock;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IExpressionQueryService _expressionQueryService;

    public ILogger<ElasticsearchEntityChangeStore> Logger { protected get; set; }

    public ElasticsearchEntityChangeStore(
        IClock clock,
        IIndexNameNormalizer indexNameNormalizer,
        IElasticsearchClientFactory clientFactory,
        IExpressionQueryService expressionQueryService)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _expressionQueryService = expressionQueryService;

        Logger = NullLogger<ElasticsearchEntityChangeStore>.Instance;
    }

    public async virtual Task<EntityChange?> GetAsync(
        Guid entityChangeId, 
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = x => x.EntityChanges.Any(e => e.Id == entityChangeId);

        var auditLogs = await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: $"{nameof(AuditLog.ExecutionTime)} DESC",
            cancellationToken: cancellationToken);

        var auditLog = auditLogs.FirstOrDefault();
        if (auditLog != null)
        {
            return auditLog
                .EntityChanges
                .Select(e => e)
                .FirstOrDefault();
        }

        return null;
    }

    public async virtual Task<long> GetCountAsync(
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null, 
        EntityChangeType? changeType = null, 
        string? entityId = null, 
        string? entityTypeFullName = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(auditLogId.HasValue, x => x.Id == auditLogId)
            .AndIf(startTime.HasValue, x => x.ExecutionTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.ExecutionTime <= _clock.Normalize(endTime!.Value))
            .AndIf(changeType.HasValue, x => x.EntityChanges.Any(x => x.ChangeType == changeType))
            .AndIf(!entityId.IsNullOrWhiteSpace(), x => x.EntityChanges.Any(x => x.EntityId == entityId))
            .AndIf(!entityTypeFullName.IsNullOrWhiteSpace(), x => x.EntityChanges.Any(x => x.EntityTypeFullName == entityTypeFullName));

        var auditLogs = await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: $"{nameof(AuditLog.ExecutionTime)} DESC",
            cancellationToken: cancellationToken);

        return auditLogs.Sum(log => log.EntityChanges.Count);
    }

    public async virtual Task<List<EntityChange>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Guid? auditLogId = null, 
        DateTime? startTime = null, 
        DateTime? endTime = null, 
        EntityChangeType? changeType = null, 
        string? entityId = null,
        string? entityTypeFullName = null, 
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var result = new List<EntityChange>();
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(auditLogId.HasValue, x => x.Id == auditLogId)
            .AndIf(startTime.HasValue, x => x.ExecutionTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.ExecutionTime <= _clock.Normalize(endTime!.Value))
            .AndIf(changeType.HasValue, x => x.EntityChanges.Any(x => x.ChangeType == changeType))
            .AndIf(!entityId.IsNullOrWhiteSpace(), x => x.EntityChanges.Any(x => x.EntityId == entityId))
            .AndIf(!entityTypeFullName.IsNullOrWhiteSpace(), x => x.EntityChanges.Any(x => x.EntityTypeFullName == entityTypeFullName));

        var auditLogs = await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: sorting,
            cancellationToken: cancellationToken);
        if (auditLogs.Count > 0)
        {
            var groupAuditLogs = auditLogs.GroupBy(log => log.UserName);
            foreach (var group in groupAuditLogs)
            {
                var entityChangesList = group.Select(log => log.EntityChanges);

                foreach (var entityChanges in entityChangesList)
                {
                    foreach (var entityChange in entityChanges)
                    {
                        result.Add(entityChange);
                    }
                }
            }
        }

        // TODO: 临时在内存中分页
        return result
            .AsQueryable()
            .PageBy(skipCount, maxResultCount)
            .ToList();
    }

    public async virtual Task<EntityChangeWithUsername?> GetWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = x => x.EntityChanges.Any(e => e.Id == entityChangeId);

        var auditLogs = await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: $"{nameof(AuditLog.ExecutionTime)} DESC",
            cancellationToken: cancellationToken);

        var auditLog = auditLogs.FirstOrDefault();
        if (auditLog != null)
        {
            return auditLog.EntityChanges.Select(e => 
                new EntityChangeWithUsername
                {
                    UserName = auditLog.UserName,
                    EntityChange = e
                })
                .FirstOrDefault();
        }

        return null;
    }

    public async virtual Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(
        string entityId,
        string entityTypeFullName, 
        CancellationToken cancellationToken = default)
    {
        var result = new List<EntityChangeWithUsername>();
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = x => x.EntityChanges.Any(e => e.EntityId == entityId && e.EntityTypeFullName == entityTypeFullName);

        var auditLogs = await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: $"{nameof(AuditLog.ExecutionTime)} DESC",
            cancellationToken: cancellationToken);

        if (auditLogs.Count > 0)
        {
            var groupAuditLogs = auditLogs.GroupBy(log => log.UserName);
            foreach (var group in groupAuditLogs)
            {
                var entityChangesList = group.Select(log => log.EntityChanges);

                foreach (var entityChanges in entityChangesList)
                {
                    foreach (var entityChange in entityChanges.Where(e => string.Equals(e.EntityId, entityId) && string.Equals(e.EntityTypeFullName, entityTypeFullName)))
                    {
                        result.Add(
                            new EntityChangeWithUsername
                            {
                                UserName = group.Key,
                                EntityChange = entityChange
                            });
                    }
                }
            }
        }

        return result;
    }

    protected virtual string CreateIndexPattern()
    {
        return _indexNameNormalizer.NormalizeIndexPattern("audit-log");
    }
}
