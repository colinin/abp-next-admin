using COSXML;
using System.Threading.Tasks;

namespace LINGYUN.Abp.BlobStoring.Tencent;

public interface ICosClientFactory
{
    Task<CosXml> CreateAsync<TContainer>();

    Task<CosXml> CreateAsync(TencentBlobProviderConfiguration configuration);
}
