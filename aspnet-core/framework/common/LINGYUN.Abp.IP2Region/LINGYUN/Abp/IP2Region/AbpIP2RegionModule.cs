using IP2Region.Net.Abstractions;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IP2Region;

[DependsOn(typeof(AbpIPLocationModule))]
[DependsOn(typeof(AbpVirtualFileSystemModule))]
public class AbpIP2RegionModule : AbpModule
{
    /// <summary>
    /// 内置的 IPv4 离线库(虚拟文件系统路径)
    /// </summary>
    public const string DefaultIPv4DatabaseFile = "/LINGYUN/Abp/IP2Region/Resources/ip2region_v4.xdb";

    /// <summary>
    /// 内置的 IPv6 离线库(虚拟文件系统路径)
    /// </summary>
    public const string DefaultIPv6DatabaseFile = "/LINGYUN/Abp/IP2Region/Resources/ip2region_v6.xdb";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpIP2RegionOptions>(configuration.GetSection("IP2Region"));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIP2RegionModule>();
        });

        context.Services.AddSingleton<ISearcher>((serviceProvider) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpIP2RegionOptions>>().Value;

            // 物理文件优先, 便于替换/更新离线库; 否则使用内置资源
            var ipv4XdbStream = CreateDatabaseStream(serviceProvider, options.IPv4DatabaseFile, true);

            // IPv6 数据文件较大且可选(可通过打包时排除该资源来减小体积), 不存在时不加载, IPv6 查询返回 null
            var ipv6XdbStream = CreateDatabaseStream(serviceProvider, options.IPv6DatabaseFile, false);

            // 按地址族自动选择数据文件
            return new AbpSearcher(options.CachePolicy, ipv4XdbStream!, ipv6XdbStream);
        });

        Configure<AbpIPLocationResolveOptions>(options =>
        {
            options.IPLocationResolvers.Add(new IP2RegionIPLocationResolveContributor());
        });
    }

    /// <summary>
    /// 创建离线库数据流
    /// </summary>
    /// <remarks>
    /// 物理文件优先, 其次为虚拟文件系统路径(内置资源)
    /// </remarks>
    private static Stream? CreateDatabaseStream(IServiceProvider serviceProvider, string databaseFile, bool required)
    {
        if (File.Exists(databaseFile))
        {
            return File.OpenRead(databaseFile);
        }

        var virtualFileProvider = serviceProvider.GetRequiredService<IVirtualFileProvider>();
        var databaseFileInfo = virtualFileProvider.GetFileInfo(databaseFile);
        if (databaseFileInfo.Exists)
        {
            return databaseFileInfo.CreateReadStream();
        }

        if (required)
        {
            throw new AbpException(
                $"IP2Region database file not found: {databaseFile}. " +
                "Please check the IP2Region:IPv4DatabaseFile option or the embedded ip2region_v4.xdb resource.");
        }

        return null;
    }
}
