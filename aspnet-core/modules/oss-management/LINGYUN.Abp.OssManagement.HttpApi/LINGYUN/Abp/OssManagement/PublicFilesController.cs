using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement
{
    [RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("oss-management")]
    [Route("api/files/public")]
    public class PublicFilesController : AbpController, IPublicFileAppService
    {
        private readonly IPublicFileAppService _publicFileAppService;

        public PublicFilesController(
            IPublicFileAppService publicFileAppService)
        {
            _publicFileAppService = publicFileAppService;

            LocalizationResource = typeof(AbpOssManagementResource);
        }


        [HttpPost]
        public virtual async Task<OssObjectDto> UploadAsync([FromForm] UploadFileInput input)
        {
            return await _publicFileAppService.UploadAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        public virtual async Task UploadAsync(UploadFileChunkInput input)
        {
            await _publicFileAppService.UploadAsync(input);
        }

        [HttpGet]
        [Route("search")]
        public virtual async Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
        {
            return await _publicFileAppService.GetListAsync(input);
        }


        [HttpGet]
        [Route("{name}")]
        [Route("{name}/{process}")]
        [Route("p/{path}/{name}")]
        [Route("p/{path}/{name}/{process}")]
        public virtual async Task<IRemoteStreamContent> GetAsync(GetPublicFileInput input)
        {
            return await _publicFileAppService.GetAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GetPublicFileInput input)
        {
            await _publicFileAppService.DeleteAsync(input);
        }
    }
}
