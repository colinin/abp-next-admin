using LINGYUN.Abp.Features.LimitValidation;
using LINGYUN.Abp.OssManagement.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Features;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    public abstract class FileAppServiceBase : OssManagementApplicationServiceBase, IFileAppService
    {
        protected IFileUploader FileUploader { get; }
        protected IFileValidater FileValidater { get; }
        protected IOssContainerFactory OssContainerFactory { get; }

        protected FileAppServiceBase(
            IFileUploader fileUploader,
            IFileValidater fileValidater,
            IOssContainerFactory ossContainerFactory)
        {
            FileUploader = fileUploader;
            FileValidater = fileValidater;
            OssContainerFactory = ossContainerFactory;
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        public virtual async Task UploadAsync(UploadFileChunkInput input)
        {
            input.Bucket = GetCurrentBucket();
            input.Path = GetCurrentPath(HttpUtility.UrlDecode(input.Path));
            await FileUploader.UploadAsync(input);
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.UploadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.UploadLimit,
            AbpOssManagementFeatureNames.OssObject.UploadInterval,
            LimitPolicy.Month)]
        public virtual async Task<OssObjectDto> UploadAsync(UploadFileInput input)
        {
            if (input.File == null || !input.File.ContentLength.HasValue)
            {
                ThrowValidationException(L["FileNotBeNullOrEmpty"], "File");
            }

            await FileValidater.ValidationAsync(new UploadFile
            {
                TotalSize = input.File.ContentLength.Value,
                FileName = input.Object
            });

            var oss = OssContainerFactory.Create();

            var createOssObjectRequest = new CreateOssObjectRequest(
                 GetCurrentBucket(),
                 HttpUtility.UrlDecode(input.Object),
                 input.File.GetStream(),
                 GetCurrentPath(HttpUtility.UrlDecode(input.Path)))
            {
                Overwrite = input.Overwrite
            };

            var ossObject = await oss.CreateObjectAsync(createOssObjectRequest);

            return ObjectMapper.Map<OssObject, OssObjectDto>(ossObject);
        }

        public virtual async Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
        {
            var ossContainer = OssContainerFactory.Create();
            var response = await ossContainer.GetObjectsAsync(
                GetCurrentBucket(),
                GetCurrentPath(HttpUtility.UrlDecode(input.Path)),
                skipCount: 0,
                maxResultCount: input.MaxResultCount);

            return new ListResultDto<OssObjectDto>(
                ObjectMapper.Map<List<OssObject>, List<OssObjectDto>>(response.Objects));
        }

        [RequiresFeature(AbpOssManagementFeatureNames.OssObject.DownloadFile)]
        [RequiresLimitFeature(
            AbpOssManagementFeatureNames.OssObject.DownloadLimit,
            AbpOssManagementFeatureNames.OssObject.DownloadInterval,
            LimitPolicy.Month)]
        public virtual async Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input)
        {
            var ossObjectRequest = new GetOssObjectRequest(
                GetCurrentBucket(),
                // 需要处理特殊字符
                HttpUtility.UrlDecode(input.Name),
                GetCurrentPath(HttpUtility.UrlDecode(input.Path)),
                HttpUtility.UrlDecode(input.Process))
            {
                MD5 = true,
            };

            var ossContainer = OssContainerFactory.Create();
            var ossObject = await ossContainer.GetObjectAsync(ossObjectRequest);

            return new RemoteStreamContent(ossObject.Content);
        }

        public virtual async Task DeleteAsync(GetPublicFileInput input)
        {
            var ossContainer = OssContainerFactory.Create();

            await ossContainer.DeleteObjectAsync(
                GetCurrentBucket(),
                HttpUtility.UrlDecode(input.Name),
                GetCurrentPath(input.Path));
        }

        protected virtual string GetCurrentBucket()
        {
            throw new System.NotImplementedException();
        }

        protected virtual string GetCurrentPath(string path)
        {
            if (path.IsNullOrWhiteSpace())
            {
                return "";
            }
            path = HttpUtility.UrlDecode(path);
            path = path.RemovePreFix(".").RemovePreFix("/");
            return path;
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
