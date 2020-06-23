using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpMessageServiceApplicationContractsModule),
        typeof(AbpMessageServiceDomainModule))]
    public class AbpMessageServiceApplicationModule : AbpModule
    {

    }
}
