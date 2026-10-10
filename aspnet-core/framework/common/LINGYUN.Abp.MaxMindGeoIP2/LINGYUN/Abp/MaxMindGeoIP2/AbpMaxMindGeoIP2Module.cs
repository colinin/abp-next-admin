using LINGYUN.Abp.IP.Location;
using MaxMind.GeoIP2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.MaxMindGeoIP2;

[DependsOn(typeof(AbpIPLocationModule))]
[DependsOn(typeof(AbpVirtualFileSystemModule))]
public class AbpMaxMindGeoIP2Module : AbpModule
{
    /// <summary>
    /// 内置的 GeoLite2-City 离线库(虚拟文件系统路径)
    /// </summary>
    public const string DefaultDatabaseFile = "/LINGYUN/Abp/MaxMindGeoIP2/Resources/GeoLite2-City.mmdb";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpMaxMindGeoIP2Options>(configuration.GetSection("MaxMindGeoIP2"));

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpMaxMindGeoIP2Module>();
        });

        Configure<AbpIPLocationResolveOptions>(options =>
        {
            options.IPLocationResolvers.Add(new MaxMindGeoIP2IPLocationResolveContributor());
        });

        context.Services.AddSingleton<IGeoIP2DatabaseReader>((serviceProvider) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpMaxMindGeoIP2Options>>().Value;

            // 地理名称按当前文化从 Names 中获取(见 MaxMindGeoIP2IPLocationResolveContributor),
            // 因此这里无需指定 locales, 同一个查询器可为所有文化提供服务。
            // 物理文件优先(便于替换/更新离线的 mmdb 文件, 可按 FileAccessMode 打开)
            if (File.Exists(options.DatabaseFile))
            {
                return new DatabaseReader(options.DatabaseFile, options.FileAccessMode);
            }

            // 否则从虚拟文件系统加载内置资源(始终加载到内存)
            var virtualFileProvider = serviceProvider.GetRequiredService<IVirtualFileProvider>();
            var databaseFile = virtualFileProvider.GetFileInfo(options.DatabaseFile);
            if (!databaseFile.Exists)
            {
                throw new AbpException(
                    $"MaxMind GeoIP2 database file not found: {options.DatabaseFile}. " +
                    "Please check the MaxMindGeoIP2:DatabaseFile option or the embedded GeoLite2-City.mmdb resource.");
            }

            return new DatabaseReader(databaseFile.CreateReadStream());
        });
    }
}
