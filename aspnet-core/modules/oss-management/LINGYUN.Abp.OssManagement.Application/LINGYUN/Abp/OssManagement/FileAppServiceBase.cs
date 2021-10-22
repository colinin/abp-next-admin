using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Features;

namespace LINGYUN.Abp.OssManagement
{
    public abstract class FileAppServiceBase : OssManagementApplicationServiceBase, IFileAppService
    {
        private readonly IFileValidater _fileValidater;
        private readonly IOssContainerFactory _ossContainerFactory;

        protected FileAppServiceBase(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
        {
            _fileValidater = fileValidater;
            _ossContainerFactory = ossContainerFactory;
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public virtual async Task<OssObjectDto> UploadAsync(UploadPublicFileInput input)
        {
            await _fileValidater.ValidationAsync(new UploadFile
            {
                TotalSize = input.Content.Length,
                FileName = input.Object
            });

            var oss = _ossContainerFactory.Create();

            var createOssObjectRequest = new CreateOssObjectRequest(
                 GetCurrentBucket(),
                 HttpUtility.UrlDecode(input.Object),
                 input.Content,
                 GetCurrentPath(HttpUtility.UrlDecode(input.Path)))
            {
                Overwrite = input.Overwrite
            };

            var ossObject = await oss.CreateObjectAsync(createOssObjectRequest);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.DownloadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public virtual async Task<Stream> GetAsync(GetPublicFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                GetCurrentBucket(),
                // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                GetCurrentPath(HttpUtility.UrlDecode(input.Path)),
                HttpUtility.UrlDecode(input.Process));

            var ossContainer = _ossContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return ossObject.Content;
        }

        protected virtual string GetCurrentBucket()
        {
            throw new System.NotImplementedException();
        }

        protected virtual string GetCurrentPath(string path)
        {
            path = path.RemovePreFix(".").RemovePreFix("/");
            return path;
        }
    }
}
