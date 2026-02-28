using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Tests.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Sms.Aliyun
{
    [DependsOn(
        typeof(AbpAliyunTestModule),
        typeof(AbpAliyunSmsModule))]
    public class AbpAliyunSmsTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FakeFeatureOptions>(options =>
            {
                options.Map(AliyunFeatureNames.Sms.Enable, (_) => "true");
            });
        }
    }
}
