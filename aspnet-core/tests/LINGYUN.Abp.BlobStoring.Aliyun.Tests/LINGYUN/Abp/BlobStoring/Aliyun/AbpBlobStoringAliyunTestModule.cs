using Aliyun.OSS;
using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Aliyun.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringAliyunModule),
        typeof(AbpAliyunTestModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpBlobStoringAliyunTestModule : AbpModule
    {
        private string _bucketName;
        private string _accessKeyId;
        private string _accessKeySecret;
        private string _endPoint;

        private IConfiguration _configuration;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _configuration = context.Services.GetConfiguration();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            _endPoint = _configuration[AliyunBlobProviderConfigurationNames.Endpoint];
            _bucketName = _configuration[AliyunBlobProviderConfigurationNames.BucketName];

            var encryptionService = context.ServiceProvider.GetRequiredService<IStringEncryptionService>();

            _accessKeyId = encryptionService.Decrypt(_configuration["Settings:" + AliyunSettingNames.Authorization.AccessKeyId]);
            _accessKeySecret = encryptionService.Decrypt(_configuration["Settings:" + AliyunSettingNames.Authorization.AccessKeySecret]);
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var ossClient = new OssClient(_endPoint, _accessKeyId, _accessKeySecret);
            if (ossClient.DoesBucketExist(_bucketName))
            {
                ossClient.DeleteBucket(_bucketName);
            }
        }
    }
}
