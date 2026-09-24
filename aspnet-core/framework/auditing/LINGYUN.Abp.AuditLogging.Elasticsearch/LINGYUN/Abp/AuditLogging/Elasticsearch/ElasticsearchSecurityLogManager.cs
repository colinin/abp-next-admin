using LINGYUN.Abp.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class ElasticsearchSecurityLogManager : ISecurityLogManager, ITransientDependency
{
    private readonly IClock _clock;
    private readonly IIndexNameNormalizer _indexNameNormalizer;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IExpressionQueryService _expressionQueryService;

    public ILogger<ElasticsearchSecurityLogManager> Logger { protected get; set; }

    public ElasticsearchSecurityLogManager(
        IClock clock,
        IIndexNameNormalizer indexNameNormalizer,
        IElasticsearchClientFactory clientFactory,
        IExpressionQueryService expressionQueryService)
    {
        _clock = clock;
        _clientFactory = clientFactory;
        _indexNameNormalizer = indexNameNormalizer;
        _expressionQueryService = expressionQueryService;

        Logger = NullLogger<ElasticsearchSecurityLogManager>.Instance;
    }

    public async virtual Task<SecurityLog?> GetAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        var response = await client.SearchAsync<SecurityLog>(s =>
        {
            s.Indices(CreateIndexPattern());
            s.Query(q => q.Ids(ids => ids.Values(id.ToString())));
            s.Size(1);
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

        await client.DeleteByQueryAsync<SecurityLog>(d =>
        {
            d.Indices(CreateIndexPattern());
            d.Query(q => q.Ids(idsQuery => idsQuery.Values(idValues)));
        }, cancellationToken);
    }

    public async virtual Task<List<SecurityLog>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? clientIpAddress = null,
        string? correlationId = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{nameof(SecurityLog.CreationTime)} DESC";
        }

        Expression<Func<SecurityLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.CreationTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.CreationTime <= _clock.Normalize(endTime!.Value))
            .AndIf(!applicationName.IsNullOrWhiteSpace(), x => x.ApplicationName == applicationName)
            .AndIf(!identity.IsNullOrWhiteSpace(), x => x.Identity == identity)
            .AndIf(!action.IsNullOrWhiteSpace(), x => x.Action == action)
            .AndIf(userId.HasValue, x => x.UserId == userId)
            .AndIf(!userName.IsNullOrWhiteSpace(), x => x.UserName == userName)
            .AndIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .AndIf(!clientIpAddress.IsNullOrWhiteSpace(), x => x.ClientIpAddress == clientIpAddress)
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.CorrelationId == correlationId);

        return await _expressionQueryService.GetListAsync(
            CreateIndexPattern(),
            expression,
            sorting: sorting,
            maxResultCount: maxResultCount,
            skipCount: skipCount,
            cancellationToken: cancellationToken);
    }


    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? applicationName = null,
        string? identity = null,
        string? action = null,
        Guid? userId = null,
        string? userName = null,
        string? clientId = null,
        string? clientIpAddress = null,
        string? correlationId = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();

        Expression<Func<SecurityLog, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.CreationTime >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.CreationTime <= _clock.Normalize(endTime!.Value))
            .AndIf(!applicationName.IsNullOrWhiteSpace(), x => x.ApplicationName == applicationName)
            .AndIf(!identity.IsNullOrWhiteSpace(), x => x.Identity == identity)
            .AndIf(!action.IsNullOrWhiteSpace(), x => x.Action == action)
            .AndIf(userId.HasValue, x => x.UserId == userId)
            .AndIf(!userName.IsNullOrWhiteSpace(), x => x.UserName == userName)
            .AndIf(!clientId.IsNullOrWhiteSpace(), x => x.ClientId == clientId)
            .AndIf(!clientIpAddress.IsNullOrWhiteSpace(), x => x.ClientIpAddress == clientIpAddress)
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.CorrelationId == correlationId);

        return await _expressionQueryService.GetCountAsync(
            CreateIndexPattern(),
            expression,
            cancellationToken);
    }

    protected virtual string CreateIndexPattern()
    {
        return _indexNameNormalizer.NormalizeIndexPattern("security-log");
    }
}
