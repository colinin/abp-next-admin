using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MessageService
{
    [DependsOn(
        typeof(AbpMessageServiceApplicationContrantsModule),
        typeof(AbpMessageServiceDomainModule))]
    public class AbpMessageServiceApplicationModule : AbpModule
    {

    }
}
