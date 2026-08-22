using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Elasticsearch;

public interface IIndexMappingProvider
{
    Task<IndexMappingInfo> GetMappingAsync(
        string indexPattern, 
        CancellationToken cancellationToken = default);

    Task<IndexMappingInfo> GetMappingAsync<TDocument>(
        string indexPattern,
        CancellationToken cancellationToken = default);
}
