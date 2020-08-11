using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.FileManagement
{
    public class FolderMoveDto
    {
        [Required]
        [StringLength(255)]
        public string MoveToPath { get; set; }
    }
}
