using LINGYUN.Abp.IP.Location;
using MaxMind.Db;
using MaxMind.GeoIP2;
using Microsoft.Extensions.Options;
using Shouldly;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.MaxMindGeoIP2;
public class IPLocationResolverTest : AbpMaxMindGeoIP2TestBase
{
    /// <summary>
    /// 默认选项: 使用内置 GeoLite2-City.mmdb、内存映射模式
    /// </summary>
    [Fact]
    public void TestDefaultOptions()
    {
        var options = GetRequiredService<IOptions<AbpMaxMindGeoIP2Options>>().Value;

        options.DatabaseFile.ShouldBe(AbpMaxMindGeoIP2Module.DefaultDatabaseFile);
        options.FileAccessMode.ShouldBe(FileAccessMode.MemoryMapped);
    }

    /// <summary>
    /// 离线库应正确加载(GeoLite2-City), GeoIP2 的 Name 为离线库默认名称, 本地化名称在 Names 中
    /// </summary>
    [Fact]
    public void TestDatabaseReader()
    {
        var reader = GetRequiredService<IGeoIP2DatabaseReader>();

        reader.Metadata.DatabaseType.ShouldBe("GeoLite2-City");

        reader.TryCity(IPAddress.Parse("223.5.5.5"), out var response).ShouldBeTrue();
        response!.Country.Name.ShouldBe("China");
        response.Country.Names["zh-CN"].ShouldBe("中国");
        response.MostSpecificSubdivision.Name.ShouldBe("Zhejiang");
        response.MostSpecificSubdivision.Names["zh-CN"].ShouldBe("浙江");
        response.City.Name.ShouldBe("Hangzhou");
        response.City.Names["zh-CN"].ShouldBe("杭州");

        reader.TryCity(IPAddress.Parse("2400:3200::1"), out var ipv6Response).ShouldBeTrue();
        ipv6Response!.Country.IsoCode.ShouldBe("CN");
    }

    /// <summary>
    /// 通过 IIPLocationResolver 解析位置(当前文化为简体中文时的备注, 规则与 IP2Region 模块一致)
    /// </summary>
    [Theory]
    [InlineData("8.8.8.8", "美国")]
    [InlineData("2001:4860:4860::8888", "美国")]
    [InlineData("223.5.5.5", "浙江杭州")]
    [InlineData("2400:3200::1", "浙江杭州")]
    [InlineData("114.114.114.114", "中国")]
    [InlineData("180.76.76.76", "中国")]
    [InlineData("103.151.173.211", "日本")]
    [InlineData("2a00:1450:4001:81b::200e", "德国")]
    [InlineData("220.246.0.1", "香港")]
    public async Task TestSearchLocation(string ip, string shouldBeRemarks)
    {
        UseCulture("zh-CN");

        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldNotBeNull();
        result.Location.Remarks.ShouldBe(shouldBeRemarks);
        result.AppliedResolvers.ShouldContain(MaxMindGeoIP2IPLocationResolveContributor.ContributorName);
    }

    /// <summary>
    /// 地理名称按当前文化从 Names 中获取, 完整文化不存在时按语言获取, 都没有时回退到 Name
    /// </summary>
    [Theory]
    // 完整文化名称(zh-CN / en / ja 等)
    [InlineData("zh-CN", "223.5.5.5", "浙江杭州")]
    [InlineData("en-US", "223.5.5.5", "China")]
    [InlineData("ja-JP", "2a00:1450:4001:81b::200e", "ドイツ連邦共和国")]
    // 完整文化不存在(es-MX), 按语言名称(es)获取
    [InlineData("es-MX", "8.8.8.8", "Estados Unidos")]
    // 离线库没有该语言(ko), 回退到 Name(离线库默认语言)
    [InlineData("ko-KR", "8.8.8.8", "United States")]
    [InlineData("ko-KR", "223.5.5.5", "China")]
    public async Task TestSearchLocationByCulture(string culture, string ip, string shouldBeRemarks)
    {
        UseCulture(culture);

        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldNotBeNull();
        result.Location.Remarks.ShouldBe(shouldBeRemarks);
    }

    /// <summary>
    /// 内网、回环、保留地址以及离线库未收录的地址: 国家/省份/城市/备注均为空
    /// </summary>
    [Theory]
    [InlineData("10.0.0.1")]
    [InlineData("172.16.0.1")]
    [InlineData("192.168.1.1")]
    [InlineData("169.254.1.1")]
    [InlineData("224.0.0.1")]
    [InlineData("fe80::1")]
    [InlineData("1.1.1.1")]
    public async Task TestSearchSpecialLocation(string ip)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldNotBeNull();
        result.Location.Remarks.ShouldBeNullOrEmpty();
        result.Location.Country.ShouldBeNull();
        result.Location.Province.ShouldBeNull();
        result.Location.City.ShouldBeNull();
    }

    /// <summary>
    /// 非法地址、本地回环与未指定地址由 IIPLocationResolver 统一处理, 不返回位置
    /// </summary>
    [Theory]
    [InlineData("not-an-ip")]
    [InlineData("999.999.999.999")]
    [InlineData("127.0.0.1")]
    [InlineData("::1")]
    [InlineData("0.0.0.0")]
    [InlineData("::")]
    public async Task TestSearchInvalidLocation(string ip)
    {
        var resolver = GetRequiredService<IIPLocationResolver>();

        var result = await resolver.ResolveAsync(ip);

        result.Location.ShouldBeNull();
    }

    /// <summary>
    /// 查询器为单例且线程安全, 并发查询应返回正确结果
    /// </summary>
    [Fact]
    public async Task TestSearchConcurrently()
    {
        UseCulture("zh-CN");

        var resolver = GetRequiredService<IIPLocationResolver>();

        var expected = new (string Ip, string Remarks)[]
        {
            ("8.8.8.8", "美国"),
            ("223.5.5.5", "浙江杭州"),
            ("220.246.0.1", "香港"),
            ("2001:4860:4860::8888", "美国"),
            ("2400:3200::1", "浙江杭州"),
        };

        var tasks = Enumerable.Range(0, System.Environment.ProcessorCount * 4)
            .Select(_ => Task.Run(async () =>
            {
                for (var i = 0; i < 20; i++)
                {
                    foreach (var (ip, remarks) in expected)
                    {
                        var result = await resolver.ResolveAsync(ip);
                        result.Location.Remarks.ShouldBe(remarks);
                    }
                }
            }));

        await Task.WhenAll(tasks);
    }

    private static void UseCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}
