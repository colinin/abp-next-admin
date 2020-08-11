using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.FileManagement
{
    public class FileCopyOrMoveDto
    {
        [StringLength(255)]
        public string Path { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string ToPath { get; set; }

        [StringLength(255)]
        public string ToName { get; set; }
    }
}
