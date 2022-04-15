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

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchEntityChangeStore : IEntityChangeStore, ITransientDependency
{
    private readonly AbpElasticsearchOptions _elasticsearchOptions;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;

    public ILogger<ElasticsearchEntityChangeStore> Logger { protected get; set; }

    public ElasticsearchEntityChangeStore(
        IIndexNameNormalizer indexNameNormalizer,
        IElasticsearchClientFactory clientFactory,
        IOptions<AbpElasticsearchOptions> elasticsearchOptions)
    {
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

        var resposne = await client.SearchAsync<EntityChange>(
            dsl => dsl.Index(CreateIndex())
                      .Query(query =>
                            query.Bool(bo => 
                                bo.Must(m => 
                                    m.Nested(n =>
                                        n.InnerHits()
                                         .Path("EntityChanges")
                                         .Query(nq =>
                                            nq.Term(nqt =>
                                                nqt.Field(GetField(nameof(EntityChange.Id))).Value(entityChangeId)))))))
                      .Source(x => x.Excludes(f => f.Field("*")))
                      .Sort(entity => entity.Field("EntityChanges.ChangeTime", SortOrder.Descending))
                      .Size(1),
            ct: cancellationToken);

        if (resposne.Shards.Successful > 0)
        {
            var hits = resposne.Hits.FirstOrDefault();
            if (hits.InnerHits.Count > 0)
            {
                return hits.InnerHits.First().Value.Documents<EntityChange>().FirstOrDefault();
            }
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
        await Task.CompletedTask;
        return 0;
        //var client = _clientFactory.Create();

        //var querys = BuildQueryDescriptor(
        //    auditLogId,
        //    startTime,
        //    endTime,
        //    changeType,
        //    entityId,
        //    entityTypeFullName);

        //Func<QueryContainerDescriptor<EntityChange>, QueryContainer> selector = q => q.MatchAll();
        //if (querys.Count > 0)
        //{
        //    selector = q => q.Bool(b => b.Must(querys.ToArray()));
        //}

        //var response = await client.CountAsync<EntityChange>(dsl =>
        //    dsl.Index(CreateIndex())
        //       .Query(q =>
        //            q.Bool(b =>
        //                b.Must(m =>
        //                    m.Nested(n =>
        //                        n.InnerHits(hit => hit.Source(s => s.ExcludeAll()))
        //                         .Path("EntityChanges")
        //                         .Query(selector)
        //                   )
        //                )
        //            )
        //       ),
        //    ct: cancellationToken);

        //return response.Count;
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
        // TODO: 需要解决Nested格式数据返回方式

        //var client = _clientFactory.Create();

        //var sortOrder = !sorting.IsNullOrWhiteSpace() && sorting.EndsWith("asc", StringComparison.InvariantCultureIgnoreCase)
        //    ? SortOrder.Ascending : SortOrder.Descending;
        //sorting = !sorting.IsNullOrWhiteSpace()
        //    ? sorting.Split()[0]
        //    : nameof(EntityChange.ChangeTime);

        //var querys = BuildQueryDescriptor(
        //    auditLogId,
        //    startTime,
        //    endTime,
        //    changeType,
        //    entityId,
        //    entityTypeFullName);

        //SourceFilterDescriptor<EntityChange> SourceFilter(SourceFilterDescriptor<EntityChange> selector)
        //{
        //    selector.Includes(GetEntityChangeSources());
        //    if (!includeDetails)
        //    {
        //        selector.Excludes(field =>
        //            field.Field("EntityChanges.PropertyChanges")
        //                 .Field("EntityChanges.ExtraProperties"));
        //    }

        //    return selector;
        //}

        //Func<QueryContainerDescriptor<EntityChange>, QueryContainer> selector = q => q.MatchAll();
        //if (querys.Count > 0)
        //{
        //    selector = q => q.Bool(b => b.Must(querys.ToArray()));
        //}

        //var response = await client.SearchAsync<EntityChange>(dsl =>
        //    dsl.Index(CreateIndex())
        //       .Query(q =>
        //            q.Bool(b =>
        //                b.Must(m =>
        //                    m.Nested(n =>
        //                        n.InnerHits(hit => hit.Source(SourceFilter))
        //                         .Path("EntityChanges")
        //                         .Query(selector)
        //                   )
        //                )
        //            )
        //       )
        //       .Source(x => x.Excludes(f => f.Field("*")))
        //       .Sort(entity => entity.Field(GetField(sorting), sortOrder))
        //       .From(skipCount)
        //       .Size(maxResultCount),
        //    cancellationToken);

        //if (response.Shards.Successful > 0)
        //{
        //    var hits = response.Hits.FirstOrDefault();
        //    if (hits.InnerHits.Count > 0)
        //    {
        //        return hits.InnerHits.First().Value.Documents<EntityChange>().ToList();
        //    }
        //}
        await Task.CompletedTask;
        return new List<EntityChange>();
    }

    public async virtual Task<EntityChangeWithUsername> GetWithUsernameAsync(
        Guid entityChangeId,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var response = await client.SearchAsync<AuditLog>(
            dsl => dsl.Index(CreateIndex())
                      .Query(query =>
                            query.Bool(bo =>
                                bo.Must(m =>
                                    m.Nested(n => 
                                        n.InnerHits()
                                         .Path("EntityChanges")
                                         .Query(nq => 
                                            nq.Bool(nb =>
                                                nb.Must(nm =>
                                                    nm.Term(nt =>
                                                        nt.Field(GetField(nameof(EntityChange.Id))).Value(entityChangeId)))))))))
                      .Source(selector => selector.Includes(field =>
                            field.Field(f => f.UserName)))
                      .Size(1),
            ct: cancellationToken);

        var auditLog = response.Documents.FirstOrDefault();
        EntityChange entityChange = null;

        if (response.Shards.Successful > 0)
        {
            var hits = response.Hits.FirstOrDefault();
            if (hits.InnerHits.Count > 0)
            {
                entityChange = hits.InnerHits.First().Value.Documents<EntityChange>().FirstOrDefault();
            }
        }

        return new EntityChangeWithUsername()
        {
            EntityChange = entityChange,
            UserName = auditLog?.UserName
        };
    }

    public async virtual Task<List<EntityChangeWithUsername>> GetWithUsernameAsync(
        string entityId,
        string entityTypeFullName, 
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var response = await client.SearchAsync<AuditLog>(
            dsl => dsl.Index(CreateIndex())
                      .Query(query =>
                            query.Bool(bo =>
                                bo.Must(m =>
                                    m.Nested(n =>
                                        n.InnerHits()
                                         .Path("EntityChanges")
                                         .Query(nq =>
                                            nq.Bool(nb =>
                                                nb.Must(nm =>
                                                    nm.Term(nt =>
                                                        nt.Field(GetField(nameof(EntityChange.EntityId))).Value(entityId)),
                                                        nm =>
                                                    nm.Term(nt =>
                                                        nt.Field(GetField(nameof(EntityChange.EntityTypeFullName))).Value(entityTypeFullName))
                                                    )
                                                )
                                            )
                                         )
                                    )
                                )
                            )
                      .Source(selector => selector.Includes(field => 
                            field.Field(f => f.UserName)))
                      .Sort(entity => entity.Field(f => f.ExecutionTime, SortOrder.Descending)),
            ct: cancellationToken);

        if (response.Hits.Count > 0)
        {
            return response.Hits.
                Select(hit => new EntityChangeWithUsername
                {
                    UserName = hit.Source.UserName,
                    EntityChange = hit.InnerHits.Any() ? 
                        hit.InnerHits.First().Value.Documents<EntityChange>().FirstOrDefault()
                        : null
                })
                .ToList();
        }

        return new List<EntityChangeWithUsername>();
    }

    protected virtual List<Func<QueryContainerDescriptor<EntityChange>, QueryContainer>> BuildQueryDescriptor(
        Guid? auditLogId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        EntityChangeType? changeType = null,
        string entityId = null,
        string entityTypeFullName = null)
    {
        var querys = new List<Func<QueryContainerDescriptor<EntityChange>, QueryContainer>>();

        if (auditLogId.HasValue)
        {
            querys.Add(entity => entity.Term(q => q.Field(GetField(nameof(EntityChange.AuditLogId))).Value(auditLogId)));
        }
        if (startTime.HasValue)
        {
            querys.Add(entity => entity.DateRange(q => q.Field(GetField(nameof(EntityChange.ChangeTime))).GreaterThanOrEquals(startTime)));
        }
        if (endTime.HasValue)
        {
            querys.Add(entity => entity.DateRange(q => q.Field(GetField(nameof(EntityChange.ChangeTime))).LessThanOrEquals(endTime)));
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
