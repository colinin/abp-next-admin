using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchEntityChangeStore : IEntityChangeStore, ITransientDependency
{
    private readonly AbpElasticsearchOptions _elasticsearchOptions;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IClock _clock;

    public ILogger<ElasticsearchEntityChangeStore> Logger { protected get; set; }

    public ElasticsearchEntityChangeStore(
        IClock clock,
        IIndexNameNormalizer indexNameNormalizer,
        IElasticsearchClientFactory clientFactory,
        IOptions<AbpElasticsearchOptions> elasticsearchOptions)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _elasticsearchOptions = elasticsearchOptions.Value;

        Logger = NullLogger<ElasticsearchEntityChangeStore>.Instance;
    }

    public async virtual Task<EntityChange> GetAsync(
        Guid entityChangeId, 
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var sortOrder = SortOrder.Descending;

        var querys = BuildQueryDescriptor(entityChangeId: entityChangeId);

        static SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
        {
            selector.IncludeAll()
                    .Excludes(field =>
                        field.Field(f => f.Actions)
                             .Field(f => f.Comments)
                             .Field(f => f.Exceptions));

            return selector;
        }

        var response = await client.SearchAsync<AuditLog>(dsl =>
            dsl.Index(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray())))
               .Source(SourceFilter)
               .Sort(log => log.Field(GetField(nameof(EntityChange.ChangeTime)), sortOrder))
               .From(0)
               .Size(1),
            cancellationToken);

        var auditLog = response.Documents.FirstOrDefault();
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
        string entityId = null, 
        string entityTypeFullName = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var querys = BuildQueryDescriptor(
            auditLogId,
            startTime,
            endTime,
            changeType,
            entityId,
            entityTypeFullName);

        SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
        {
            return selector
                .Includes(field =>
                    field.Field(f => f.UserName)
                         .Field(f => f.EntityChanges))
                .Excludes(field =>
                    field.Field(f => f.Actions)
                         .Field(f => f.Comments)
                         .Field(f => f.Exceptions));
        }

        var response = await client.SearchAsync<AuditLog>(dsl =>
            dsl.Index(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray())))
               .Source(SourceFilter)
               .From(0)
               .Size(1000),
            cancellationToken);

        var auditLogs = response.Documents.ToList();

        return auditLogs.Sum(log => log.EntityChanges.Count);
    }

    public async virtual Task<List<EntityChange>> GetListAsync(
        string sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Guid? auditLogId = null, 
        DateTime? startTime = null, 
        DateTime? endTime = null, 
        EntityChangeType? changeType = null, 
        string entityId = null,
        string entityTypeFullName = null, 
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        // TODO: 正确的索引可以避免性能损耗

        var result = new List<EntityChange>();
        var client = _clientFactory.Create();

        var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Ascending : SortOrder.Descending;
        sorting = !sorting.IsNullOrWhiteSpace()
                ? sorting.Split()[0]
                : nameof(EntityChange.ChangeTime);

        var querys = BuildQueryDescriptor(
            auditLogId,
            startTime,
            endTime,
            changeType,
            entityId,
            entityTypeFullName);

        SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
        {
            selector
                .Includes(field =>
                    field.Field(f => f.UserName)
                         .Field(f => f.EntityChanges));
            if (includeDetails)
            {
                selector.Includes(field =>
                    field.Field(f => f.Actions)
                         .Field(f => f.Comments)
                         .Field(f => f.Exceptions));
            }

            return selector;
        }

        var response = await client.SearchAsync<AuditLog>(dsl =>
            dsl.Index(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray())))
               .Source(SourceFilter)
               .Sort(log => log.Field(GetField(nameof(EntityChange.ChangeTime)), sortOrder))
               .From(0)
               .Size(1000), 
            cancellationToken);

        var auditLogs = response.Documents.ToList();
        if (auditLogs.Any())
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

    public async virtual Task<EntityChangeWithUsername> GetWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var sortOrder = SortOrder.Descending;

        var querys = BuildQueryDescriptor(entityChangeId: entityChangeId);

        static SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
        {
            selector.IncludeAll()
                    .Excludes(field =>
                        field.Field(f => f.Actions)
                             .Field(f => f.Comments)
                             .Field(f => f.Exceptions));

            return selector;
        }

        var response = await client.SearchAsync<AuditLog>(dsl =>
            dsl.Index(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray())))
               .Source(SourceFilter)
               .Sort(log => log.Field(GetField(nameof(EntityChange.ChangeTime)), sortOrder))
               .From(0)
               .Size(1),
            cancellationToken);

        var auditLog = response.Documents.FirstOrDefault();
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

        var sortOrder = SortOrder.Descending;

        var querys = BuildQueryDescriptor(entityId: entityId, entityTypeFullName: entityTypeFullName);

        static SourceFilterDescriptor<AuditLog> SourceFilter(SourceFilterDescriptor<AuditLog> selector)
        {
            selector.IncludeAll()
                    .Excludes(field =>
                        field.Field(f => f.Actions)
                             .Field(f => f.Comments)
                             .Field(f => f.Exceptions));

            return selector;
        }

        var response = await client.SearchAsync<AuditLog>(dsl =>
            dsl.Index(CreateIndex())
               .Query(log => log.Bool(b => b.Must(querys.ToArray())))
               .Source(SourceFilter)
               .Sort(log => log.Field(GetField(nameof(EntityChange.ChangeTime)), sortOrder))
               .From(0)
               .Size(100),
            cancellationToken);

        var auditLogs = response.Documents.ToList();
        if (auditLogs.Any())
        {
            var groupAuditLogs = auditLogs.GroupBy(log => log.UserName);
            foreach (var group in groupAuditLogs)
            {
                var entityChangesList = group.Select(log => log.EntityChanges);

                foreach (var entityChanges in entityChangesList)
                {
                    foreach (var entityChange in entityChanges.Where(e => e.EntityId.Equals(entityId) && e.EntityTypeFullName.Equals(entityTypeFullName)))
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

    protected virtual List<Func<QueryContainerDescriptor<AuditLog>, QueryContainer>> BuildQueryDescriptor(
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null,
        Guid? entityChangeId = null)
    {
        var querys = new List<Func<QueryContainerDescriptor<AuditLog>, QueryContainer>>();

        if (auditLogId.HasValue)
        {
            querys.Add(entity => entity.Term(q => q.Field(GetField(nameof(EntityChange.AuditLogId))).Value(auditLogId)));
        }
        if (startTime.HasValue)
        {
            querys.Add(entity => entity.DateRange(q => q.Field(GetField(nameof(EntityChange.ChangeTime))).GreaterThanOrEquals(_clock.Normalize(startTime.Value))));
        }
        if (endTime.HasValue)
        {
            querys.Add(entity => entity.DateRange(q => q.Field(GetField(nameof(EntityChange.ChangeTime))).LessThanOrEquals(_clock.Normalize(endTime.Value))));
        }
        if (changeType.HasValue)
        {
            querys.Add(entity => entity.Term(q => q.Field(GetField(nameof(EntityChange.ChangeType))).Value(changeType)));
        }
        if (!entityId.IsNullOrWhiteSpace())
        {
            querys.Add(entity => entity.Term(q => q.Field(GetField(nameof(EntityChange.EntityId))).Value(entityId)));
        }
        if (!entityTypeFullName.IsNullOrWhiteSpace())
        {
            querys.Add(entity => entity.Wildcard(q => q.Field(GetField(nameof(EntityChange.EntityTypeFullName))).Value($"*{entityTypeFullName}*")));
        }
        if (entityChangeId.HasValue)
        {
            querys.Add(entity => entity.Term(q => q.Field(GetField(nameof(EntityChange.Id))).Value(entityChangeId)));
        }

        return querys;
    }

    protected virtual string CreateIndex()
    {
        return _indexNameNormalizer.NormalizeIndex("audit-log");
    }

    protected Func<FieldsDescriptor<EntityChange>, IPromise<Fields>> GetEntityChangeSources()
    {
        return field => field
            .Field("EntityChanges.Id")
            .Field("EntityChanges.AuditLogId")
            .Field("EntityChanges.TenantId")
            .Field("EntityChanges.ChangeTime")
            .Field("EntityChanges.ChangeType")
            .Field("EntityChanges.EntityTenantId")
            .Field("EntityChanges.EntityId")
            .Field("EntityChanges.EntityTypeFullName")
            .Field("EntityChanges.PropertyChanges")
            .Field("EntityChanges.ExtraProperties");
    }

    private readonly static IDictionary<string, string> _fieldMaps = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Id", "EntityChanges.Id.keyword" },
            { "AuditLogId", "EntityChanges.AuditLogId.keyword" },
            { "TenantId", "EntityChanges.TenantId.keyword" },
            { "EntityTenantId", "EntityChanges.EntityTenantId.keyword" },
            { "EntityId", "EntityChanges.EntityId.keyword" },
            { "EntityTypeFullName", "EntityChanges.EntityTypeFullName.keyword" },
            { "PropertyChanges", "EntityChanges.PropertyChanges" },
            { "ExtraProperties", "EntityChanges.ExtraProperties" },
            { "ChangeType", "EntityChanges.ChangeType" },
            { "ChangeTime", "EntityChanges.ChangeTime" },
        };
    protected virtual string GetField(string field)
    {
        if (_fieldMaps.TryGetValue(field, out var mapField))
        {
            return _elasticsearchOptions.FieldCamelCase ? mapField.ToCamelCase() : mapField.ToPascalCase();
        }

        return _elasticsearchOptions.FieldCamelCase ? field.ToCamelCase() : field.ToPascalCase();
    }
}
