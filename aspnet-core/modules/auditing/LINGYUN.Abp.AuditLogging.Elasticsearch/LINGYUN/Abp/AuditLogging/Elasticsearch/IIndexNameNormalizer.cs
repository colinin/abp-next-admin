namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public interface IIndexNameNormalizer
    {
        string NormalizeIndex(string index);
    }
}
