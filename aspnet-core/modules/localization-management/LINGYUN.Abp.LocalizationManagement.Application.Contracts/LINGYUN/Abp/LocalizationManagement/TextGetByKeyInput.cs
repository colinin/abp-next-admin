using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.LocalizationManagement;

public class TextGetByKeyInput
{
    [Required]
    public string Key { get; set; }

    [Required]
    public string CultureName { get; set; }

    [Required]
    public string ResourceName { get; set; }
}
