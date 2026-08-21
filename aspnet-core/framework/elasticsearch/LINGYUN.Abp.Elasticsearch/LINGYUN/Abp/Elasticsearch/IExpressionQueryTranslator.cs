using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elasticsearch;

public interface IExpressionQueryTranslator
{
    Task<Query> TranslateAsync<TDocument>(string indexName, Expression<Func<TDocument, bool>> expression);
}
