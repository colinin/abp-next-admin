using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    public class FileUploadMerger : ITransientDependency
    {
        private readonly IFileValidater _fileValidater;
        private readonly IOssContainerFactory _ossContainerFactory;
        private readonly IStringLocalizer _stringLocalizer;

        public FileUploadMerger(
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory,
            IStringLocalizer<AbpOssManagementResource> stringLocalizer)
        {
            _fileValidater = fileValidater;
            _ossContainerFactory = ossContainerFactory;
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public virtual async Task<OssObject> MergeAsync(CreateOssObjectInput input)
        {
            if (input.Content.IsNullOrEmpty())
            {
                ThrowValidationException(_stringLocalizer["FileNotBeNullOrEmpty"], "File");
            }

            await _fileValidater.ValidationAsync(new UploadFile
            {
                TotalSize = input.Content.Length,
                FileName = input.Object
            });

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
            return await oss.CreateObjectAsync(createOssObjectRequest);
        }

        protected virtual IOssContainer CreateOssContainer()
        {
            return _ossContainerFactory.Create();
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
