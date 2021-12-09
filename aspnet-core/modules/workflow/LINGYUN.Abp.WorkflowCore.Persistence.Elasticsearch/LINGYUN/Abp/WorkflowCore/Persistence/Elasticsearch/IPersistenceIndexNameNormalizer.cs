namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch
{
    public interface IPersistenceIndexNameNormalizer
    {
        string NormalizeIndex(string index);
    }
}
