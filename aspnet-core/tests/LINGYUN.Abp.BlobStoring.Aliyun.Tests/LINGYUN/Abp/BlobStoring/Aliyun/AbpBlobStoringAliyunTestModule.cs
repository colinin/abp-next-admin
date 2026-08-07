using AlibabaCloud.OSS.V2;
using AlibabaCloud.OSS.V2.Credentials;
using LINGYUN.Abp.Aliyun;
using LINGYUN.Abp.Aliyun.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;
using Volo.Abp.Threading;

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

        public async override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
        {
            var ossClient = new Client(
                new Configuration
                {
                    Endpoint = _endPoint,
                    CredentialsProvider = new StaticCredentialsProvider(_accessKeyId, _accessKeySecret),
                });
            if (await ossClient.IsBucketExistAsync(_bucketName))
            {
                await ossClient.DeleteBucketAsync(new AlibabaCloud.OSS.V2.Models.DeleteBucketRequest
                {
                    Bucket = _bucketName,
                });
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            AsyncHelper.RunSync(async () => await OnApplicationShutdownAsync(context));
        }
    }
}
