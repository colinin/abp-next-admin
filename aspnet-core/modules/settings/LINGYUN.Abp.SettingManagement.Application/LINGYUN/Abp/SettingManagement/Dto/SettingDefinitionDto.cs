using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.SettingManagement;

public class SettingDefinitionDto : ExtensibleObject
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string DefaultValue { get; set; }

    public bool IsVisibleToClients { get; set; }

    public List<string> Providers { get; set; }

    public bool IsInherited { get; set; }

    public bool IsEncrypted { get; set; }

    public bool IsStatic { get; set; }
    public SettingDefinitionDto()
    {
        Providers = new List<string>();
    }
}
