using Elastic.Clients.Elasticsearch.IndexManagement;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public class AbpAuditLoggingElasticsearchOptions
{
    public const string DefaultIndexPrefix = "auditlogging";
    public string IndexPrefix { get; set; }
    /// <summary>
    /// 索引初始化失败抛出异常
    /// </summary>
    /// <remarks>
    /// 默认为: true, 索引初始化失败后应用程序停止运行
    /// </remarks>
    public bool ThrowIfIndexInitFailed { get; set; }
    /// <summary>
    /// 审计日志索引设置
    /// </summary>
    public IndexSettings AuditLogSettings { get; set; }
    /// <summary>
    /// 安全日志索引设置
    /// </summary>
    public IndexSettings SecurityLogSettings { get; set; }

    public AbpAuditLoggingElasticsearchOptions()
    {
        IndexPrefix = DefaultIndexPrefix;
        ThrowIfIndexInitFailed = true;

        AuditLogSettings = new IndexSettings()
        {
            NumberOfReplicas = 1,
            NumberOfShards = 3,
            Mapping = new MappingLimitSettings
            {
                TotalFields = new MappingLimitSettingsTotalFields
                {
                    Limit = 1000,
                },
                NestedFields = new MappingLimitSettingsNestedFields
                {
                    Limit = 50,
                },
                Depth = new MappingLimitSettingsDepth
                {
                    Limit = 10,
                },
            }
        };

        SecurityLogSettings = new IndexSettings();
    }
}
