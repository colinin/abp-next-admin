using LINGYUN.Abp.Dapr.Client;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpDaprClientModule),
    typeof(ProjectNameApplicationContractsModule))]
public class ProjectNameDaprClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticDaprClientProxies(
            typeof(ProjectNameApplicationContractsModule).Assembly,
            ProjectNameRemoteServiceConsts.RemoteServiceName);
    }
}
