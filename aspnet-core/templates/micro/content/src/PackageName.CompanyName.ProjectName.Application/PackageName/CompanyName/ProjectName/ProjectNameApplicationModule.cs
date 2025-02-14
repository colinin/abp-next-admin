using LINGYUN.Abp.Dynamic.Queryable;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationModule),
    typeof(ProjectNameDomainModule),
    typeof(ProjectNameApplicationContractsModule),
    typeof(AbpDynamicQueryableApplicationModule))]
public class ProjectNameApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<ProjectNameApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<ProjectNameApplicationMapperProfile>(validate: true);
        });
    }
}
