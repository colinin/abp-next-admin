using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OpenObserve;

[DependsOn(
    typeof(AbpTestsBaseModule),
    typeof(AbpOpenObserveModule))]
public class AbpOpenObserveTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpOpenObserveOptions>(options =>
        {
            options.Endpoint = "http://localhost:5080";
            options.Organization = "default";
            options.UserName = "admin@abp.io";
            options.Password = "test-password";
        });
    }
}
