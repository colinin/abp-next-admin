using COSXML;
using COSXML.Auth;
using LINGYUN.Abp.Tencent.Features;
using LINGYUN.Abp.Tencent.Settings;
using LINGYUN.Abp.Tests;
using LINGYUN.Abp.Tests.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.BlobStoring.Tencent;

[DependsOn(
    typeof(AbpBlobStoringModule),
    typeof(AbpBlobStoringTencentCloudModule),
    typeof(AbpTestsBaseModule),
    typeof(AbpAutofacModule)
    )]
public class AbpBlobStoringTencentTestModule : AbpModule
{
    private string _bucketName;
    private string _secretId;
    private string _secretKey;
    private string _region;
    private string _appId;

    private IConfiguration _configuration;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        _configuration = context.Services.GetConfiguration();

        _appId = _configuration[TencentBlobProviderConfigurationNames.AppId];
        _region = _configuration[TencentBlobProviderConfigurationNames.Region];
        _bucketName = _configuration[TencentBlobProviderConfigurationNames.BucketName];

        Configure<FakeFeatureOptions>(options =>
        {
            options.Map(TencentCloudFeatures.BlobStoring.MaximumStreamSize, (feature) => (int.MaxValue - 1024).ToString());
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseTencentCloud(blob =>
                {
                    blob.AppId = _appId;
                    blob.Region = _region;
                    blob.BucketName = _bucketName;
                    blob.CreateBucketIfNotExists = true;
                });
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var encryptionService = context.ServiceProvider.GetRequiredService<IStringEncryptionService>();

        _secretId = encryptionService.Decrypt(_configuration["Settings:" + TencentCloudSettingNames.SecretId]);
        _secretKey = encryptionService.Decrypt(_configuration["Settings:" + TencentCloudSettingNames.SecretKey]);
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var configBuilder = new CosXmlConfig.Builder();
        configBuilder
            .IsHttps(true)
            .SetAppid(_appId)
            .SetRegion(_region)
            .SetDebugLog(true);

        var cred = new DefaultQCloudCredentialProvider(
            _secretId,
            _secretKey,
            60);

        var ossClient = new CosXmlServer(configBuilder.Build(), cred);
        if (ossClient.DoesBucketExist(new COSXML.Model.Bucket.DoesBucketExistRequest(_bucketName)))
        {
            var bucket = ossClient.GetBucket(new COSXML.Model.Bucket.GetBucketRequest(_bucketName));
            if (bucket.listBucket.contentsList.Any())
            {
                var request = new COSXML.Model.Object.DeleteMultiObjectRequest(_bucketName);
                request.SetObjectKeys(bucket.listBucket.contentsList.Select(x => x.key).ToList());
                ossClient.DeleteMultiObjects(request);
            }
            ossClient.DeleteBucket(new COSXML.Model.Bucket.DeleteBucketRequest(_bucketName));
        }
    }
}
