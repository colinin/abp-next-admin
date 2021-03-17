using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace LINGYUN.Abp.Location.Tencent
{
    public class TencentLocationResolveProviderTests : AbpLocationTencentTestBase
    {
        private readonly ILocationResolveProvider _provider;

        public TencentLocationResolveProviderTests()
        {
            _provider = GetRequiredService<ILocationResolveProvider>();
            _provider.ShouldBeOfType<TencentLocationResolveProvider>();
        }

        [Theory]
        [InlineData(39.906049, 116.398773, 1000)]
        public async Task ReGeocode_Test(double lat, double lng, int radius = 50)
        {
            var location = await _provider.ReGeocodeAsync(lat, lng, radius);

            location.ShouldNotBeNull();
            location.Address.ShouldBe("北京市东城区东长安街");
            location.FormattedAddress.ShouldBe("天安门广场");

            // TODO
        }

        [Theory]
        [InlineData("北京市东城区广场东侧路")]
        public async Task Geocode_Test(string address, string city = "")
        {
            var location = await _provider.GeocodeAsync(address, city);

            location.ShouldNotBeNull();
            location.Latitude.ShouldBe(39.907631);
            location.Longitude.ShouldBe(116.399384);
            location.Level.ShouldBe("7");
            location.Pomprehension.ShouldBe(0);

            // TODO
        }

        [Theory]
        [InlineData("111.206.145.41")]
        public async Task IPGeocode_Test(string ipAddress)
        {
            var location = await _provider.IPGeocodeAsync(ipAddress);

            location.ShouldNotBeNull();
            location.Location.Latitude.ShouldBe(40.0403);
            location.Location.Longitude.ShouldBe(116.2734);

            // TODO
        }
    }
}
