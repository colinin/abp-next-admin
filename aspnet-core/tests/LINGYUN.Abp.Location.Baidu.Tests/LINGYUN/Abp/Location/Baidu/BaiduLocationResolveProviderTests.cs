using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Location.Baidu
{
    public class BaiduLocationResolveProviderTests : AbpLocationBaiduTestBase
    {
        private readonly ILocationResolveProvider _provider;

        public BaiduLocationResolveProviderTests()
        {
            _provider = GetRequiredService<ILocationResolveProvider>();
            _provider.ShouldBeOfType<BaiduLocationResolveProvider>();
        }

        [Theory]
        [InlineData(39.906049, 116.398773, 1000)]
        public async Task ReGeocode_Test(double lat, double lng, int radius = 50)
        {
            var location = await _provider.ReGeocodeAsync(lat, lng, radius);

            location.ShouldNotBeNull();
            location.Address.ShouldBe("北京市西城区煤市街与前门西后河沿街交叉路口往西北约50米");
            location.FormattedAddress.ShouldBe("西城区正阳市场");

            // TODO
        }

        [Theory]
        [InlineData("北京市东城区广场东侧路", "北京", 39.9102810304917, 116.40588509187855, "道路")]
        [InlineData("天安门-城楼检票处(入口)", "北京", 39.91573869262374, 116.40366250438577, "NoClass")]
        [InlineData("国家图书馆(古籍馆)", "北京", 39.92968852456012, 116.39170686652291, "教育")]
        public async Task Geocode_Test(string address, string city, double lat, double lng, string level)
        {
            var location = await _provider.GeocodeAsync(address, city);

            location.ShouldNotBeNull();
            location.Latitude.ShouldBe(lat);
            location.Longitude.ShouldBe(lng);
            location.Level.ShouldBe(level);
            location.Pomprehension.ShouldBe(0);

            // TODO
        }

        [Theory]
        [InlineData("111.206.145.41")]
        public async Task IPGeocode_Test(string ipAddress)
        {
            var location = await _provider.IPGeocodeAsync(ipAddress);

            location.ShouldNotBeNull();
            location.Location.Latitude.ShouldBe(39.91092300415039);
            location.Location.Longitude.ShouldBe(116.41338348388672);

            // TODO
        }
    }
}
