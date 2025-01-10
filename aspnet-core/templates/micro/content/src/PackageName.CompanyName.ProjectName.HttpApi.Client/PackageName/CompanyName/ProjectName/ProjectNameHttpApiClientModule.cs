using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace PackageName.CompanyName.ProjectName;

[DependsOn(
    typeof(AbpHttpClientModule),
    typeof(ProjectNameApplicationContractsModule))]
public class ProjectNameHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(ProjectNameApplicationContractsModule).Assembly,
            ProjectNameRemoteServiceConsts.RemoteServiceName);
    }
}
