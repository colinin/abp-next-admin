using LINGYUN.Abp.OssManagement;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.BlobStoring.OssManagement
{
    public class OssManagementBlobProvider : BlobProviderBase, ITransientDependency
    {
        public ILogger<OssManagementBlobProvider> Logger { protected get; set; }

        private readonly IOssObjectAppService _ossObjectAppService;
        public OssManagementBlobProvider(
            IOssObjectAppService ossObjectAppService)
        {
            _ossObjectAppService = ossObjectAppService;

            Logger = NullLogger<OssManagementBlobProvider>.Instance;
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var configuration = args.Configuration.GetOssManagementConfiguration();
            await _ossObjectAppService.DeleteAsync(new GetOssObjectInput
            {
                Bucket = configuration.Bucket,
                Path = GetOssPath(args),
                Object = GetOssName(args),
            });
            return true;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            try
            {
                var configuration = args.Configuration.GetOssManagementConfiguration();
                var oss = await _ossObjectAppService.GetAsync(new GetOssObjectInput
                {
                    Bucket = configuration.Bucket,
                    Path = GetOssPath(args),
                    Object = GetOssName(args),
                });
                return oss != null;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while getting the OSS object, always returning that the object does not exist");
                Logger.LogWarning(ex.Message);

                return false;
            }
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            try
            {
                var configuration = args.Configuration.GetOssManagementConfiguration();
                var content = await _ossObjectAppService.GetContentAsync(new GetOssObjectInput
                {
                    Bucket = configuration.Bucket,
                    Path = GetOssPath(args),
                    Object = GetOssName(args),
                });

                return content?.GetStream();
            }
            catch (Exception ex)
            {
                Logger.LogWarning("An error occurred while getting the OSS object and an empty data stream will be returned");
                Logger.LogWarning(ex.Message);

                return null;
            }
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var configuration = args.Configuration.GetOssManagementConfiguration();
            await _ossObjectAppService.CreateAsync(new CreateOssObjectInput
            {
                Bucket = configuration.Bucket,
                Overwrite = args.OverrideExisting,
                Path = GetOssPath(args),
                FileName = GetOssName(args),
                File = new RemoteStreamContent(args.BlobStream)
            });
        }

        protected virtual string GetOssPath(BlobProviderArgs args)
        {
            // ContainerName: blob
            // path1/path2/path3/path3/path5/file.txt   =>  blob/path1/path2/path3/path3/path5/
            var path = args.ContainerName;
            if (args.BlobName.Contains("/"))
            {
                var lastIndex = args.BlobName.LastIndexOf('/');
                path += args.BlobName.Substring(0, lastIndex);
            }

            return path.EnsureEndsWith('/');
        }

        protected virtual string GetOssName(BlobProviderArgs args)
        {
            // path1/path2/path3/path3/path5/file.txt   =>  file.txt
            if (args.BlobName.Contains("/"))
            {
                var lastIndex = args.BlobName.LastIndexOf('/');

                // TODO: 用户传递以 / 为结尾符的文件名,让系统抛出异常?
                return args.BlobName.Substring(lastIndex + 1);
            }

            return args.BlobName;
        }
    }
}
