using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobDownloadByKeyInput
{
    public Guid? TenantId { get; set; }

    [Required]
    public string Key { get; set; }
}
