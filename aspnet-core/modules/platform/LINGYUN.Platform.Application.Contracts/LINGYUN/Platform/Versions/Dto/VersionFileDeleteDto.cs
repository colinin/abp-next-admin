using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Platform.Versions
{
    public class VersionFileDeleteDto
    {
        [Required]
        public Guid VersionId { get; set; }

        [Required]
        [StringLength(VersionFileConsts.MaxNameLength)]
        public string FileName { get; set; }
    }
}
