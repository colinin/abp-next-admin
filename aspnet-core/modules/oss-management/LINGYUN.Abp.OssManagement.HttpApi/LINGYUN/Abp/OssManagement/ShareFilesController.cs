using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace LINGYUN.Abp.OssManagement
{
    [Area("oss-management")]
    [Route("api/files/share")]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShareFilesController : AbpController
    {
        private readonly IShareFileAppService _service;

        public ShareFilesController(
           IShareFileAppService service)
        {
            _service = service;

            LocalizationResource = typeof(AbpOssManagementResource);
        }

        [HttpGet]
        [Route("{url}")]
        public virtual async Task<IActionResult> GetAsync(string url)
        {
            var ossObject = await _service.GetAsync(url);

            if (ossObject.Content.IsNullOrEmpty())
            {
                return NotFound();
            }

            return File(ossObject.Content, MimeTypes.GetByExtension(Path.GetExtension(ossObject.Name)));
        }
    }
}
