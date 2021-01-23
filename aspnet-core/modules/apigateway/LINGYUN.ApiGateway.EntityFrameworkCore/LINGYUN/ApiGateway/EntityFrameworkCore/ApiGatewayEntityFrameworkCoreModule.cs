using LINGYUN.ApiGateway.Data.Filter;
using LINGYUN.ApiGateway.Ocelot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    [DependsOn(
        typeof(ApiGatewayDomainModule)
        )]
    public class ApiGatewayEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDataFilterOptions>(options =>
            {
                options.DefaultStates.Add(typeof(IActivation), new DataFilterState(true));
            });

            context.Services.AddAbpDbContext<ApiGatewayDbContext>(options =>
            {
                //Remove "includeAllEntities: true" to create default repositories only for aggregate roots
                options.AddRepository<RouteGroup, EfCoreRouteGroupRepository>();
                options.AddRepository<ReRoute, EfCoreReRouteRepository>();
                options.AddRepository<GlobalConfiguration, EfCoreGlobalConfigRepository>();
                options.AddRepository<DynamicReRoute, EfCoreDynamicReRouteRepository>();
                options.AddRepository<AggregateReRoute, EfCoreAggregateReRouteRepository>();

                options.Entity<ReRoute>(opt =>
                    opt.DefaultWithDetailsFunc = (p) => 
                        p.Include(q => q.AuthenticationOptions)
                         .Include(q => q.CacheOptions)
                         .Include(q => q.HttpHandlerOptions)
                         .Include(q => q.LoadBalancerOptions)
                         .Include(q => q.QoSOptions)
                         .Include(q => q.RateLimitOptions)
                         .Include(q => q.SecurityOptions));

                options.Entity<GlobalConfiguration>(opt =>
                    opt.DefaultWithDetailsFunc = (p) =>
                        p.Include(q => q.HttpHandlerOptions)
                         .Include(q => q.LoadBalancerOptions)
                         .Include(q => q.QoSOptions)
                         .Include(q => q.RateLimitOptions)
                         .Include(q => q.ServiceDiscoveryProvider));

                options.Entity<DynamicReRoute>(opt =>
                    opt.DefaultWithDetailsFunc = (p) =>
                        p.Include(q => q.RateLimitRule));

                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
