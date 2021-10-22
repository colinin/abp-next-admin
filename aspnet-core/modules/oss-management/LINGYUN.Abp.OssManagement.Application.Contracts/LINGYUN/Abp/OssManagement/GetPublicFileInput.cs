using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OssManagement
{
    public class GetPublicFileInput
    {
        [Required]
        public string Name { get; set; }

        public string Path { get; set; }

        public string Process { get; set; }
    }
}
