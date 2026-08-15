using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.LocalizationManagement;

public class TextGetByKeyInput
{
    [Required]
    public string Key { get; set; } = default!;

    [Required]
    public string CultureName { get; set; } = default!;

    [Required]
    public string ResourceName { get; set; } = default!;
}
