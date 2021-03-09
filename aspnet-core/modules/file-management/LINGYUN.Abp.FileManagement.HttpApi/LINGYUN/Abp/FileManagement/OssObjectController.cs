using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FileManagement
{
    [RemoteService(Name = "AbpFileManagement")]
    [Area("file-management")]
    [Route("api/file-management/objects")]
    public class OssObjectController : AbpController, IOssObjectAppService
    {
        protected IOssObjectAppService OssObjectAppService { get; }

        public OssObjectController(
            IOssObjectAppService ossObjectAppService)
        {
            OssObjectAppService = ossObjectAppService;
        }

        [HttpPost]
        public virtual async Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
        {
            return await OssObjectAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        public virtual async Task<OssObjectDto> UploadAsync([FromForm] UploadOssObjectInput input)
        {
            var createOssObjectInput = new CreateOssObjectInput
            {
                Bucket = input.Bucket,
                Path = input.Path,
                Object = input.Name
            };
            if (input.File != null)
            {
                //if (input.File.Length <= 0)
                //{
                //    ThrowValidationException(L["FileNotBeNullOrEmpty"], "File");
                //}
                createOssObjectInput.Object = input.File.FileName;
                createOssObjectInput.Content = input.File.OpenReadStream();
            }

            return await OssObjectAppService.CreateAsync(createOssObjectInput);
        }

        [HttpDelete]
        [Route("bulk-delete")]
        public virtual async Task BulkDeleteAsync(BulkDeleteOssObjectInput input)
        {
            await OssObjectAppService.BulkDeleteAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GetOssObjectInput input)
        {
            await OssObjectAppService.DeleteAsync(input);
        }

        [HttpGet]
        public virtual async Task<OssObjectDto> GetAsync(GetOssObjectInput input)
        {
            return await OssObjectAppService.GetAsync(input);
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
