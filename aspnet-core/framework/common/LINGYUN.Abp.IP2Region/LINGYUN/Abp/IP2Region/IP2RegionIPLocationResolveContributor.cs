using IP2Region.Net.Abstractions;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP2Region;
public class IP2RegionIPLocationResolveContributor : IPLocationResolveContributorBase
{
    public const string ContributorName = "IP2Region";
    public override string Name => ContributorName;

    public override Task ResolveAsync(IIPLocationResolveContext context)
    {
        // 非法地址、本地回环与未指定地址由 IIPLocationResolver 统一处理, 此处不再判断
        var searcher = context.ServiceProvider.GetRequiredService<ISearcher>();
        var region = searcher.Search(context.IpAddress);

        if (string.IsNullOrWhiteSpace(region))
        {
            return Task.CompletedTask;
        }

        var regions = region!.Split('|');
        // |    0    |      1      |  2 |  3   |   4  | 5 | 6 |
        // 39.128.0.0|39.128.31.255|中国|云南省|昆明市|移动|CN
        // regions:
        // 中国|云南省|昆明市|移动|CN

        // 内网、链路本地、组播、保留地址等特殊地址段(离线库返回 Reserved|Reserved|Reserved|0|0):
        // 不返回国家/省份/城市, Remarks 为空
        if (string.Equals(regions[0], "Reserved", StringComparison.OrdinalIgnoreCase))
        {
            context.Location = new IPLocation();

            return Task.CompletedTask;
        }

        context.Location = CreateIPLocation(
            context,
            regions.Length >= 1 && !string.Equals(regions[0], "0") ? regions[0] : null,
            regions.Length >= 2 && !string.Equals(regions[1], "0") ? regions[1] : null,
            regions.Length >= 3 && !string.Equals(regions[2], "0") ? regions[2] : null);

        return Task.CompletedTask;
    }
}
