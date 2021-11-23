using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("oss-management")]
    [Route("api/files/private")]
    public class PrivateFilesController : AbpController, IPrivateFileAppService
    {
        private readonly IPrivateFileAppService _service;

        public PrivateFilesController(
            IPrivateFileAppService service)
        {
            _service = service;

            LocalizationResource = typeof(AbpOssManagementResource);
        }

        [HttpPost]
        public virtual async Task<OssObjectDto> UploadAsync([FromForm] UploadFileInput input)
        {
            return await _service.UploadAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        public virtual async Task UploadAsync([FromForm] UploadFileChunkInput input)
        {
            await _service.UploadAsync(input);
        }

        [HttpGet]
        [Route("search")]
        public virtual async Task<ListResultDto<OssObjectDto>> GetListAsync(GetFilesInput input)
        {
            return await _service.GetListAsync(input);
        }

        [HttpGet]
        [Route("{Name}")]
        [Route("{Name}/{Process}")]
        [Route("p/{Path}/{Name}")]
        [Route("p/{Path}/{Name}/{Process}")]
        public virtual async Task<IRemoteStreamContent> GetAsync([FromRoute] GetPublicFileInput input)
        {
            return await _service.GetAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GetPublicFileInput input)
        {
            await _service.DeleteAsync(input);
        }

        [HttpGet]
        [Route("share")]
        public virtual async Task<ListResultDto<MyFileShareDto>> GetShareListAsync()
        {
            return await _service.GetShareListAsync();
        }

        [HttpPost]
        [Route("share")]
        public virtual async Task<FileShareDto> ShareAsync(FileShareInput input)
        {
            return await _service.ShareAsync(input);
        }
    }
}
