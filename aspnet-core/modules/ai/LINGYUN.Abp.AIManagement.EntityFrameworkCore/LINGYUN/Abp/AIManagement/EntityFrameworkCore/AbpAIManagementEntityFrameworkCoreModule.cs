using LINGYUN.Abp.AIManagement.Messages;
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

            options.AddRepository<UserTextMessageRecord, EfCoreUserTextMessageRecordRepository>();

            options.AddRepository<WorkspaceDefinitionRecord, EfCoreWorkspaceDefinitionRecordRepository>();
        });
    }
}
