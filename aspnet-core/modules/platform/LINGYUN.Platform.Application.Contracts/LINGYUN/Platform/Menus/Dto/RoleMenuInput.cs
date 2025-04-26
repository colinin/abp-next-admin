using LINGYUN.Platform.Routes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Platform.Menus;

public class RoleMenuInput
{
    [Required]
    [StringLength(80)]
    public string RoleName { get; set; }

    [DynamicStringLength(typeof(LayoutConsts), nameof(LayoutConsts.MaxFrameworkLength))]
    public string Framework { get; set; }

    public Guid? StartupMenuId { get; set; }

    [Required]
    public List<Guid> MenuIds { get; set; } = new List<Guid>();
}
