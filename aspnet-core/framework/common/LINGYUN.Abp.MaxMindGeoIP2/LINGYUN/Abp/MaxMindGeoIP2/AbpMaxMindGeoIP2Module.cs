using LINGYUN.Abp.IP.Location;
using MaxMind.GeoIP2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.MaxMindGeoIP2;

/// <summary>
/// MaxMind GeoIP2 模块
/// </summary>
/// <remarks>
/// 离线库(GeoLite2-City.mmdb)受 MaxMind 许可约束, 不由本模块分发:
/// 使用方需通过 <c>MaxMindGeoIP2:DatabaseFile</c> 指定文件路径,
/// 文件不存在时使用 <see cref="EmptyGeoIP2DatabaseReader"/> 并记录警告日志
/// </remarks>
[DependsOn(typeof(AbpIPLocationModule))]
public class AbpMaxMindGeoIP2Module : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpMaxMindGeoIP2Options>(configuration.GetSection("MaxMindGeoIP2"));

        Configure<AbpIPLocationResolveOptions>(options =>
        {
            options.IPLocationResolvers.Add(new MaxMindGeoIP2IPLocationResolveContributor());
        });

        context.Services.AddSingleton<IGeoIP2DatabaseReader>((serviceProvider) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AbpMaxMindGeoIP2Options>>().Value;
            var logger = serviceProvider.GetService<ILogger<AbpMaxMindGeoIP2Module>>();

            if (options.DatabaseFileExists())
            {
                logger?.LogInformation(
                    "The MaxMind GeoIP2 database was loaded from {DatabaseFile} with {FileAccessMode}.",
                    options.DatabaseFile,
                    options.FileAccessMode);

                return new DatabaseReader(options.DatabaseFile, options.FileAccessMode);
            }

            // 未提供离线库(或文件不存在)时使用空解读器, 避免阻断业务
            logger?.LogWarning(
                "The MaxMind GeoIP2 database file was not found: {DatabaseFile}, " +
                "IP location resolution will return empty results. " +
                "Download a GeoLite2 database from MaxMind (its license applies) " +
                "and set MaxMindGeoIP2:DatabaseFile.",
                options.DatabaseFile);

            return new EmptyGeoIP2DatabaseReader();
        });
    }
}
