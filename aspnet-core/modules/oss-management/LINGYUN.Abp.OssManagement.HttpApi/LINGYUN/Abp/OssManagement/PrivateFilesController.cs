using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [Area("oss-management")]
    [Route("api/files/private")]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PrivateFilesController : AbpController
    {
        private readonly IPrivateFileAppService _privateFileAppService;

        public PrivateFilesController(
            IPrivateFileAppService privateFileAppService)
        {
            _privateFileAppService = privateFileAppService;

            LocalizationResource = typeof(AbpOssManagementResource);
        }

        [HttpPost]
        [Route("{path}")]
        [Route("{path}/{name}")]
        public virtual async Task<OssObjectDto> UploadAsync(string path, string name)
        {
            if (Request.ContentLength <= 0)
            {
                ThrowValidationException(L["FileNotBeNullOrEmpty"], "File");
            }

            var file = Request.Form.Files[0];
            var fileName = name ?? file.FileName;

            var createOssObjectInput = new UploadPublicFileInput
            {
                Path = path,
                Object = fileName,
                Content = file.OpenReadStream(),
                Overwrite = true
            };

            return await _privateFileAppService.UploadAsync(createOssObjectInput);
        }

        [HttpGet]
        [Route("{name}")]
        [Route("{name}/{process}")]
        [Route("p/{path}/{name}")]
        [Route("p/{path}/{name}/{process}")]
        public virtual async Task<IActionResult> GetAsync(string path, string name, string process)
        {
            var input = new GetPublicFileInput
            {
                Name = name,
                Path = path,
                Process = process
            };
            var fileStream = await _privateFileAppService.GetAsync(input);

            if (fileStream.IsNullOrEmpty())
            {
                return NotFound();
            }

            return File(
                    fileStream,
                    MimeTypes.GetByExtension(Path.GetExtension(input.Name))
                    );
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
