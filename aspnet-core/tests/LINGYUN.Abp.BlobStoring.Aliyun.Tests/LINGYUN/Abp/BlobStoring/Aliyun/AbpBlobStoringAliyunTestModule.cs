using Aliyun.OSS;
using LINGYUN.Abp.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    [DependsOn(
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringAliyunModule),
        typeof(AbpTestsBaseModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpBlobStoringAliyunTestModule : AbpModule
    {
        private string _bucketName;
        private string _accessKeyId;
        private string _accessKeySecret;
        private string _endPoint;

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configurationOptions = new AbpConfigurationBuilderOptions
            {
                BasePath = @"D:\Projects\Development\Abp\BlobStoring\Aliyun",
                EnvironmentName = "Development"
            };

            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(configurationOptions));
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            _endPoint = configuration[AliyunBlobProviderConfigurationNames.Endpoint];
            _bucketName = configuration[AliyunBlobProviderConfigurationNames.BucketName];
            _accessKeyId = configuration["Aliyun:Auth:AccessKeyId"];
            _accessKeySecret = configuration["Aliyun:Auth:AccessKeySecret"];
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
