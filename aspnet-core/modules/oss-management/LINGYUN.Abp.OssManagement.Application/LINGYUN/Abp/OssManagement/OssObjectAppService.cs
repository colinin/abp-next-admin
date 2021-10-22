using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [Authorize(AbpOssManagementPermissions.OssObject.Default)]
    public class OssObjectAppService : OssManagementApplicationServiceBase, IOssObjectAppService
    {
        protected IFileValidater FileValidater { get; }
        protected IOssContainerFactory OssContainerFactory { get; }

        public OssObjectAppService(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
        {
            FileValidater = fileValidater;
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
                await FileValidater.ValidationAsync(new UploadFile
                {
                    TotalSize = input.Content.Length,
                    FileName = input.Object
                });
            }

            var oss = CreateOssContainer();

            var createOssObjectRequest = new CreateOssObjectRequest(
                input.Bucket,
                input.Object,
                input.Content,
                input.Path,
                input.ExpirationTime)
            {
                Overwrite = input.Overwrite
            };
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
