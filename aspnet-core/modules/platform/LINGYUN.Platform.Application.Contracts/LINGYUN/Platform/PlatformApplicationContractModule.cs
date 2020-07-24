using Volo.Abp.Modularity;

namespace LINGYUN.Platform
{
    [DependsOn(typeof(PlatformDomainSharedModule))]
    public class PlatformApplicationContractModule : AbpModule
    {

    }
}
