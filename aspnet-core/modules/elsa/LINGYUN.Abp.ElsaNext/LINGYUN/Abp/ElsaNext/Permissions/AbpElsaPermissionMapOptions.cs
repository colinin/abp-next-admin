using System.Collections.Generic;
using System.Linq;

namespace LINGYUN.Abp.ElsaNext.Permissions;

public class AbpElsaPermissionMapOptions
{
    public IList<ElsaPermissionMapRecord> PermissionMaps { get; }
    public AbpElsaPermissionMapOptions()
    {
        PermissionMaps = new List<ElsaPermissionMapRecord>();
    }

    public AbpElsaPermissionMapOptions MapPermission(string source, string target)
    {
        if (!PermissionMaps.Any(x => x.Target == target))
        {
            PermissionMaps.Add(new ElsaPermissionMapRecord(source, target));
        }

        return this;
    }
}

public class ElsaPermissionMapRecord
{
    /// <summary>
    /// Abp Permission 
    /// </summary>
    public string Source { get; }
    /// <summary>
    /// Elsa Permission
    /// </summary>
    public string Target { get; }
    public ElsaPermissionMapRecord(string source, string target)
    {
        Source = source;
        Target = target;
    }
}