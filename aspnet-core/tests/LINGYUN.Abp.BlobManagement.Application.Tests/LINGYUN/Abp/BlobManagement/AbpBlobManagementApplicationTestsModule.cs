using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LINGYUN.Abp.BlobManagement.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement;

[DependsOn(
    typeof(AbpBlobManagementApplicationModule),
    typeof(AbpBlobManagementEntityFrameworkCoreTestsModule),
    typeof(AbpBlobManagementFileSystemModule))]
public class AbpBlobManagementApplicationTestsModule : AbpModule
{
    /// <summary>
    /// 测试根目录(每个测试宿主一个)
    /// </summary>
    public static string RootPath { get; } = Path.Combine(
        Path.GetTempPath(),
        "LINGYUN.Abp.BlobManagement.Application.Tests",
        Guid.NewGuid().ToString("N"));

    /// <summary>
    /// 测试用存储根目录
    /// </summary>
    public static string BasePath { get; } = Path.Combine(RootPath, "blobs");

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["BlobManagement:Provider"] = "FileSystem",
                ["BlobManagement:FileSystem:BasePath"] = BasePath,
                ["BlobManagement:DownloadKeySecret"] = "application-tests-download-key-secret",
            })
            .Build();

        context.Services.ReplaceConfiguration(configuration);

        Directory.CreateDirectory(BasePath);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<TestAuthorizationService>();
        context.Services.Replace(
            ServiceDescriptor.Singleton<IAuthorizationService>(
                serviceProvider => serviceProvider.GetRequiredService<TestAuthorizationService>()));
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, true);
        }
    }
}
