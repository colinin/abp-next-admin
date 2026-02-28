using IP2Region.Net.XDB;
using LINGYUN.Abp.IP.Location;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace LINGYUN.Abp.IP2Region;
public class SearcherTest : AbpIP2RegionTestBase
{
    private readonly Stream _xdbStream;

    public SearcherTest()
    {
        var virtualFileProvider = GetRequiredService<IVirtualFileProvider>();
        _xdbStream = virtualFileProvider.GetFileInfo("/LINGYUN/Abp/IP2Region/Resources/ip2region_v4.xdb").CreateReadStream();
    }

    [Theory]
    [InlineData("8.8.8.8", "美国")]
    [InlineData("36.133.108.1", "重庆市")]
    [InlineData("111.26.31.1", "吉林省吉林市")]
    [InlineData("220.246.0.1", "中国香港")]
    [InlineData("103.151.173.211", "日本")]
    [InlineData("103.151.191.5", "印度尼西亚")]
    public async Task TestSearchLocation(string ip, string shouldBeRemarks)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();
        var result = await resolver.ResolveAsync(ip);
        result.Location.Remarks.ShouldBe(shouldBeRemarks);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0")]
    [InlineData("119.29.29.29", "中国|北京|北京市|腾讯")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里")]
    [InlineData("180.76.76.76", "中国|北京|北京市|百度")]
    [InlineData("8.8.8.8", "美国|0|0|谷歌")]
    public void TestSearchCacheContent(string ip, string shouldBeRegion)
    {
        var contentSearcher = new AbpSearcher(CachePolicy.Content, _xdbStream);
        var region = contentSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0")]
    [InlineData("119.29.29.29", "中国|北京|北京市|腾讯")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里")]
    [InlineData("180.76.76.76", "中国|北京|北京市|百度")]
    [InlineData("8.8.8.8", "美国|0|0|谷歌")]
    public void TestSearchCacheVector(string ip, string shouldBeRegion)
    {
        var vectorSearcher = new AbpSearcher(CachePolicy.VectorIndex, _xdbStream);
        var region = vectorSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0")]
    [InlineData("119.29.29.29", "中国|北京|北京市|腾讯")]
    [InlineData("223.5.5.5", "中国|浙江省|杭州市|阿里")]
    [InlineData("180.76.76.76", "中国|北京|北京市|百度")]
    [InlineData("8.8.8.8", "美国|0|0|谷歌")]
    public void TestSearchCacheFile(string ip, string shouldBeRegion)
    {
        var fileSearcher = new AbpSearcher(CachePolicy.File, _xdbStream);
        var region = fileSearcher.Search(ip);
        region.ShouldBe(shouldBeRegion);
    }

    [Theory]
    [InlineData(CachePolicy.Content)]
    [InlineData(CachePolicy.VectorIndex)]
    [InlineData(CachePolicy.File)]
    public void TestBenchSearch(CachePolicy cachePolicy)
    {
        var searcher = new AbpSearcher(cachePolicy, _xdbStream);
        var srcPath = Path.Combine(AppContext.BaseDirectory, "ipv4_source.txt");

        foreach (var line in File.ReadLines(srcPath))
        {
            var ps = line.Trim().Split("|", 3);

            if (ps.Length != 3)
            {
                throw new ArgumentException($"invalid ip segment line {line}", nameof(line));
            }

            var sip = Util.IpAddressToUInt32(ps[0]);
            var eip = Util.IpAddressToUInt32(ps[1]);
            var mip = Util.GetMidIp(sip, eip);

            uint[] temp = { sip, Util.GetMidIp(sip, mip), mip, Util.GetMidIp(mip, eip), eip };

            foreach (var ip in temp)
            {
                var region = searcher.Search(ip);

                if (region != ps[2])
                {
                    throw new Exception($"failed search {ip} with ({region}!={ps[2]})");
                }
            }
        }
    }
}
