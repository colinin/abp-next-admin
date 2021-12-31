using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Tencent
{
    public interface ITencentBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
