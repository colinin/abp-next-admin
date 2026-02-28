using LINGYUN.Abp.Aliyun.Features;
using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Aliyun
{
    [DependsOn(
        typeof(AbpAliyunModule),
        typeof(AbpTestsBaseModule))]
    public class AbpAliyunTestModule : AbpModule
    {
        private const string UserSecretsId = "09233B21-9A8A-43A3-AA75-8D83C8A9537D";

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets(UserSecretsId);
            }));

            Configure<FakeFeatureOptions>(options =>
            {
                options.Map(AliyunFeatureNames.Enable, (_) => "true");
            });
        }
    }
}
