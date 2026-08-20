using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elasticsearch;

public interface IExpressionQueryService
{
    /// <summary>
    /// 查询符合条件的文档数量
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="indexName">文档索引</param>
    /// <param name="expression">查询表达式树</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync<TDocument>(
        string indexName, 
        Expression<Func<TDocument, bool>> expression,
        CancellationToken cancellationToken = default) where TDocument : class;
    /// <summary>
    /// 查询符合条件的文档列表
    /// </summary>
    /// <typeparam name="TDocument">文档类型</typeparam>
    /// <param name="indexName">文档索引</param>
    /// <param name="expression">查询表达式树</param>
    /// <param name="sorting">排序字段</param>
    /// <param name="maxResultCount">最大返回数据大小</param>
    /// <param name="skipCount">跳过数据大小</param>
    /// <param name="sourceExcludes">包含字段</param>
    /// <param name="sourceIncludes">忽略字段</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TDocument>> GetListAsync<TDocument>(
        string indexName,
        Expression<Func<TDocument, bool>> expression,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        Fields? sourceExcludes = null,
        Fields? sourceIncludes = null,
        CancellationToken cancellationToken = default) where TDocument : class;
}
