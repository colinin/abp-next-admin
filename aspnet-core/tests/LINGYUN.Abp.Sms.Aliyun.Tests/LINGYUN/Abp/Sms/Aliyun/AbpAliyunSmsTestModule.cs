using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Sms.Aliyun;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Sms.Aliyun
{
    [DependsOn(
        typeof(AbpAliyunTestModule),
        typeof(AbpAliyunSmsModule))]
    public class AbpAliyunSmsTestModule : AbpModule
    {
    }
}
