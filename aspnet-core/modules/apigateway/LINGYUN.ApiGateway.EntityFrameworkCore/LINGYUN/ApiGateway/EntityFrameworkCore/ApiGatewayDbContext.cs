using LINGYUN.ApiGateway.Ocelot;
using LINGYUN.ApiGateway.Settings;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class ApiGatewayDbContext : AbpDbContext<ApiGatewayDbContext>
    {

        public virtual DbSet<ReRoute> ReRoutes { get; set; }

        public virtual DbSet<GlobalConfiguration> GlobalConfigurations { get; set; }

        public virtual DbSet<DynamicReRoute> DynamicReRoutes { get; set; }

        public virtual DbSet<AggregateReRoute> AggregateReRoutes { get; set; }

        public virtual DbSet<AggregateReRouteConfig> AggregateReRouteConfigs { get; set; }

        public virtual DbSet<AuthenticationOptions> AuthenticationOptions { get; set; }
        public virtual DbSet<CacheOptions> CacheOptions { get; set; }
        public virtual DbSet<Headers> Headers { get; set; }
        public virtual DbSet<HostAndPort> HostAndPorts { get; set; }
        public virtual DbSet<HttpHandlerOptions> HttpHandlerOptions { get; set; }
        public virtual DbSet<LoadBalancerOptions> LoadBalancerOptions { get; set; }
        public virtual DbSet<QoSOptions> QoSOptions { get; set; }
        public virtual DbSet<RateLimitOptions> RateLimitOptions { get; set; }
        public virtual DbSet<RateLimitRule> RateLimitRules { get; set; }
        public virtual DbSet<SecurityOptions> SecurityOptions { get; set; }
        public virtual DbSet<ServiceDiscoveryProvider> ServiceDiscoveryProviders { get; set; }

        public ApiGatewayDbContext(DbContextOptions<ApiGatewayDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureApiGateway(options =>
            {
                options.TablePrefix = ApiGatewaySettingNames.DefaultDbTablePrefix;
                options.Schema = ApiGatewaySettingNames.DefaultDbSchema;
            });
        }
    }
}
