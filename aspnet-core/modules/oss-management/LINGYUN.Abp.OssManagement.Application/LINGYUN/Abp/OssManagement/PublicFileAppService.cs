using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace LINGYUN.Abp.OssManagement
{
    public class PublicFileAppService : FileAppServiceBase, IPublicFileAppService
    {
        public PublicFileAppService(
           IFileUploader fileUploader,
           IFileValidater fileValidater,
           IOssContainerFactory ossContainerFactory)
           : base(fileUploader, fileValidater, ossContainerFactory)
        {
        }

        [RequiresFeature(
            AbpOssManagementFeatureNames.PublicAccess,
            AbpOssManagementFeatureNames.OssObject.UploadFile,
            RequiresAll = true)]
        public override async Task UploadAsync(UploadFileChunkInput input)
        {
            await base.UploadAsync(input);
        }

        [RequiresFeature(
           AbpOssManagementFeatureNames.PublicAccess,
           AbpOssManagementFeatureNames.OssObject.UploadFile,
           RequiresAll = true)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public override async Task<OssObjectDto> UploadAsync(UploadFileInput input)
        {
            return await base.UploadAsync(input);
        }

        [RequiresFeature(
            AbpOssManagementFeatureNames.PublicAccess,
            AbpOssManagementFeatureNames.OssObject.DownloadFile,
            RequiresAll = true)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public override async Task<Stream> GetAsync(GetPublicFileInput input)
        {
            return await base.GetAsync(input);
        }

        protected override string GetCurrentBucket()
        {
            return "public";
        }
    }
}
