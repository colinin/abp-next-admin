using LINGYUN.ApiGateway.Data.Filter;
using LINGYUN.ApiGateway.Ocelot;
using LINGYUN.ApiGateway.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class ApiGatewayDbContext : AbpDbContext<ApiGatewayDbContext>
    {
        protected virtual bool IsActivationFilterEnabled => DataFilter?.IsEnabled<IActivation>() ?? false;

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

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
        {
            var expression = base.CreateFilterExpression<TEntity>();

            if (typeof(IActivation).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> expression2 = ((TEntity e) => !IsActivationFilterEnabled || EF.Property<bool>(e, "IsActive"));

                expression = ((expression == null) ? expression2 : CombineExpressions(expression, expression2));
            }

            return expression;
        }
    }
}
