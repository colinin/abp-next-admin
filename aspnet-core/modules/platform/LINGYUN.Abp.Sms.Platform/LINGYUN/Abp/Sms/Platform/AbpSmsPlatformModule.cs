using LINGYUN.Platform.HttpApi.Client;
using Volo.Abp.Modularity;
using Volo.Abp.Sms;

namespace LINGYUN.Abp.Sms.Platform;

[DependsOn(
    typeof(AbpSmsModule),
    typeof(PlatformHttpApiClientModule))]
public class AbpSmsPlatformModule : AbpModule
{

}
