namespace LINGYUN.Abp.AuditLogging.Elasticsearch;

public interface IIndexNameNormalizer
{
    string NormalizeIndex(string index);

    string NormalizeIndexPattern(string index);

    string NormalizeIndexPrefix(string index);
}
