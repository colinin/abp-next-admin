using LINGYUN.Abp.AIManagement.Workspaces;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.AIManagement.Chats.Dtos;
public class SendTextChatMessageDto
{
    [Required]
    [DynamicStringLength(typeof(WorkspaceDefinitionRecordConsts), nameof(WorkspaceDefinitionRecordConsts.MaxNameLength))]
    public string Workspace { get; set; }

    [Required]
    public Guid ConversationId { get; set; }

    [Required]
    [DynamicStringLength(typeof(TextChatMessageRecordConsts), nameof(TextChatMessageRecordConsts.MaxContentLength))]
    public string Content { get; set; }
}
