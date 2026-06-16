using LINGYUN.Abp.Tests;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Common;

[DependsOn(
    typeof(AbpWeChatCommonModule),
    typeof(AbpTestsBaseModule))]
public class AbpWeChatCommonTestModule : AbpModule
{
}
