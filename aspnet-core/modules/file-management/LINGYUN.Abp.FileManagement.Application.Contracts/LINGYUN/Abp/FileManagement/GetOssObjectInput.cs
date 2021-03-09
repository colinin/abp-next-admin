using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.FileManagement
{
    public class GetOssObjectInput
    {
        [Required]
        public string Bucket { get; set; }

        public string Path { get; set; }

        [Required]
        public string Object { get; set; }
    }
}
