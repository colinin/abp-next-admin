using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Location.Amap
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(ILocationResolveProvider))]
    public class AmapLocationResolveProvider : ILocationResolveProvider
    {
        protected AmapHttpRequestClient AmapHttpRequestClient { get; }

        public AmapLocationResolveProvider(AmapHttpRequestClient amapHttpRequestClient)
        {
            AmapHttpRequestClient = amapHttpRequestClient;
        }

        public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
        {
            var inverseRequestPramter = new AmapInverseHttpRequestParamter();
            inverseRequestPramter.Locations = new Location[1]
            {
                new Location
                {
                    Latitude = lat,
                    Longitude = lng
                }
            };
            return await AmapHttpRequestClient.InverseAsync(inverseRequestPramter);
        }

        public async Task<GecodeLocation> GeocodeAsync(string address, string city)
        {
            var positiceRequestParamter = new AmapPositiveHttpRequestParamter
            {
                Address = address
            };
            return await AmapHttpRequestClient.PositiveAsync(positiceRequestParamter);
        }
    }
}
