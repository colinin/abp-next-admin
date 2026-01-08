using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.IP2Region;

[DependsOn(
    typeof(AbpIP2RegionModule),
    typeof(AbpTestsBaseModule))]
public class AbpIP2RegionTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpIP2RegionLocationResolveOptions>(options =>
        {
            // 仅中国IP不显示国家
            options.UseCountry = (localtion) =>
            {
                return !string.Equals("中国", localtion.Country);
            };
            // 仅中国IP显示省份
            options.UseProvince = (localtion) =>
            {
                return string.Equals("中国", localtion.Country);
            };
        });
    }
}