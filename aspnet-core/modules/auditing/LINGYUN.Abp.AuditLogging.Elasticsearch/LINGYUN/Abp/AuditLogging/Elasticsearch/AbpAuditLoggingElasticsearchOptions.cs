using Nest;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public class AbpAuditLoggingElasticsearchOptions
    {
        public const string DefaultIndexPrefix = "auditlogging";
        public string IndexPrefix { get; set; }
        public IIndexSettings IndexSettings { get; set; }

        public AbpAuditLoggingElasticsearchOptions()
        {
            IndexPrefix = DefaultIndexPrefix;
            IndexSettings = new IndexSettings();
        }
    }
}
