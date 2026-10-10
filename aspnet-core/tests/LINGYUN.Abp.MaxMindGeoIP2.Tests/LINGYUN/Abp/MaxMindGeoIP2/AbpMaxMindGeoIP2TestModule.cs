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
            // 仅中国IP不显示国家
            options.UseCountry = (location) => !string.Equals("中国", location.Country);
            // 仅中国IP显示省份
            options.UseProvince = (location) => string.Equals("中国", location.Country);
        });
    }
}
