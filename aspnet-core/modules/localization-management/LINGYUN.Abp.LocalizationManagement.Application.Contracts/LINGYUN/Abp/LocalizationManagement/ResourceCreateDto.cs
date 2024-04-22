using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.LocalizationManagement;
public class ResourceCreateDto : ResourceCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(ResourceConsts), nameof(ResourceConsts.MaxNameLength))]
    public string Name { get; set; }
}
