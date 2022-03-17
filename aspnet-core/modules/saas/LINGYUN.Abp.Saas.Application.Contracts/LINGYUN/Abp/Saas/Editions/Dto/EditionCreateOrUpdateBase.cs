using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.Saas.Editions;

public abstract class EditionCreateOrUpdateBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(EditionConsts), nameof(EditionConsts.MaxDisplayNameLength))]
    [Display(Name = "EditionName")]
    public string DisplayName { get; set; }
}
