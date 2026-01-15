using Riok.Mapperly.Abstractions;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Identity;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentitySessionToIdentitySessionEtoMapper : MapperBase<IdentitySession, IdentitySessionEto>
{
    public override partial IdentitySessionEto Map(IdentitySession source);
    public override partial void Map(IdentitySession source, IdentitySessionEto destination);
}
