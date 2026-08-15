using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.AspNetCore.Mvc.Localization;

public class GetTextByKeyInput
{
    [Required]
    public string Key { get; set; } = default!;

    [Required]
    public string CultureName { get; set; } = default!;

    [Required]
    public string ResourceName { get; set; } = default!;
}
