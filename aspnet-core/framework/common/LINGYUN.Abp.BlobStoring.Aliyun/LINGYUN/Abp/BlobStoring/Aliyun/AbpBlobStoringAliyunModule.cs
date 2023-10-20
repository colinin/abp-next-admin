using LINGYUN.Abp.Aliyun;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpAliyunModule))]
    public class AbpBlobStoringAliyunModule : AbpModule
    {
        // 需要时引用配置
        //public override void ConfigureServices(ServiceConfigurationContext context)
        //{
        //    var configuration = context.Services.GetConfiguration();

        //    Configure<AbpBlobStoringOptions>(options =>
        //    {
        //        context.Services.ExecutePreConfiguredActions(options);
        //        options.Containers.ConfigureAll((containerName, containerConfiguration) =>
        //        {
        //            containerConfiguration.UseAliyun(aliyun =>
        //            {
        //                aliyun.BucketName = configuration[AliyunBlobProviderConfigurationNames.BucketName] ?? "";
        //                aliyun.CreateBucketIfNotExists = configuration.GetSection(AliyunBlobProviderConfigurationNames.CreateBucketIfNotExists).Get<bool>();
        //                aliyun.CreateBucketReferer = configuration.GetSection(AliyunBlobProviderConfigurationNames.CreateBucketReferer).Get<List<string>>();
        //                aliyun.Endpoint = configuration[AliyunBlobProviderConfigurationNames.Endpoint];
        //            });
        //        });
        //    });
        //}
    }
}
