using Qiniu.Util;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using QiniuConfig = Qiniu.Common.Config;

namespace LINGYUN.Abp.BlobStoring.Qiniu
{
    public class QiniuBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IQiniuBlobNameCalculator QiniuBlobNameCalculator { get; }
        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task SaveAsync(BlobProviderSaveArgs args)
        {
            throw new NotImplementedException();
        }

        private Mac GetMac(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetQiniuConfiguration();
            return new Mac(configuration.AccessKey, configuration.SecretKey);
        }

        private string GetAndInitBucketName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetQiniuConfiguration();
            var bucketName = configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.BucketName;
            QiniuConfig.AutoZone(configuration.AccessKey, bucketName, true);
            return bucketName;
        }
    }
}
