using LINGYUN.Abp.AIManagement.Chats;
using LINGYUN.Abp.AIManagement.EntityFrameworkCore;
using LINGYUN.Abp.AIManagement.Tokens;
using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.AIService;

[ConnectionStringName("Default")]
public class AIServiceMigrationsDbContext : 
    AbpDbContext<AIServiceMigrationsDbContext>,
    IAIManagementDbContext
{
    public DbSet<WorkspaceDefinitionRecord> WorkspaceDefinitions { get; set; }
    public DbSet<TextChatMessageRecord> TextChatMessageRecords { get; set; }
    public DbSet<ConversationRecord> ConversationRecords { get; set; }
    public DbSet<TokenUsageRecord> TokenUsageRecords { get; set; }

    public AIServiceMigrationsDbContext(
        DbContextOptions<AIServiceMigrationsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAIManagement();
    }
}
