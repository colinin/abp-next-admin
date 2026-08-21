using Elastic.Clients.Elasticsearch;
using Elastic.Esql.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Elasticsearch.EsqlQuery;

/// <summary>
/// NOTE: The tests for methods like Contains in the collection have failed. Please do not use this interface for collection filtering.
/// TODO: According to the official documentation, is it feasible? Is the support for nested types not available?
/// See: https://www.elastic.co/docs/reference/elasticsearch/clients/dotnet/linq-to-esql#linq-esql-filtering
/// var brands = new[] { "TechCorp", "StyleMax", "HomeBase" };
/// client.Esql.Query<Product>(q => q.Where(p => brands.Contains(p.Brand)));
/// → WHERE brand IN ("TechCorp", "StyleMax", "HomeBase")
/// </summary>

[Dependency(ReplaceServices = true)]
public class EsqlExpressionQueryService : ExpressionQueryService, ITransientDependency
{
    public EsqlExpressionQueryService(
        IElasticsearchClientFactory clientFactory, 
        IExpressionQueryTranslator expressionQueryTranslator) 
        : base(clientFactory, expressionQueryTranslator)
    {
    }

    public async override Task<long> GetCountAsync<TDocument>(
        string indexName, 
        Expression<Func<TDocument, bool>> expression, 
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var client = ClientFactory.Create();

        return await client.Esql.CreateQuery<TDocument>()
            .From(indexName)
            .Where(expression)
            .AsEsqlQueryable()
            .CountAsync(cancellationToken);
    }

    public async override Task<List<TDocument>> GetListAsync<TDocument>(
        string indexName,
        Expression<Func<TDocument, bool>> expression,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Fields? sourceExcludes = null,
        Fields? sourceIncludes = null,
        object[]? beginMarker = null,
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var client = ClientFactory.Create();

        var query = client.Esql.CreateQuery<TDocument>()
            .From(indexName)
            .Where(expression);
        if (!sorting.IsNullOrWhiteSpace())
        {
            query = query.OrderBy(sorting);
        }
        if (sourceExcludes != null)
        {
            query = query.Drop(sourceExcludes.Select(f => f.Name!).ToArray());
        }
        if (sourceIncludes != null)
        {
            query = query.Keep(sourceIncludes.Select(f => f.Name!).ToArray());
        }

        // TODO: 需要构建范围过滤条件,加入到query中以实现分页查询
        // See: https://www.elastic.co/docs/reference/elasticsearch/clients/dotnet/linq-to-esql#linq-esql-sorting
        return await query
            .Take(maxResultCount)
            .AsEsqlQueryable()
            .ToListAsync(cancellationToken);
    }
}
