using LINGYUN.Abp.Tests.Features;
using LINGYUN.Abp.WeChat.Work.ExternalContact.Features;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.WeChat.Work.ExternalContact;

[DependsOn(
    typeof(AbpWeChatWorkExternalContactModule),
    typeof(AbpWeChatWorkTestModule))]
public class AbpWeChatWorkExternalContactTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(WeChatWorkExternalContactFeatureNames.Enable, (feature) =>
            {
                return true.ToString();
            });
        });
    }
}
