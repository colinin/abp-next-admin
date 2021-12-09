using LINGYUN.MicroService.Internal.ApiGateway.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Ocelot.LoadBalancer.LoadBalancers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.MicroService.Internal.ApiGateway.Utils
{
    public class LoadBalancerFinder : ILoadBalancerFinder, ISingletonDependency
    {
        private Lazy<List<LoadBalancerDescriptor>> lazyLoadBalancers;
        protected List<LoadBalancerDescriptor> LoadBalancers => lazyLoadBalancers.Value;
        protected IServiceProvider ServiceProvider { get; }
        protected IStringLocalizer Localizer { get; }
        public LoadBalancerFinder(
            IServiceProvider serviceProvider,
            IStringLocalizer<ApiGatewayResource> localizer)
        {
            Localizer = localizer;
            ServiceProvider = serviceProvider;

            lazyLoadBalancers = new Lazy<List<LoadBalancerDescriptor>>(() => FindLocalLoadBalancers());
        }

        public Task<List<LoadBalancerDescriptor>> GetLoadBalancersAsync()
        {
            return Task.FromResult(LoadBalancers);
        }

        protected List<LoadBalancerDescriptor> FindLocalLoadBalancers()
        {
            List<LoadBalancerDescriptor> loadBalancers = new List<LoadBalancerDescriptor>
            {
                new LoadBalancerDescriptor(typeof(NoLoadBalancer).Name, Localizer["NoLoadBalancer"]),
                new LoadBalancerDescriptor(typeof(RoundRobin).Name, Localizer["RoundRobin"]),
                new LoadBalancerDescriptor(typeof(LeastConnection).Name, Localizer["LeastConnection"]),
                new LoadBalancerDescriptor(typeof(CookieStickySessions).Name, Localizer["CookieStickySessions"])
            };


            var loadBalancerCreators = ServiceProvider.GetServices<ILoadBalancerCreator>();
            loadBalancerCreators = loadBalancerCreators
                .Where(lbc => !loadBalancers.Any(l => l.Type.Equals(lbc.Type)))
                .ToArray();

            foreach(var defintedLoadBalancerCreator in loadBalancerCreators)
            {
                loadBalancers.Add(new LoadBalancerDescriptor(defintedLoadBalancerCreator.Type, Localizer["CustomLoadBalancer", defintedLoadBalancerCreator.Type]));
            }

            return loadBalancers;
        }
    }
}
