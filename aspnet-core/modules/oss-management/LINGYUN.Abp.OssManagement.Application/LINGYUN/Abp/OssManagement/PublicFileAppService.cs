using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Features;

namespace LINGYUN.Abp.OssManagement
{
    [AllowAnonymous]
    public class PublicFileAppService : FileAppServiceBase, IPublicFileAppService
    {
        public PublicFileAppService(
           IFileUploader fileUploader,
           IFileValidater fileValidater,
           IOssContainerFactory ossContainerFactory)
           : base(fileUploader, fileValidater, ossContainerFactory)
        {
        }

        [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
        public override async Task DeleteAsync(GetPublicFileInput input)
        {
            await CheckPublicAccessAsync();
            await CheckPolicyAsync(AbpOssManagementPermissions.OssObject.Delete);

            await base.DeleteAsync(input);
        }

        public override async Task UploadAsync(UploadFileChunkInput input)
        {
            await CheckPublicAccessAsync();
            await FeatureChecker.CheckEnabledAsync(AbpOssManagementFeatureNames.OssObject.UploadFile);

            await base.UploadAsync(input);
        }

        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public override async Task<OssObjectDto> UploadAsync(UploadFileInput input)
        {
            await CheckPublicAccessAsync();
            await FeatureChecker.CheckEnabledAsync(AbpOssManagementFeatureNames.OssObject.UploadFile);

            // 公共目录不允许覆盖
            input.Overwrite = false;

            return await base.UploadAsync(input);
        }

        public override async Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
        {
            await CheckPublicAccessAsync();

            return await base.GetListAsync(input);
        }

        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public override async Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input)
        {
            await CheckPublicAccessAsync();
            await FeatureChecker.CheckEnabledAsync(AbpOssManagementFeatureNames.OssObject.DownloadFile);

            return await base.GetAsync(input);
        }

        protected override string GetCurrentBucket()
        {
            return "public";
        }

        protected virtual async Task CheckPublicAccessAsync()
        {
            if (!CurrentUser.IsAuthenticated)
            {
                await FeatureChecker.CheckEnabledAsync(AbpOssManagementFeatureNames.PublicAccess);
            }
        }
    }
}
