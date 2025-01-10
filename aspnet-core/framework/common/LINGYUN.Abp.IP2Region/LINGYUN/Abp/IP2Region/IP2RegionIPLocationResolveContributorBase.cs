using IP2Region.Net.Abstractions;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP2Region;
public class IP2RegionIPLocationResolveContributorBase : IPLocationResolveContributorBase
{
    public const string ContributorName = "IP2Region";
    public override string Name => ContributorName;

    public override Task ResolveAsync(IIPLocationResolveContext context)
    {
        var searcher = context.ServiceProvider.GetRequiredService<ISearcher>();

        var region = searcher.Search(context.IpAddress);

        if (string.IsNullOrWhiteSpace(region))
        {
            return Task.CompletedTask;
        }

        var regions = region!.Split('|');
        // |    0    |      1      |  2 |3|  4   |   5  | 6 |
        // 39.128.0.0|39.128.31.255|中国|0|云南省|昆明市|移动
        // regions:
        // 中国 0 云南省 昆明市 移动

        var ipLocation = new IPLocation(
            regions.Length >= 1 && !string.Equals(regions[0], "0") ? regions[0] : null,
            regions.Length >= 3 && !string.Equals(regions[2], "0") ? regions[2] : null,
            regions.Length >= 4 && !string.Equals(regions[3], "0") ? regions[3] : null);

        // 36.133.108.0|36.133.119.255|中国|0|重庆|重庆市|移动
        if (!ipLocation.Province.IsNullOrWhiteSpace() && !ipLocation.City.IsNullOrWhiteSpace())
        {
            if (ipLocation.Province.Length < ipLocation.City.Length &&
                ipLocation.City.StartsWith(ipLocation.Province, StringComparison.InvariantCultureIgnoreCase))
            {
                // 重庆市
                ipLocation.Remarks = $"{ipLocation.City}";
            }
            // 111.26.31.0|111.26.31.127|中国|0|吉林省|吉林市|移动
            else
            {
                // 吉林省吉林市
                ipLocation.Remarks = $"{ipLocation.Province}{ipLocation.City}";
            }
        }
        // 220.246.0.0|220.246.255.255|中国|0|香港|0|电讯盈科
        else if (!ipLocation.Country.IsNullOrWhiteSpace() && !ipLocation.Province.IsNullOrWhiteSpace())
        {
            // 中国香港
            ipLocation.Remarks = $"{ipLocation.Country}{ipLocation.Province}";
        }
        // 220.247.4.0|220.247.31.255|日本|0|0|0|0
        else
        {
            // 日本
            ipLocation.Remarks = $"{ipLocation.Country}";
        }

        context.Location = ipLocation;

        return Task.CompletedTask;
    }
}
