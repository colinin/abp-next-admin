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
        CancellationToken cancellationToken = default) where TDocument : class
    {
        var client = ClientFactory.Create();

        var query = client.Esql.CreateQuery<TDocument>().Where(expression);
        if (!sorting.IsNullOrWhiteSpace())
        {
            query = query.OrderBy(sorting);
        }

        return await query
            .PageBy(skipCount, maxResultCount)
            .AsEsqlQueryable()
            .ToListAsync(cancellationToken);
    }
}
