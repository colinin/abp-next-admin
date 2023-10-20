using LINGYUN.Abp.WxPusher;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity.WxPusher;

[DependsOn(
    typeof(AbpWxPusherModule),
    typeof(AbpIdentityDomainModule))]
public class AbpIdentityWxPusherModule : AbpModule
{
}
