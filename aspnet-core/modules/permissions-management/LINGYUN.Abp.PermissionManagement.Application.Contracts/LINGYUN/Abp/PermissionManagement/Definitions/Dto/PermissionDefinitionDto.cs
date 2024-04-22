using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

public class PermissionDefinitionDto : IHasExtraProperties
{
    public string Name { get; set; }

    public string ParentName { get; set; }

    public string DisplayName { get; set; }

    public string GroupName { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsStatic { get; set; }

    public MultiTenancySides MultiTenancySide { get; set; }

    public List<string> Providers { get; set; } = new List<string>();

    public string StateCheckers { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; set; } = new ExtraPropertyDictionary();
}
