using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Identity;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentitySessionToIdentitySessionEtoMapper : MapperBase<IdentitySession, IdentitySessionEto>
{
    [MapPropertyFromSource(nameof(IdentitySessionEto.Properties), Use = nameof(TryGetProperties))]
    public override partial IdentitySessionEto Map(IdentitySession source);

    [MapPropertyFromSource(nameof(IdentitySessionEto.Properties), Use = nameof(TryGetProperties))]
    public override partial void Map(IdentitySession source, IdentitySessionEto destination);

    [UserMapping(Default = false)]
    private static Dictionary<string, string> TryGetProperties(IdentitySession source)
    {
        var properties = new Dictionary<string, string>();
        if (source != null && source.ExtraProperties != null)
        {
            foreach (var property in source.ExtraProperties)
            {
                properties[property.Key] = property.Value.ToString();
            }
        }
        return properties;
    }
}
