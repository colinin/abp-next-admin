using LINGYUN.Abp.Auditing.AuditLogs;
using LINGYUN.Abp.Auditing.Logging;
using LINGYUN.Abp.Auditing.SecurityLogs;
using LINGYUN.Abp.AuditLogging;
using LINGYUN.Abp.Logging;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Auditing;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class AuditLogActionToAuditLogActionDtoMapper : MapperBase<AuditLogAction, AuditLogActionDto>
{
    public override partial AuditLogActionDto Map(AuditLogAction source);
    public override partial void Map(AuditLogAction source, AuditLogActionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityPropertyChangeToEntityPropertyChangeDtoMapper : MapperBase<EntityPropertyChange, EntityPropertyChangeDto>
{
    public override partial EntityPropertyChangeDto Map(EntityPropertyChange source);
    public override partial void Map(EntityPropertyChange source, EntityPropertyChangeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class EntityChangeWithUsernameToEntityChangeWithUsernameDtoMapper : MapperBase<EntityChangeWithUsername, EntityChangeWithUsernameDto>
{
    public override partial EntityChangeWithUsernameDto Map(EntityChangeWithUsername source);
    public override partial void Map(EntityChangeWithUsername source, EntityChangeWithUsernameDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class EntityChangeToEntityChangeDtoMapper : MapperBase<EntityChange, EntityChangeDto>
{
    public override partial EntityChangeDto Map(EntityChange source);
    public override partial void Map(EntityChange source, EntityChangeDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class AuditLogToAuditLogDtoMapper : MapperBase<AuditLog, AuditLogDto>
{
    public override partial AuditLogDto Map(AuditLog source);
    public override partial void Map(AuditLog source, AuditLogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class SecurityLogToSecurityLogDtoMapper : MapperBase<SecurityLog, SecurityLogDto>
{
    public override partial SecurityLogDto Map(SecurityLog source);
    public override partial void Map(SecurityLog source, SecurityLogDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LogFieldToLogFieldDtoMapper : MapperBase<LogField, LogFieldDto>
{
    public override partial LogFieldDto Map(LogField source);
    public override partial void Map(LogField source, LogFieldDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LogExceptionToLogExceptionDtoMapper : MapperBase<LogException, LogExceptionDto>
{
    public override partial LogExceptionDto Map(LogException source);
    public override partial void Map(LogException source, LogExceptionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class LogInfoToLogDtoMapper : MapperBase<LogInfo, LogDto>
{
    public override partial LogDto Map(LogInfo source);
    public override partial void Map(LogInfo source, LogDto destination);
}
