using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.OssManagement
{
    [RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("oss-management")]
    [Route("api/oss-management/objects")]
    public class OssObjectController : AbpController, IOssObjectAppService
    {
        protected IFileUploader FileUploader { get; }
        protected IOssObjectAppService OssObjectAppService { get; }

        public OssObjectController(
            IFileUploader fileUploader,
            IOssObjectAppService ossObjectAppService)
        {
            FileUploader = fileUploader;
            OssObjectAppService = ossObjectAppService;
        }

        [HttpPost]
        public virtual async Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
        {
            return await OssObjectAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        [DisableAuditing]
        [Authorize(AbpOssManagementPermissions.OssObject.Create)]
        public virtual async Task UploadAsync([FromForm] UploadOssObjectInput input)
        {
            await FileUploader.UploadAsync(new UploadFileChunkInput
            {
                Path = input.Path,
                Bucket = input.Bucket,
                FileName = input.FileName,
                TotalSize = input.TotalSize,
                ChunkSize = input.ChunkSize,
                ChunkNumber = input.ChunkNumber,
                TotalChunks = input.TotalChunks,
                CurrentChunkSize = input.CurrentChunkSize,
                Content = input.File?.OpenReadStream(),
            }, HttpContext.RequestAborted);
        }

        [HttpDelete]
        [Route("bulk-delete")]
        public virtual async Task BulkDeleteAsync([FromBody] BulkDeleteOssObjectInput input)
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
    }
}
