using Elastic.Clients.Elasticsearch.IndexManagement;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public class AbpAuditLoggingElasticsearchOptions
{
    public const string DefaultIndexPrefix = "auditlogging";
    public string IndexPrefix { get; set; }
    /// <summary>
    /// 是否启用审计日志记录
    /// </summary>
    public bool IsAuditLogEnabled { get; set; }
    /// <summary>
    /// 审计日志索引设置
    /// </summary>
    public IndexSettings AuditLogSettings { get; set; }
    /// <summary>
    /// 是否启用安全日志记录
    /// </summary>
    public bool IsSecurityLogEnabled { get; set; }
    /// <summary>
    /// 安全日志索引设置
    /// </summary>
    public IndexSettings SecurityLogSettings { get; set; }

    public AbpAuditLoggingElasticsearchOptions()
    {
        IndexPrefix = DefaultIndexPrefix;
        IsAuditLogEnabled = true;
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
        IsSecurityLogEnabled = true;
        SecurityLogSettings = new IndexSettings();
    }
}
