using LINGYUN.Abp.OssManagement.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [Area("oss-management")]
    [Route("api/files/public")]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PublicFilesController : AbpController
    {
        private readonly IPublicFileAppService _publicFileAppService;

        public PublicFilesController(
            IPublicFileAppService publicFileAppService)
        {
            _publicFileAppService = publicFileAppService;

            LocalizationResource = typeof(AbpOssManagementResource);
        }


        [HttpPost]
        [Route("upload")]
        public virtual async Task UploadAsync([FromForm] UploadOssObjectInput input)
        {
            await _publicFileAppService.UploadAsync(new UploadFileChunkInput
            {
                Path = input.Path,
                FileName = input.FileName,
                TotalSize = input.TotalSize,
                ChunkSize = input.ChunkSize,
                ChunkNumber = input.ChunkNumber,
                TotalChunks = input.TotalChunks,
                CurrentChunkSize = input.CurrentChunkSize,
                Content = input.File?.OpenReadStream(),
            });
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

            var createOssObjectInput = new UploadFileInput
            {
                Path = path,
                Object = fileName,
                Content = file.OpenReadStream(),
                Overwrite = true
            };

            return await _publicFileAppService.UploadAsync(createOssObjectInput);
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
        public virtual async Task<IActionResult> GetAsync(string path, string name, string process)
        {
            var input = new GetPublicFileInput
            {
                Name = name,
                Path = path,
                Process = process
            };
            var fileStream = await _publicFileAppService.GetAsync(input);

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
