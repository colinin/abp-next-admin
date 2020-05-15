using LINGYUN.ApiGateway.Ocelot;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.ApiGateway.EntityFrameworkCore
{
    public static class ApiGatewayDbContextModelCreatingExtensions
    {
        public static void ConfigureApiGateway(
            this ModelBuilder builder,
            Action<ApiGatewayModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ApiGatewayModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<CacheOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "CacheOptions", options.Schema);

                e.Property(p => p.Region).HasMaxLength(256);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.CacheOptions)
                 .HasForeignKey<CacheOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AuthenticationOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "AuthOptions", options.Schema);

                e.Property(p => p.AllowedScopes).HasMaxLength(200);
                e.Property(p => p.AuthenticationProviderKey).HasMaxLength(100);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.AuthenticationOptions)
                 .HasForeignKey<AuthenticationOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Headers>(e =>
            {
                e.ToTable(options.TablePrefix + "Headers", options.Schema);

                e.Property(p => p.ReRouteId).IsRequired();
                e.Property(p => p.Key).HasMaxLength(50);
                e.Property(p => p.Value).HasMaxLength(256);
            });

            builder.Entity<HostAndPort>(e =>
            {
                e.ToTable(options.TablePrefix + "HostAndPort", options.Schema);

                e.Property(p => p.ReRouteId).IsRequired();
                e.Property(p => p.Host).IsRequired().HasMaxLength(50);
                e.Property(p => p.Port).HasDefaultValue(0);
            });

            builder.Entity<HttpHandlerOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "HttpOptions", options.Schema);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.HttpHandlerOptions)
                 .HasForeignKey<HttpHandlerOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.GlobalConfiguration)
                 .WithOne(q => q.HttpHandlerOptions)
                 .HasForeignKey<HttpHandlerOptions>(fk => fk.ItemId)
                 .HasPrincipalKey<GlobalConfiguration>(pk => pk.ItemId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<LoadBalancerOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "BalancerOptions", options.Schema);

                e.Property(p => p.Type).HasMaxLength(100);
                e.Property(p => p.Key).HasMaxLength(100);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.LoadBalancerOptions)
                 .HasForeignKey<LoadBalancerOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.GlobalConfiguration)
                 .WithOne(q => q.LoadBalancerOptions)
                 .HasForeignKey<LoadBalancerOptions>(fk => fk.ItemId)
                 .HasPrincipalKey<GlobalConfiguration>(pk => pk.ItemId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<QoSOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "QoSOptions", options.Schema);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.QoSOptions)
                 .HasForeignKey<QoSOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.GlobalConfiguration)
                 .WithOne(q => q.QoSOptions)
                 .HasForeignKey<QoSOptions>(fk => fk.ItemId)
                 .HasPrincipalKey<GlobalConfiguration>(pk => pk.ItemId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RateLimitOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "RateLimitOptions", options.Schema);

                e.Property(p => p.ClientIdHeader).HasMaxLength(50);
                e.Property(p => p.QuotaExceededMessage).HasMaxLength(256);
                e.Property(p => p.RateLimitCounterPrefix).HasMaxLength(50);

                e.Property(p => p.HttpStatusCode).HasDefaultValue(429);
                e.Property(p => p.ClientIdHeader).HasDefaultValue("ClientId");
                e.Property(p => p.RateLimitCounterPrefix).HasDefaultValue("ocelot");

                e.HasOne(p => p.GlobalConfiguration)
                 .WithOne(q => q.RateLimitOptions)
                 .HasForeignKey<RateLimitOptions>(fk => fk.ItemId)
                 .HasPrincipalKey<GlobalConfiguration>(pk => pk.ItemId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<RateLimitRule>(e =>
            {
                e.ToTable(options.TablePrefix + "RateLimitRule", options.Schema);

                e.Property(p => p.Period).HasMaxLength(50);
                e.Property(p => p.ClientWhitelist).HasMaxLength(1000);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.RateLimitOptions)
                 .HasForeignKey<RateLimitRule>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.DynamicReRoute)
                 .WithOne(q => q.RateLimitRule)
                 .HasForeignKey<RateLimitRule>(fk => fk.DynamicReRouteId)
                 .HasPrincipalKey<DynamicReRoute>(pk => pk.DynamicReRouteId)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SecurityOptions>(e =>
            {
                e.ToTable(options.TablePrefix + "SecurityOptions", options.Schema);

                e.Property(p => p.IPAllowedList).HasMaxLength(1000);
                e.Property(p => p.IPBlockedList).HasMaxLength(1000);

                e.HasOne(p => p.ReRoute)
                 .WithOne(q => q.SecurityOptions)
                 .HasForeignKey<SecurityOptions>(fk => fk.ReRouteId)
                 .HasPrincipalKey<ReRoute>(pk => pk.ReRouteId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ServiceDiscoveryProvider>(e =>
            {
                e.ToTable(options.TablePrefix + "Discovery", options.Schema);

                e.Property(p => p.Host).HasMaxLength(50);
                e.Property(p => p.Type).HasMaxLength(128);
                e.Property(p => p.Token).HasMaxLength(256);
                e.Property(p => p.Namespace).HasMaxLength(128);
                e.Property(p => p.Scheme).HasMaxLength(50);
                e.Property(p => p.ConfigurationKey).HasMaxLength(256);

                e.HasOne(p => p.GlobalConfiguration)
                 .WithOne(q => q.ServiceDiscoveryProvider)
                 .HasForeignKey<ServiceDiscoveryProvider>(fk => fk.ItemId)
                 .HasPrincipalKey<GlobalConfiguration>(pk => pk.ItemId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AggregateReRouteConfig>(e =>
            {
                e.ToTable(options.TablePrefix + "AggregateConfig", options.Schema);

                e.Property(p => p.ReRouteId).IsRequired();
                e.Property(p => p.ReRouteKey).HasMaxLength(256);
                e.Property(p => p.Parameter).HasMaxLength(1000);
                e.Property(p => p.JsonPath).HasMaxLength(256);

            });

            builder.Entity<RouteGroup>(e =>
            {
                e.ToTable(options.TablePrefix + "RouteGroup", options.Schema);

                e.Property(p => p.AppId).IsRequired().HasMaxLength(50);
                e.Property(p => p.AppIpAddress).IsRequired().HasMaxLength(256);
                e.Property(p => p.AppName).IsRequired().HasMaxLength(100);
                e.Property(p => p.Name).IsRequired().HasMaxLength(50);

                e.Property(p => p.Description).HasMaxLength(256);

                e.ConfigureAuditedAggregateRoot();

                e.HasIndex(p => new { p.AppId, p.AppName, p.AppIpAddress });
            });

            builder.Entity<AggregateReRoute>(e =>
            {
                e.ToTable(options.TablePrefix + "Aggregate", options.Schema);

                e.Property(p => p.ReRouteId).IsRequired();
                e.Property(p => p.AppId).IsRequired().HasMaxLength(50);

                e.Property(p => p.Aggregator).HasMaxLength(256);
                e.Property(p => p.ReRouteKeys).HasMaxLength(1000);
                e.Property(p => p.UpstreamHost).HasMaxLength(1000);
                e.Property(p => p.UpstreamPathTemplate).HasMaxLength(1000);
                e.Property(p => p.UpstreamHttpMethod).HasMaxLength(50);

                e.Property(p => p.ReRouteIsCaseSensitive).HasDefaultValue(false);

                e.ConfigureConcurrencyStamp();
                e.ConfigureExtraProperties();
            });

            builder.Entity<DynamicReRoute>(e =>
            {
                e.ToTable(options.TablePrefix + "DynamicReRoute", options.Schema);

                e.Property(p => p.AppId).IsRequired().HasMaxLength(50);
                e.Property(p => p.ServiceName).IsRequired().HasMaxLength(100);
                e.Property(p => p.DownstreamHttpVersion).HasMaxLength(30);

                e.ConfigureConcurrencyStamp();
                e.ConfigureExtraProperties();
            });

            builder.Entity<GlobalConfiguration>(e =>
            {
                e.ToTable(options.TablePrefix + "GlobalConfiguration", options.Schema);

                e.Property(p => p.AppId).IsRequired().HasMaxLength(50);
                e.Property(p => p.ItemId).IsRequired();
                e.Property(p => p.RequestIdKey).HasMaxLength(100);
                e.Property(p => p.BaseUrl).IsRequired().HasMaxLength(256);
                e.Property(p => p.DownstreamScheme).HasMaxLength(100);
                e.Property(p => p.DownstreamHttpVersion).HasMaxLength(30);

                e.ConfigureSoftDelete();
                e.ConfigureConcurrencyStamp();
                e.ConfigureExtraProperties();
            });

            builder.Entity<ReRoute>(e =>
            {
                e.ToTable(options.TablePrefix + "ReRoute", options.Schema);

                e.Property(p => p.AppId).IsRequired().HasMaxLength(50);
                e.Property(p => p.ReRouteId).IsRequired();
                e.Property(p => p.ReRouteName).IsRequired().HasMaxLength(50);
                e.Property(p => p.RequestIdKey).HasMaxLength(100);
                e.Property(p => p.DownstreamScheme).HasMaxLength(100);
                e.Property(p => p.DownstreamPathTemplate).IsRequired().HasMaxLength(100);
                e.Property(p => p.UpstreamPathTemplate).IsRequired().HasMaxLength(100);
                e.Property(p => p.UpstreamHttpMethod).IsRequired().HasMaxLength(50);
                e.Property(p => p.DownstreamHttpMethod).HasMaxLength(100);
                e.Property(p => p.ServiceName).HasMaxLength(100);
                e.Property(p => p.ServiceNamespace).HasMaxLength(100);
                e.Property(p => p.DownstreamHttpVersion).HasMaxLength(30);
                e.Property(p => p.UpstreamHost).HasMaxLength(100);
                e.Property(p => p.Key).HasMaxLength(100);

                e.Property(p => p.ChangeDownstreamPathTemplate).HasMaxLength(1000);
                e.Property(x => x.AddHeadersToRequest).HasMaxLength(1000);
                e.Property(x => x.UpstreamHeaderTransform).HasMaxLength(1000);
                e.Property(x => x.DownstreamHeaderTransform).HasMaxLength(1000);
                e.Property(x => x.AddClaimsToRequest).HasMaxLength(1000);
                e.Property(x => x.RouteClaimsRequirement).HasMaxLength(1000);
                e.Property(x => x.AddQueriesToRequest).HasMaxLength(1000);
                e.Property(x => x.DownstreamHostAndPorts).HasMaxLength(1000);
                e.Property(x => x.DelegatingHandlers).HasMaxLength(1000);

                e.HasIndex(i => new { i.DownstreamPathTemplate, i.UpstreamPathTemplate }).IsUnique();

                e.ConfigureConcurrencyStamp();
                e.ConfigureExtraProperties();
            });
        }
    }
}
