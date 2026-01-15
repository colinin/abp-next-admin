using LINGYUN.Abp.AuditLogging;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Account;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class SecurityLogToSecurityLogDtoMapper : MapperBase<SecurityLog, SecurityLogDto>
{
    public override partial SecurityLogDto Map(SecurityLog source);
    public override partial void Map(SecurityLog source, SecurityLogDto destination);
}

