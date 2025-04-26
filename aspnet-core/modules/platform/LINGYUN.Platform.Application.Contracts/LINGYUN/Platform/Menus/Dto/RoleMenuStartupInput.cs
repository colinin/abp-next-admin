using LINGYUN.Platform.Routes;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;

public class RoleMenuStartupInput
{
    [Required]
    [StringLength(80)]
    public string RoleName { get; set; }

    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]
    public string Framework { get; set; }
}
