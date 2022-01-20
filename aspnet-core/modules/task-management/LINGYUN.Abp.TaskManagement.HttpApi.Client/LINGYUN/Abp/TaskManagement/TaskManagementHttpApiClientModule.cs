using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.TaskManagement.HttpApi.Client;

[DependsOn(typeof(TaskManagementApplicationContractsModule))]
[DependsOn(typeof(AbpHttpClientModule))]
public class TaskManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(TaskManagementApplicationContractsModule).Assembly,
            TaskManagementRemoteServiceConsts.RemoteServiceName);
    }
}