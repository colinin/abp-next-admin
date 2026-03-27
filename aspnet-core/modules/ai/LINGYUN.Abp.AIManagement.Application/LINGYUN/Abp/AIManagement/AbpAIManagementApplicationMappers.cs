using LINGYUN.Abp.AIManagement.Chats;
using LINGYUN.Abp.AIManagement.Chats.Dtos;
using LINGYUN.Abp.AIManagement.Tools;
using LINGYUN.Abp.AIManagement.Tools.Dtos;
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

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class TextChatMessageRecordToTextChatMessageDtoMapper : MapperBase<TextChatMessageRecord, TextChatMessageDto>
{
    [MapPropertyFromSource(nameof(TextChatMessageDto.Role), Use = nameof(ConvertChatRole))]
    public override partial TextChatMessageDto Map(TextChatMessageRecord source);

    [MapPropertyFromSource(nameof(TextChatMessageDto.Role), Use = nameof(ConvertChatRole))]
    public override partial void Map(TextChatMessageRecord source, TextChatMessageDto destination);

    [UserMapping(Default = false)]
    private static string ConvertChatRole(TextChatMessageRecord record)
    {
        return record.Role.Value;
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties(DefinitionChecks = MappingPropertyDefinitionChecks.None)]
public partial class AIToolDefinitionRecordToAIToolDefinitionRecordDtoMapper : MapperBase<AIToolDefinitionRecord, AIToolDefinitionRecordDto>
{
    public override partial AIToolDefinitionRecordDto Map(AIToolDefinitionRecord source);
    public override partial void Map(AIToolDefinitionRecord source, AIToolDefinitionRecordDto destination);
}
