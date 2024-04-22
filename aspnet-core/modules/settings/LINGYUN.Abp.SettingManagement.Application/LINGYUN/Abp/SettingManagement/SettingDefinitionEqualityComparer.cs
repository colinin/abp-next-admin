using System.Collections.Generic;
using Volo.Abp.Settings;

namespace LINGYUN.Abp.SettingManagement;
public class SettingDefinitionEqualityComparer : EqualityComparer<SettingDefinition>
{
    public override bool Equals(SettingDefinition x, SettingDefinition y)
    {
        return x.Name == y.Name;
    }

    public override int GetHashCode(SettingDefinition obj)
    {
        return obj.Name.GetHashCode();
    }
}
