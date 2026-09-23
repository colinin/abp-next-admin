using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.LocalizationManagement;

public class TextDifferenceGetListInput
{
    [Required]
    public string CultureName { get; set; } = default!;

    [Required]
    public string TargetCultureName { get; set; } = default!;

    public string? ResourceName { get; set; }

    public bool? OnlyNull { get; set; }

    public string? Filter { get; set; }
}
