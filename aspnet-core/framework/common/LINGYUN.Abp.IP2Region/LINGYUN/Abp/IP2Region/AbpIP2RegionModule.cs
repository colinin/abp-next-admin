using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.IP2Region;

[DependsOn(typeof(AbpVirtualFileSystemModule))]
public class AbpIP2RegionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpIP2RegionModule>();
        });

        context.Services.AddSingleton<ISearcher, AbpSearcher>((serviceProvider) =>
        {
            var virtualFileProvider = serviceProvider.GetRequiredService<IVirtualFileProvider>();
            var xdbFile = virtualFileProvider.GetFileInfo("/LINGYUN/Abp/IP2Region/Resources/ip2region.xdb");
            var searcher = new AbpSearcher(CachePolicy.File, xdbFile.CreateReadStream());

            return searcher;
        });
    }
}
