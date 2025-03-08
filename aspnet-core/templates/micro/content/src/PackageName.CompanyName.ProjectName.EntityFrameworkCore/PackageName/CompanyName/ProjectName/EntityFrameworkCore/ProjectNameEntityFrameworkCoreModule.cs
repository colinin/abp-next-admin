using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName.EntityFrameworkCore;

[DependsOn(
    typeof(ProjectNameDomainModule),
    typeof(AbpDataProtectionEntityFrameworkCoreModule))]
public class ProjectNameEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ProjectNameDbContext>(options =>
        {
            options.AddDefaultRepositories<IProjectNameDbContext>();
        });
    }
}
