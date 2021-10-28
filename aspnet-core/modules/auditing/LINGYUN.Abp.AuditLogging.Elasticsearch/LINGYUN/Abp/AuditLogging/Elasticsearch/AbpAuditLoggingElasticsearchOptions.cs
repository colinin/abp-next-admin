namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public class AbpAuditLoggingElasticsearchOptions
    {
        public const string DefaultIndexPrefix = "auditlogging";
        public string IndexPrefix { get; set; }

        public AbpAuditLoggingElasticsearchOptions()
        {
            IndexPrefix = DefaultIndexPrefix;
        }
    }
}
