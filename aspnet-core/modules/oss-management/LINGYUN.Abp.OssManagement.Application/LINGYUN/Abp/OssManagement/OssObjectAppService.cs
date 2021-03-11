using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using LINGYUN.Abp.OssManagement.Permissions;
using LINGYUN.Abp.OssManagement.Settings;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.IO;
using Volo.Abp.Settings;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [Authorize(AbpOssManagementPermissions.OssObject.Default)]
    public class OssObjectAppService : OssManagementApplicationServiceBase, IOssObjectAppService
    {
        protected IOssContainerFactory OssContainerFactory { get; }

        public OssObjectAppService(
            IOssContainerFactory ossContainerFactory)
        {
            OssContainerFactory = ossContainerFactory;
        }

        [Authorize(AbpOssManagementPermissions.OssObject.Create)]
        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public virtual async Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
        {
            if (!input.Content.IsNullOrEmpty())
            {
                // 检查文件大小
                var fileSizeLimited = await SettingProvider
                    .GetAsync(
                        AbpOssManagementSettingNames.FileLimitLength,
                        AbpOssManagementSettingNames.DefaultFileLimitLength);
                if (fileSizeLimited * 1024 * 1024 < input.Content.Length)
                {
                    ThrowValidationException(L["UploadFileSizeBeyondLimit", fileSizeLimited], nameof(input.Content));
                }

                // 文件扩展名
                var fileExtensionName = FileHelper.GetExtension(input.Object);
                var fileAllowExtension = await SettingProvider.GetOrNullAsync(AbpOssManagementSettingNames.AllowFileExtensions);
                // 检查文件扩展名
                if (!fileAllowExtension.Split(',')
                    .Any(fe => fe.Equals(fileExtensionName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    ThrowValidationException(L["NotAllowedFileExtensionName", fileExtensionName], "FileName");
                }
            }

            var oss = CreateOssContainer();

            var createOssObjectRequest = new CreateOssObjectRequest(
                input.Bucket,
                input.Object,
                input.Content,
                input.Path,
                input.ExpirationTime);
            var ossObject = await oss.CreateObjectAsync(createOssObjectRequest);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }

        [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
        public virtual async Task BulkDeleteAsync(BulkDeleteOssObjectInput input)
        {
            var oss = CreateOssContainer();

            await oss.BulkDeleteObjectsAsync(input.Bucket, input.Objects, input.Path);
        }

        [Authorize(AbpOssManagementPermissions.OssObject.Delete)]
        public virtual async Task DeleteAsync(GetOssObjectInput input)
        {
            var oss = CreateOssContainer();

            await oss.DeleteObjectAsync(input.Bucket, input.Object, input.Path);
        }

        public virtual async Task<OssObjectDto> GetAsync(GetOssObjectInput input)
        {
            var oss = CreateOssContainer();

            var ossObject = await oss.GetObjectAsync(input.Bucket, input.Object, input.Path);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }

        protected virtual IOssContainer CreateOssContainer()
        {
            return OssContainerFactory.Create();
        }

        private static void ThrowValidationException(string message, string memberName)
        {
            throw new AbpValidationException(message,
                new List<ValidationResult>
                {
                    new ValidationResult(message, new[] {memberName})
                });
        }
    }
}
