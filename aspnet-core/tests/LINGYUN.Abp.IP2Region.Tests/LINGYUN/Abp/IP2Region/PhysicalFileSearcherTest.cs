using IP2Region.Net.Abstractions;
using LINGYUN.Abp.IP.Location;
using Microsoft.Extensions.Options;
using Shouldly;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.IP2Region;
public class PhysicalFileSearcherTest : AbpIP2RegionPhysicalFileTestBase
{
    /// <summary>
    /// 离线库配置为物理文件(优先于内置资源)
    /// </summary>
    [Fact]
    public void TestOptionsUsePhysicalFiles()
    {
        var options = GetRequiredService<IOptions<AbpIP2RegionOptions>>().Value;

        options.IPv4DatabaseFile.ShouldNotBe(AbpIP2RegionModule.DefaultIPv4DatabaseFile);
        options.IPv6DatabaseFile.ShouldNotBe(AbpIP2RegionModule.DefaultIPv6DatabaseFile);
        File.Exists(options.IPv4DatabaseFile).ShouldBeTrue();
        File.Exists(options.IPv6DatabaseFile).ShouldBeTrue();
    }

    /// <summary>
    /// 从物理文件加载的 IPv4 / IPv6 离线库均可正常查询
    /// </summary>
    [Theory]
    [InlineData("114.114.114.114", "中国|江苏省|南京市|0|CN")]
    [InlineData("8.8.8.8", "United States|California|0|Google LLC|US")]
    [InlineData("2001:4860:4860::8888", "United States|Florida|Miami|Google LLC|US")]
    [InlineData("2400:3200::1", "中国|浙江省|杭州市|阿里|CN")]
    public void TestSearchFromPhysicalFile(string ip, string shouldBeRegion)
    {
        var searcher = GetRequiredService<ISearcher>();

        searcher.Search(ip).ShouldBe(shouldBeRegion);
    }

    /// <summary>
    /// 位置解析使用默认备注规则(国家+省份)
    /// </summary>
    [Fact]
    public async Task TestResolveLocationFromPhysicalFile()
    {
        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync("223.5.5.5");

        result.Location.ShouldNotBeNull();
        result.Location.Country.ShouldBe("中国");
        result.Location.Province.ShouldBe("浙江省");
        result.Location.City.ShouldBe("杭州市");
        result.Location.Remarks.ShouldBe("中国浙江省");
    }
}
