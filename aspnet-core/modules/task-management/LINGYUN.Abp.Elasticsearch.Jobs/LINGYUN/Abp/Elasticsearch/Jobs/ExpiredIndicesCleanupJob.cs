using Elastic.Transport.Products.Elasticsearch;
using LINGYUN.Abp.BackgroundTasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Elasticsearch.Jobs;

/// <summary>
/// Elasticsearch 过期索引清理作业
/// </summary>
public class ExpiredIndicesCleanupJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(
                PropertyIndexPrefix,
                LocalizableStatic.Create("Indices:IndexPrefix"),
                required: true),
            new JobDefinitionParamter(
                PropertyExpirationTime,
                LocalizableStatic.Create("Indices:ExpirationTime")),
        };

    public const string Name = "ExpiredIndicesCleanupJob";
    /// <summary>
    /// 每次清除记录大小
    /// </summary>
    private const string PropertyIndexPrefix = "IndexPrefix";
    /// <summary>
    /// 过期时间, 单位秒, 默认 5184000 (60天)
    /// </summary>
    private const string PropertyExpirationTime = "ExpirationTime";

    #endregion

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        #region Initializes Job Parameters

        var indexPrefix = context.GetString(PropertyIndexPrefix);
        var expirationSecond = context.GetOrDefaultJobData(PropertyExpirationTime, 5184000L);

        var elasticClientFactory = context.GetRequiredService<IElasticsearchClientFactory>();
        var elasticClient = elasticClientFactory.Create();

        var clock = context.GetRequiredService<IClock>();
        var expirationTime = clock.Now.AddSeconds(-expirationSecond);
        var removeIndices = new List<string>();

        #endregion

        #region ES indices.get_settings API

        // GET demo*/_settings
        var indexResponse = await elasticClient.Indices.GetAsync(indexPrefix);
        if (!indexResponse.IsValidResponse)
        {
            indexResponse.TryGetOriginalException(out var originalException);
            indexResponse.TryGetElasticsearchServerError(out var elasticsearchServerError);
            throw new AbpJobExecutionException(GetType(), elasticsearchServerError?.ToString(), originalException);
        }

        foreach (var index in indexResponse.Indices)
        {
            // 索引创建日期
            if (index.Value.Settings?.CreationDate <= expirationTime ||
                index.Value.Settings?.Index?.CreationDate <= expirationTime)
            {
                removeIndices.Add(index.Key);
            }
        }

        #endregion

        #region ES indices.delete API

        foreach (var index in removeIndices)
        {
            var delResponse = await elasticClient.Indices.DeleteAsync(index);
            if (!delResponse.IsValidResponse)
            {
                delResponse.TryGetOriginalException(out var originalException);
                delResponse.TryGetElasticsearchServerError(out var elasticsearchServerError);
                throw new AbpJobExecutionException(GetType(), elasticsearchServerError?.ToString(), originalException);
            }
        }

        #endregion
    }
}
