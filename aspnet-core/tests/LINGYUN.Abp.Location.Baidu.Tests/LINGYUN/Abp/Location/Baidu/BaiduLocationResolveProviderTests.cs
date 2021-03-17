using Shouldly;
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
            location.Address.ShouldBe("北京市西城区前门西大街正阳市场4号楼");
            location.FormattedAddress.ShouldBe("前门西大街正阳市场4号楼");

            // TODO
        }

        [Theory]
        [InlineData("北京市东城区广场东侧路", "北京")]
        public async Task Geocode_Test(string address, string city = "")
        {
            var location = await _provider.GeocodeAsync(address, city);

            location.ShouldNotBeNull();
            location.Latitude.ShouldBe(39.91125781161926);
            location.Longitude.ShouldBe(116.40588581321788);
            location.Level.ShouldBe("道路");
            location.Pomprehension.ShouldBe(0);

            // TODO
        }

        [Theory]
        [InlineData("111.206.145.41")]
        public async Task IPGeocode_Test(string ipAddress)
        {
            var location = await _provider.IPGeocodeAsync(ipAddress);

            location.ShouldNotBeNull();
            location.Location.Latitude.ShouldBe(39.91489028930664);
            location.Location.Longitude.ShouldBe(116.40387725830078);

            // TODO
        }
    }
}
