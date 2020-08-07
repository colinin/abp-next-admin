using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.FileManagement
{
    public class FileSystemUpdateDto
    {
        [Required]
        [StringLength(255)]
        public string NewName { get; set; }
    }
}
