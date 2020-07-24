using LINGYUN.Abp.Location;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

#if DEBUG
namespace LINGYUN.BackendAdmin.Controllers
{
    [Route("Location")]
    public class LocationController : AbpController
    {
        protected ILocationResolveProvider LocationResolveProvider { get; }

        public LocationController(
            ILocationResolveProvider locationResolveProvider)
        {
            LocationResolveProvider = locationResolveProvider;
        }

        [HttpGet]
        [Route("IpGeocode")]
        public async Task<IPGecodeLocation> IPGeocodeAsync(string ipAddress)
        {
            var location = await LocationResolveProvider.IPGeocodeAsync(ipAddress);
            return location;
        }

        [HttpGet]
        [Route("Geocode")]
        public async Task<GecodeLocation> GeocodeAsync(string address)
        {
            var location = await LocationResolveProvider.GeocodeAsync(address);
            return location;
        }

        [HttpGet]
        [Route("ReGeocode")]

        public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng, int redius = 50)
        {
            var location = await LocationResolveProvider.ReGeocodeAsync(lat, lng);
            return location;
        }
    }
}
#endif