using LINGYUN.Abp.Sms.Aliyun;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Account.Security.Aliyun;

[DependsOn(
    typeof(AbpAccountSecurityModule),
    typeof(AbpAliyunSmsModule))]
public class AbpAccountSecurityAliyunModule : AbpModule
{
}
