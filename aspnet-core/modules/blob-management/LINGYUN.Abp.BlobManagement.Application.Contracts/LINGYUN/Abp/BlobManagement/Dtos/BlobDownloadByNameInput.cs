using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobDownloadByNameInput
{
    [Required]
    [DynamicStringLength(typeof(BlobContainerConsts), nameof(BlobContainerConsts.MaxNameLength))]
    public string ContainerName { get; set; }

    [Required]
    [DynamicStringLength(typeof(BlobConsts), nameof(BlobConsts.MaxNameLength))]
    public string BlobName { get; set; }
}
