using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account.Web
{
    [DependsOn(typeof(Volo.Abp.Account.Web.AbpAccountWebModule))]
    public class AbpAccountWebModule : AbpModule
    {
    }
}
