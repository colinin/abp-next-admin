using System.ComponentModel.DataAnnotations;
using System.IO;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    public class UploadFileInput
    {
        public string Path { get; set; }
        public string Object { get; set; }
        public bool Overwrite { get; set; } = true;

        [Required]
        [DisableAuditing]
        [DisableValidation]
        public Stream Content { get; set; }
    }
}
