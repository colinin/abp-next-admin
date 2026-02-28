using LINGYUN.Abp.Tests.Features;
using LINGYUN.Abp.WeChat.Work.Contacts.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.Contacts;

[DependsOn(
    typeof(AbpWeChatWorkContactModule),
    typeof(AbpWeChatWorkTestModule))]
public class AbpWeChatWorkContactTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(WeChatWorkContactsFeatureNames.Enable, (feature) =>
            {
                return true.ToString();
            });
        });
    }
}
