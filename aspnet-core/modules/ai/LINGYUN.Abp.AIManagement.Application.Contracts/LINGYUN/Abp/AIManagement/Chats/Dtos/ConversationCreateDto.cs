using LINGYUN.Abp.AIManagement.Workspaces;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class ConversationCreateDto
{
    [DynamicStringLength(typeof(ConversationRecordConsts), nameof(ConversationRecordConsts.MaxNameLength))]
    public string? Name { get; set; }

    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxNameLength))]
    public string Workspace { get; set; }

}
