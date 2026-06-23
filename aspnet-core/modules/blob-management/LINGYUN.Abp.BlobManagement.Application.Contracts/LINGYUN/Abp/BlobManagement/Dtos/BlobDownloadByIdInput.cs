using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobDownloadByIdInput
{
    [Required]
    public string Key { get; set; }
}
