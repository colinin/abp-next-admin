using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchAuditLogManager : IAuditLogManager, ITransientDependency
{
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IIndexMappingProvider _indexMappingProvider;
    private readonly IExpressionQueryService _expressionQueryService;
    private readonly IClock _clock;

    public ILogger<ElasticsearchAuditLogManager> Logger { protected get; set; }

    public ElasticsearchAuditLogManager(
        IClock clock,
        IElasticsearchClientFactory clientFactory,
        IIndexNameNormalizer indexNameNormalizer,
        IIndexMappingProvider indexMappingProvider,
        IExpressionQueryService expressionQueryService)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _indexMappingProvider = indexMappingProvider;
        _expressionQueryService = expressionQueryService;

        Logger = NullLogger<ElasticsearchAuditLogManager>.Instance;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<AuditLog> specification,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndexPattern();

        return await _expressionQueryService.GetCountAsync(
            indexName,
            specification.ToExpression(),
            cancellationToken);
    }

    public async virtual Task<List<AuditLog>> GetListAsync(
        ISpecification<AuditLog> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var indexName = CreateIndexPattern();

        var sortingField = sorting;
        if (sortingField.IsNullOrWhiteSpace())
        {
            var indexMapping = await _indexMappingProvider.GetMappingAsync<AuditLog>(indexName, cancellationToken);
            if (indexMapping != null)
            {
                var sortingFieldMap = indexMapping.Fields
                    .Where(x => x.Key.Equals(sortingField, StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Value)
                    .FirstOrDefault();
                if (sortingFieldMap != null)
                {
                    sortingField = sortingFieldMap.Path;
                }
            }
        }

        return await _expressionQueryService.GetListAsync(
            indexName,
            specification.ToExpression(),
            sortingField,
            maxResultCount,
            skipCount,
            sourceExcludes: includeDetails == false
                ? Fields.FromFields(
                [
                    new Field(nameof(AuditLog.Actions)),
                    new Field(nameof(AuditLog.Comments)),
                    new Field(nameof(AuditLog.EntityChanges)),
                    new Field(nameof(AuditLog.Exceptions)),
                ])
                : null,
            cancellationToken: cancellationToken);
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
        var client = _clientFactory.Create();

        Expression<Func<AuditLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.ExecutionTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.ExecutionTime <= _clock.Normalize(endTime!.Value))
            .AndIf(!httpMethod.IsNullOrWhiteSpace(), x => x.HttpMethod == httpMethod)
            .AndIf(!url.IsNullOrWhiteSpace(), x => x.Url!.Contains(url!))
            .AndIf(userId.HasValue, x => x.UserId == userId)
            .AndIf(!userName.IsNullOrWhiteSpace(), x => x.UserName == userName)
            .AndIf(!applicationName.IsNullOrWhiteSpace(), x => x.ApplicationName == applicationName)
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.CorrelationId == correlationId)
            .AndIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .AndIf(!clientIpAddress.IsNullOrWhiteSpace(), x => x.ClientIpAddress == clientIpAddress)
            .AndIf(maxExecutionDuration.HasValue, x => x.ExecutionDuration >= maxExecutionDuration)
            .AndIf(minExecutionDuration.HasValue, x => x.ExecutionDuration <= minExecutionDuration)
            .AndIf(hasException == true, x => x.Exceptions != null)
            .AndIf(hasException == false, x => x.Exceptions == null)
            .AndIf(httpStatusCode.HasValue, x => x.HttpStatusCode == (int)httpStatusCode!);

        return await _expressionQueryService.GetCountAsync(
            CreateIndexPattern(),
            expression,
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
        var client = _clientFactory.Create();
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(AuditLog.ExecutionTime)} DESC";
        }

        Expression<Func<AuditLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.ExecutionTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.ExecutionTime <= _clock.Normalize(endTime!.Value))
            .AndIf(!httpMethod.IsNullOrWhiteSpace(), x => x.HttpMethod == httpMethod)
            .AndIf(!url.IsNullOrWhiteSpace(), x => x.Url!.Contains(url!))
            .AndIf(userId.HasValue, x => x.UserId == userId)
            .AndIf(!userName.IsNullOrWhiteSpace(), x => x.UserName == userName)
            .AndIf(!applicationName.IsNullOrWhiteSpace(), x => x.ApplicationName == applicationName)
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.CorrelationId == correlationId)
            .AndIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .AndIf(!clientIpAddress.IsNullOrWhiteSpace(), x => x.ClientIpAddress == clientIpAddress)
            .AndIf(maxExecutionDuration.HasValue, x => x.ExecutionDuration >= maxExecutionDuration)
            .AndIf(minExecutionDuration.HasValue, x => x.ExecutionDuration <= minExecutionDuration)
            .AndIf(hasException == true, x => x.Exceptions != null)
            .AndIf(hasException == false, x => x.Exceptions == null)
            .AndIf(httpStatusCode.HasValue, x => x.HttpStatusCode == (int)httpStatusCode!);

        return await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: sorting,
            maxResultCount: maxResultCount,
            skipCount: skipCount,
            sourceExcludes: includeDetails == false
                ? Fields.FromFields(
                [
                    new Field(nameof(AuditLog.Actions)),
                    new Field(nameof(AuditLog.Comments)),
                    new Field(nameof(AuditLog.EntityChanges)),
                    new Field(nameof(AuditLog.Exceptions)),
                ])
                : null,
            cancellationToken: cancellationToken);
    }

    public async virtual Task<AuditLog?> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var response = await client.SearchAsync<AuditLog>(s =>
        {
            s.Indices(CreateIndexPattern());
            s.Query(q => q.Ids(ids => ids.Values(id.ToString())));
            s.Size(1);

            if (!includeDetails)
            {
                s.SourceExcludes(
                    ex => ex.Actions,
                    ex => ex.Comments,
                    ex => ex.Exceptions,
                    ex => ex.EntityChanges);
            }
        }, cancellationToken);

        return response.Documents.FirstOrDefault();
    }

    public async virtual Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await DeleteManyAsync([id], cancellationToken);
    }

    public async virtual Task DeleteManyAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var idValues = ids.Select(id => id.ToString()).ToArray();

        await client.DeleteByQueryAsync<AuditLog>(d =>
        {
            d.Indices(CreateIndexPattern());
            d.Query(q => q.Ids(idsQuery => idsQuery.Values(idValues)));
        }, cancellationToken);
    }

    protected virtual string CreateIndexPattern()
    {
        return _indexNameNormalizer.NormalizeIndexPattern("audit-log");
    }
}
