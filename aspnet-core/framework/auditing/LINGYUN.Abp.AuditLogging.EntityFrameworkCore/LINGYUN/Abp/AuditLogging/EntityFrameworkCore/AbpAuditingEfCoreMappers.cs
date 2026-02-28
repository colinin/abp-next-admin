using Riok.Mapperly.Abstractions;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;
using VoloAuditLog = Volo.Abp.AuditLogging.AuditLog;
using VoloAuditLogAction = Volo.Abp.AuditLogging.AuditLogAction;
using VoloEntityChange = Volo.Abp.AuditLogging.EntityChange;
using VoloEntityChangeWithUsername = Volo.Abp.AuditLogging.EntityChangeWithUsername;
using VoloEntityPropertyChange = Volo.Abp.AuditLogging.EntityPropertyChange;

namespace LINGYUN.Abp.AuditLogging.EntityFrameworkCore;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class VoloAuditLogActionToAuditLogActionMapper : MapperBase<VoloAuditLogAction, AuditLogAction>
{
    public override partial AuditLogAction Map(VoloAuditLogAction source);
    public override partial void Map(VoloAuditLogAction source, AuditLogAction destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class VoloEntityPropertyChangeToEntityPropertyChangeMapper : MapperBase<VoloEntityPropertyChange, EntityPropertyChange>
{
    public override partial EntityPropertyChange Map(VoloEntityPropertyChange source);
    public override partial void Map(VoloEntityPropertyChange source, EntityPropertyChange destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class VoloEntityChangeToEntityChangeMapper : MapperBase<VoloEntityChange, EntityChange>
{
    public override partial EntityChange Map(VoloEntityChange source);
    public override partial void Map(VoloEntityChange source, EntityChange destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class VoloAuditLogToAuditLogMapper : MapperBase<VoloAuditLog, AuditLog>
{
    public override partial AuditLog Map(VoloAuditLog source);
    public override partial void Map(VoloAuditLog source, AuditLog destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class VoloEntityChangeWithUsernameToEntityChangeWithUsernameMapper : MapperBase<VoloEntityChangeWithUsername, EntityChangeWithUsername>
{
    public override partial EntityChangeWithUsername Map(VoloEntityChangeWithUsername source);
    public override partial void Map(VoloEntityChangeWithUsername source, EntityChangeWithUsername destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class IdentitySecurityLogToSecurityLogMapper : MapperBase<IdentitySecurityLog, SecurityLog>
{
    public override partial SecurityLog Map(IdentitySecurityLog source);
    public override partial void Map(IdentitySecurityLog source, SecurityLog destination);
}

