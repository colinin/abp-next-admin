using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BlobManagement.Integration.Dtos;

public class BlobFileGetByNameIntegrationDto
{
    [Required]
    [DynamicStringLength(typeof(BlobContainerConsts), nameof(BlobContainerConsts.MaxNameLength))]
    public string ContainerName { get; set; }

    [Required]
    [DynamicStringLength(typeof(BlobConsts), nameof(BlobConsts.MaxNameLength))]
    public string BlobName { get; set; }
}
