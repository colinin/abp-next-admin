using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Content;

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
        public virtual async Task<OssObjectDto> CreateAsync([FromForm] CreateOssObjectInput input)
        {
            return await OssObjectAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        [DisableAuditing]
        [Authorize(AbpOssManagementPermissions.OssObject.Create)]
        public virtual async Task UploadAsync([FromForm] UploadFileChunkInput input)
        {
            await FileUploader.UploadAsync(input);
        }

        [HttpPost]
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

        [HttpGet]
        [Route("download")]
        public virtual async Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input)
        {
            return await OssObjectAppService.GetContentAsync(input);
        }
    }
}
