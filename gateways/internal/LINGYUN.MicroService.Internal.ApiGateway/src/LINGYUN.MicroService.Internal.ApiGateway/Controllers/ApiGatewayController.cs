using LINGYUN.MicroService.Internal.ApiGateway.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Multiplexer;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.MicroService.Internal.ApiGateway.Controllers
{
    [Area("ApiGateway")]
    [Route("api/ApiGateway/Basic")]
    public class ApiGatewayController : Controller
    {
        protected IServiceProvider ServiceProvider { get; }
        protected ILoadBalancerFinder LoadBalancerFinder { get; }
        public ApiGatewayController(
            IServiceProvider serviceProvider,
            ILoadBalancerFinder loadBalancerFinder)
        {
            ServiceProvider = serviceProvider;
            LoadBalancerFinder = loadBalancerFinder;
        }

        [HttpGet]
        [Route("Aggregators")]
        public Task<JsonResult> GetAggregatorsAsync()
        {
            var aggregators = ServiceProvider.GetServices<IDefinedAggregator>();

            var aggregatorDtos = new ListResultDto<string>(aggregators.Select(agg => agg.GetType().Name).ToList());

            return Task.FromResult(Json(aggregatorDtos));
        }

        [HttpGet]
        [Route("LoadBalancers")]
        public async Task<JsonResult> GetLoadBalancersAsync()
        {
            var loadBalancers = await LoadBalancerFinder.GetLoadBalancersAsync();

            var loadBalancerDtos = new ListResultDto<LoadBalancerDescriptor>(loadBalancers);

            return Json(loadBalancerDtos);
        }
    }
}
