using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Location.Baidu
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(ILocationResolveProvider))]
    public class BaiduLocationResolveProvider : ILocationResolveProvider
    {
        protected BaiduLocationHttpClient BaiduLocationHttpClient { get; }

        public BaiduLocationResolveProvider(BaiduLocationHttpClient baiduHttpRequestClient)
        {
            BaiduLocationHttpClient = baiduHttpRequestClient;
        }

        public virtual Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
        {
            return Task.FromResult(new IPGecodeLocation());
        }

        public virtual async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int radius = 50)
        {
            return await BaiduLocationHttpClient.ReGeocodeAsync(lat, lng, radius);
        }

        public virtual async Task<GecodeLocation> GeocodeAsync(string address, string city = null)
        {
            return await BaiduLocationHttpClient.GeocodeAsync(address, city);
        }
    }
}
