using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors.IdentityModel
{
    [DependsOn(
       typeof(AbpDaprActorsModule),
       typeof(AbpIdentityModelModule)
       )]
    public class AbpDaprActorsIdentityModelModule : AbpModule
    {
    }
}
