using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Workflow.Elsa;

[DependsOn(typeof(AbpWorkflowElsaModule))]
public class AbpWorkflowElsaServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddElsaApiEndpoints();
    }
}
