using IP2Region.Net.Abstractions;
using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IP2Region;

public class IP2RegionLocationInfoProvider : IIpLocationInfoProvider
{
    protected static readonly LocationInfo? _nullCache = null;

    protected ISearcher Searcher { get; }
    public IP2RegionLocationInfoProvider(ISearcher searcher)
    {
        Searcher = searcher;
    }

    public virtual Task<LocationInfo?> GetLocationInfoAsync(string ipAddress)
    {
        var region = Searcher.Search(ipAddress);
        if (string.IsNullOrWhiteSpace(region))
        {
            return Task.FromResult(_nullCache);
        }

        var regions = region!.Split('|');
        // |    0    |      1      |  2 |3|  4   |   5  | 6 |
        // 39.128.0.0|39.128.31.255|中国|0|云南省|昆明市|移动
        // regions:
        // 中国 0 云南省 昆明市 移动
        var locationInfo = new LocationInfo
        {
            Country = regions.Length >= 1 && !string.Equals(regions[0], "0") ? regions[0] : null,
            Province = regions.Length >= 3 && !string.Equals(regions[2], "0") ? regions[2] : null,
            City = regions.Length >= 4 && !string.Equals(regions[3], "0") ? regions[3] : null,
        };

        // 36.133.108.0|36.133.119.255|中国|0|重庆|重庆市|移动
        if (!locationInfo.Province.IsNullOrWhiteSpace() && !locationInfo.City.IsNullOrWhiteSpace())
        {
            if (locationInfo.Province.Length < locationInfo.City.Length &&
                locationInfo.City.StartsWith(locationInfo.Province, StringComparison.InvariantCultureIgnoreCase))
            {
                // 重庆市
                locationInfo.Remarks = $"{locationInfo.City}";
            }
            // 111.26.31.0|111.26.31.127|中国|0|吉林省|吉林市|移动
            else
            {
                // 吉林省吉林市
                locationInfo.Remarks = $"{locationInfo.Province}{locationInfo.City}";
            }
        }
        // 220.246.0.0|220.246.255.255|中国|0|香港|0|电讯盈科
        else if (!locationInfo.Country.IsNullOrWhiteSpace() && !locationInfo.Province.IsNullOrWhiteSpace())
        {
            // 中国香港
            locationInfo.Remarks = $"{locationInfo.Country}{locationInfo.Province}";
        }
        // 220.247.4.0|220.247.31.255|日本|0|0|0|0
        else
        {
            // 日本
            locationInfo.Remarks = $"{locationInfo.Country}";
        }

        return Task.FromResult<LocationInfo?>(locationInfo);
    }
}
