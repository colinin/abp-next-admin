using LINGYUN.Abp.OssManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    [RemoteService(Name = OssManagementRemoteServiceConsts.RemoteServiceName)]
    [Area("oss-management")]
    [Route("api/files/static")]
    public class StaticFilesController : AbpController
    {
        private readonly IOssObjectAppService _ossObjectAppService;
        private readonly IStaticFilesAppService _staticFilesAppServic;

        public StaticFilesController(
            IOssObjectAppService ossObjectAppService,
            IStaticFilesAppService staticFilesAppServic)
        {
            _ossObjectAppService = ossObjectAppService;
            _staticFilesAppServic = staticFilesAppServic;
        }

        [HttpPost]
        [Route("{bucket}")]
        [Route("{bucket}/{path}")]
        [Authorize(AbpOssManagementPermissions.OssObject.Create)]
        public virtual async Task<OssObjectDto> UploadAsync(string bucket, string path, [FromForm] IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                ThrowValidationException(L["FileNotBeNullOrEmpty"], "File");
            }

            var createOssObjectInput = new CreateOssObjectInput
            {
                Bucket = HttpUtility.UrlDecode(bucket),
                Path = HttpUtility.UrlDecode(path),
                Object = file.FileName,
                Content = file.OpenReadStream()
            };

            return await _ossObjectAppService.CreateAsync(createOssObjectInput);
        }

        [HttpGet]
        [Route("{bucket}/{name}")]
        [Route("{bucket}/{name}/{process}")]
        [Route("{bucket}/p/{path}/{name}")]
        [Route("{bucket}/p/{path}/{name}/{process}")]
        public virtual async Task<IActionResult> GetAsync(string bucket, string path, string name, string process)
        {
            var input = new GetStaticFileInput
            {
                Bucket = bucket,
                Name = name,
                Path = path,
                Process = process
            };
            var fileStream = await _staticFilesAppServic.GetAsync(input);

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
