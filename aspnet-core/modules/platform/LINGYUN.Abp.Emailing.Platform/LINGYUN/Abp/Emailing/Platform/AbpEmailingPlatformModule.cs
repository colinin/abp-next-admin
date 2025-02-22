using LINGYUN.Platform.HttpApi.Client;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Emailing.Platform;

[DependsOn(
    typeof(AbpEmailingModule),
    typeof(PlatformHttpApiClientModule))]
public class AbpEmailingPlatformModule : AbpModule
{

}
