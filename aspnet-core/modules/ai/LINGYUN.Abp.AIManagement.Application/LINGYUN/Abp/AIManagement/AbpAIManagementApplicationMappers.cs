using LINGYUN.Abp.AIManagement.Workspaces;
using LINGYUN.Abp.AIManagement.Workspaces.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.AIManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class WorkspaceDefinitionRecordToWorkspaceDefinitionRecordDtoMapper : MapperBase<WorkspaceDefinitionRecord, WorkspaceDefinitionRecordDto>
{
    public override partial WorkspaceDefinitionRecordDto Map(WorkspaceDefinitionRecord source);
    public override partial void Map(WorkspaceDefinitionRecord source, WorkspaceDefinitionRecordDto destination);
}