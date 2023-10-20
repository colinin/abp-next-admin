using LINGYUN.Abp.BackgroundTasks;
using System;
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
                PropertyTimeZone,
                LocalizableStatic.Create("Indices:TimeZone"),
                LocalizableStatic.Create("Indices:TimeZoneDesc")),
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
    /// 计算时差的时区, 默认Utc
    /// </summary>
    private const string PropertyTimeZone = "TimeZone";
    /// <summary>
    /// 过期时间, 单位秒, 默认 5184000 (60天)
    /// </summary>
    private const string PropertyExpirationTime = "ExpirationTime";

    #endregion

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        #region Initializes Job Parameters

        var timeZone = TimeZoneInfo.Utc;
        var indexPrefix = context.GetString(PropertyIndexPrefix);
        var timeZoneString = context.GetOrDefaultString(PropertyTimeZone, "utc");
        var expirationSecond = context.GetOrDefaultJobData(PropertyExpirationTime, 5184000L);

        if (!timeZoneString.IsNullOrWhiteSpace())
        {
            timeZone = timeZoneString.ToLowerInvariant() switch
            {
                "local" => TimeZoneInfo.Local,
                _ => TimeZoneInfo.Utc,
            };
        }

        var elasticClientFactory = context.GetRequiredService<IElasticsearchClientFactory>();
        var elasticClient = elasticClientFactory.Create();

        var clock = context.GetRequiredService<IClock>();
        var expirationTime = clock.Now.AddSeconds(-expirationSecond);
        var startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), timeZone);
        var removeIndices = new List<string>();

        #endregion

        #region ES indices.get_settings API

        // GET demo*/_settings
        var settingResponse = await elasticClient.Indices.GetSettingsAsync(indexPrefix);
        if (!settingResponse.IsValid)
        {
            throw new AbpJobExecutionException(GetType(), settingResponse.ServerError.ToString(), settingResponse.OriginalException);
        }

        foreach (var indexSet in settingResponse.Indices)
        {
            // 索引创建日期
            if (indexSet.Value.Settings.TryGetValue("index.creation_date", out var indexSetV) &&
                long.TryParse(indexSetV.ToString(), out var timestamp))
            {
                var indexCreationDate = startTime.AddMilliseconds(timestamp);
                if (indexCreationDate <= expirationTime)
                {
                    removeIndices.Add(indexSet.Key.Name);
                }
            }
        }

        #endregion

        #region ES indices.delete API

        foreach (var index in removeIndices)
        {
            var delResponse = await elasticClient.Indices.DeleteAsync(index);
            if (!delResponse.IsValid)
            {
                throw new AbpJobExecutionException(GetType(), delResponse.ServerError.ToString(), delResponse.OriginalException);
            }
        }

        #endregion
    }
}
