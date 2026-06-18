using LINGYUN.Abp.Dynamic.Queryable;
using LINGYUN.Abp.Exporter;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAuthorizationModule),
    typeof(AbpDddApplicationModule),
    typeof(ProjectNameDomainModule),
    typeof(ProjectNameApplicationContractsModule),
    typeof(AbpDynamicQueryableApplicationModule),
    typeof(AbpExporterApplicationModule))]
public class ProjectNameApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<ProjectNameApplicationModule>();
    }
}
