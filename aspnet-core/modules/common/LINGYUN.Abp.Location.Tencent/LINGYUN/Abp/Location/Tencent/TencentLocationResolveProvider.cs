using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Location.Tencent
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(ILocationResolveProvider))]
    public class TencentLocationResolveProvider : ILocationResolveProvider
    {
        protected TencentLocationHttpClient TencentLocationHttpClient { get; }

        public TencentLocationResolveProvider(TencentLocationHttpClient tencentLocationHttpClient)
        {
            TencentLocationHttpClient = tencentLocationHttpClient;
        }

        public virtual async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
        {
            return await TencentLocationHttpClient.IPGeocodeAsync(ipAddress);
        }

        public virtual async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
        {
            return await TencentLocationHttpClient.ReGeocodeAsync(lat, lng, radius);
        }

        public virtual async Task<GecodeLocation> GeocodeAsync(string address, string city = null)
        {
            return await TencentLocationHttpClient.GeocodeAsync(address, city);
        }
    }
}
