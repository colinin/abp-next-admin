using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public interface IAliyunBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
