using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    public interface IQiniuBlobNameCalculator
    {
        string Calculate(BlobProviderArgs args);
    }
}
