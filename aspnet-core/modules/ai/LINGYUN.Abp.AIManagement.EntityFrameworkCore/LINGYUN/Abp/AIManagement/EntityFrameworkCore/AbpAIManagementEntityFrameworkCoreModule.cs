using LINGYUN.Abp.AIManagement.Chats;
using LINGYUN.Abp.AIManagement.Tokens;
using LINGYUN.Abp.AIManagement.Workspaces;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.AIManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpAIManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpAIManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AIManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IAIManagementDbContext>();

            options.AddRepository<ConversationRecord, EfCoreConversationRecordRepository>();
            options.AddRepository<TextChatMessageRecord, EfCoreTextChatMessageRecordRepository>();
            options.AddRepository<TokenUsageRecord, EfCoreTokenUsageRecordRepository>();

            options.AddRepository<WorkspaceDefinitionRecord, EfCoreWorkspaceDefinitionRecordRepository>();
        });
    }
}
