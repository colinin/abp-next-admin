using LINGYUN.Abp.Location;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

#if DEBUG
namespace LINGYUN.Platform.Controllers
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
        [Route("Geocode")]
        public async Task<GecodeLocation> GeocodeAsync(string address)
        {
            var location = await LocationResolveProvider.GeocodeAsync(address);
            return location;
        }

        [HttpGet]
        [Route("ReGeocode")]

        public async Task<ReGeocodeLocation> ReGeocodeAsync(double lat, double lng)
        {
            var location = await LocationResolveProvider.ReGeocodeAsync(lat, lng);
            return location;
        }
    }
}
#endif