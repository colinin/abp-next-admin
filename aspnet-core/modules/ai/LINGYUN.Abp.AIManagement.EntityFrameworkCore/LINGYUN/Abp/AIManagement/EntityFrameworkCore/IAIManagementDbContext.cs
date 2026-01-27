using LINGYUN.Abp.AIManagement.Chats;
using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;

[ConnectionStringName(AbpAIManagementDbProperties.ConnectionStringName)]
public interface IAIManagementDbContext : IEfCoreDbContext
{
    DbSet<WorkspaceDefinitionRecord> WorkspaceDefinitions { get; }
    DbSet<TextChatMessageRecord> TextChatMessageRecords { get; }
    DbSet<ConversationRecord> ConversationRecords { get; }
}
