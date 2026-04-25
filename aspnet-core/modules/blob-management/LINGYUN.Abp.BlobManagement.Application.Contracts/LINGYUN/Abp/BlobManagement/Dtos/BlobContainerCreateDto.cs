using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobContainerCreateDto
{
    [Required]
    [DynamicStringLength(typeof(BlobContainerConsts), nameof(BlobContainerConsts.MaxNameLength))]
    public string Name { get; set; }
}
