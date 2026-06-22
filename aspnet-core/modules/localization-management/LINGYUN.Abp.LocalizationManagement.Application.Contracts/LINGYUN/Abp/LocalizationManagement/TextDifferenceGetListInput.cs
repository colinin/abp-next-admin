using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.LocalizationManagement;

public class TextDifferenceGetListInput
{
    [Required]
    public string CultureName { get; set; }

    [Required]
    public string TargetCultureName { get; set; }

    public string ResourceName { get; set; }

    public bool? OnlyNull { get; set; }

    public string Filter { get; set; }
}
