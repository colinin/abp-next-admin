using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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

        var sortOrder = SortOrder.Desc;

        var querys = BuildQueryDescriptor(entityChangeId: entityChangeId);

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
               .From(0)
               .Size(1)
               .Query(new BoolQuery
               {
                   Must = querys
               });

            dsl.Sort(s => s.Field(new FieldSort(GetField(nameof(EntityChange.ChangeTime)))
            {
                Order = sortOrder
            }));

            dsl.SourceIncludes(ix => ix.EntityChanges);

            dsl.SourceExcludes(
                ex => ex.Actions,
                ex => ex.Comments,
                ex => ex.Exceptions);
        }, cancellationToken);

        var auditLog = searchResponse.Documents.FirstOrDefault();
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

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               })
               .SourceIncludes(
                    ix => ix.UserName, 
                    ix => ix.EntityChanges)
               .SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions)
               .From(0)
               .Size(1000);
        }, cancellationToken);

        var auditLogs = searchResponse.Documents.ToList();

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
                ? SortOrder.Asc : SortOrder.Desc;
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

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               })
               .SourceIncludes(
                    ix => ix.UserName,
                    ix => ix.EntityChanges)
               .From(0)
               .Size(1000);
            if (includeDetails)
            {
                dsl.SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions);
            }
        }, cancellationToken);

        var auditLogs = searchResponse.Documents.ToList();
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

        var sortOrder = SortOrder.Desc;

        var querys = BuildQueryDescriptor(entityChangeId: entityChangeId);

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               })
               .SourceExcludes(
                    ix => ix.UserName,
                    ix => ix.EntityChanges)
               .Sort(s => s.Field(new FieldSort(GetField(nameof(EntityChange.ChangeTime)))
               {
                   Order = sortOrder
               }))
               .From(0)
               .Size(1);
        }, cancellationToken);

        var auditLog = searchResponse.Documents.FirstOrDefault();
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

        var sortOrder = SortOrder.Desc;

        var querys = BuildQueryDescriptor(entityId: entityId, entityTypeFullName: entityTypeFullName);

        var searchResponse = await client.SearchAsync<AuditLog>(dsl =>
        {
            dsl.Indices(CreateIndex())
               .Query(new BoolQuery
               {
                   Must = querys
               })
               .SourceExcludes(
                    ix => ix.Actions,
                    ix => ix.Comments,
                    ix => ix.Exceptions)
               .Sort(s => s.Field(new FieldSort(GetField(nameof(EntityChange.ChangeTime)))
               {
                   Order = sortOrder
               }))
               .From(0)
               .Size(1);
        }, cancellationToken);

        var auditLogs = searchResponse.Documents.ToList();
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

    protected virtual List<Query> BuildQueryDescriptor(
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null,
        Guid? entityChangeId = null)
    {
        var queries = new List<Query>();

        if (auditLogId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(EntityChange.AuditLogId)), auditLogId.Value.ToString()));
        }
        if (startTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(EntityChange.ChangeTime)))
            {
                Gte = _clock.Normalize(startTime.Value)
            });
        }
        if (endTime.HasValue)
        {
            queries.Add(new DateRangeQuery(GetField(nameof(EntityChange.ChangeTime)))
            {
                Lte = _clock.Normalize(endTime.Value)
            });
        }
        if (changeType.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(EntityChange.ChangeType)), ((int)changeType.Value).ToString()));
        }
        if (!entityId.IsNullOrWhiteSpace())
        {
            queries.Add(new TermQuery(GetField(nameof(EntityChange.EntityId)), entityId));
        }
        if (!entityTypeFullName.IsNullOrWhiteSpace())
        {
            queries.Add(new WildcardQuery(GetField(nameof(EntityChange.EntityTypeFullName)))
            {
                Value = $"*{entityTypeFullName}*"
            });
        }
        if (entityChangeId.HasValue)
        {
            queries.Add(new TermQuery(GetField(nameof(EntityChange.Id)), entityChangeId.Value.ToString()));
        }

        return queries;
    }

    protected virtual string CreateIndex()
    {
        return _indexNameNormalizer.NormalizeIndex("audit-log");
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
