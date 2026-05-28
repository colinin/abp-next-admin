using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobDownloadByIdInput
{
    public Guid? TenantId { get; set; }

    [Required]
    public Guid Id { get; set; }
}
