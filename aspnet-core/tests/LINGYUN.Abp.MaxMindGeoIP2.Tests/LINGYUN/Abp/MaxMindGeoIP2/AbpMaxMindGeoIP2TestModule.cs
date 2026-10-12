using LINGYUN.Abp.IP.Location;
using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MaxMindGeoIP2;

[DependsOn(
    typeof(AbpMaxMindGeoIP2Module),
    typeof(AbpTestsBaseModule))]
public class AbpMaxMindGeoIP2TestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIPLocationResolveOptions>(options =>
        {
            // 中国IP不显示国家(从备注中移除)
            options.RemoveCountry = new[] { "中国" };
        });
    }
}
