using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.Abp.Serilog.Enrichers.UniqueId;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Middleware;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Modularity;

namespace LINGYUN.MicroService.Internal.ApiGateway
{

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSerilogEnrichersApplicationModule),
        typeof(AbpSerilogEnrichersUniqueIdModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpAspNetCoreSerilogModule)
        )]
    public partial class InternalApiGatewayModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigureApp();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureLocalization();
            ConfigureJsonSerializer();
            ConfigureVirtualFileSystem();

            ConfigureCaching(configuration);
            ConfigureApiGateway(configuration);

            ConfigureKestrelServer(configuration, hostingEnvironment.IsDevelopment());
            ConfigureSecurity(context.Services, configuration, hostingEnvironment.IsDevelopment());

            ConfigureMvc(context.Services);
            ConfigureSwagger(context.Services);
            ConfigureCors(context.Services, configuration);
            ConfigureOcelot(context.Services, configuration);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseAuditing();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseAbpClaimsMap();
            app.MapWhen(
                ctx => ctx.Request.Path.ToString().StartsWith("/api/ApiGateway/Basic/"),
                appNext =>
                {
                    // 仅针对属于网关自己的控制器进入MVC管道
                    appNext.UseRouting();
                    appNext.UseConfiguredEndpoints();
                });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Open API Document");
                var options = app.ApplicationServices.GetRequiredService<IOptions<InternalApiGatewayOptions>>().Value;
                foreach (var api in options.DownstreamOpenApis)
                {
                    c.SwaggerEndpoint(api.EndPoint, api.Name);
                }
            });
            // 启用ws协议
            app.UseWebSockets();
            app.UseAbpSerilogEnrichers();
            app.UseOcelot().Wait();
        }
    }
}
