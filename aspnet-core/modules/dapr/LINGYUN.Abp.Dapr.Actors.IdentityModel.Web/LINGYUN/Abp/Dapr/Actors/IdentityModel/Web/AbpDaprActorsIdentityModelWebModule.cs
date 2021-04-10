using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors.IdentityModel.Web
{
    [DependsOn(
        typeof(AbpDaprActorsIdentityModelModule))]
    public class AbpDaprActorsIdentityModelWebModule : AbpModule
    {
    }
}
