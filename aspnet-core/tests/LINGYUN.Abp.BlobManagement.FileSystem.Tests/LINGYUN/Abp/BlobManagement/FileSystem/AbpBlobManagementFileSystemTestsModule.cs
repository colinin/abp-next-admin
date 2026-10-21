#nullable enable
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.FileSystem;

[DependsOn(
    typeof(AbpBlobManagementFileSystemModule),
    typeof(AbpBlobManagementDomainTestsModule))]
public class AbpBlobManagementFileSystemTestsModule : AbpModule
{
    /// <summary>
    /// 测试根目录(每个测试宿主一个)
    /// </summary>
    public static string RootPath { get; } = Path.Combine(
        Path.GetTempPath(),
        "LINGYUN.Abp.BlobManagement.FileSystem.Tests",
        Guid.NewGuid().ToString("N"));

    /// <summary>
    /// 测试用存储根目录(BlobManagement:FileSystem:BasePath)
    /// </summary>
    public static string BasePath { get; } = Path.Combine(RootPath, "blobs");

    /// <summary>
    /// 存储根目录之外的目录, 用于验证穿越/越权访问会被拒绝
    /// </summary>
    public static string OutsidePath { get; } = Path.Combine(RootPath, "outside");

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // 让生产模块按配置启用 FileSystem provider(BlobManagement:Provider)
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["BlobManagement:Provider"] = "FileSystem",
                ["BlobManagement:FileSystem:BasePath"] = BasePath,
            })
            .Build();

        context.Services.ReplaceConfiguration(configuration);

        Directory.CreateDirectory(BasePath);
        Directory.CreateDirectory(OutsidePath);
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        // 清理测试目录
        if (Directory.Exists(RootPath))
        {
            Directory.Delete(RootPath, true);
        }

        // 外层目录在所有测试宿主结束后为空时一并移除
        var parentPath = Path.GetDirectoryName(RootPath);
        if (parentPath != null &&
            Directory.Exists(parentPath) &&
            !Directory.EnumerateFileSystemEntries(parentPath).Any())
        {
            Directory.Delete(parentPath);
        }
    }
}
