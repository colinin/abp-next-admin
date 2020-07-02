using DotNetCore.CAP;
using LINGYUN.Abp.EventBus.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Extenssions;
using Ocelot.Middleware.Multiplexer;
using Ocelot.Provider.Polly;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpAutoMapperModule),
        typeof(ApiGatewayHttpApiClientModule),
        typeof(AbpCAPEventBusModule),
        typeof(AbpAspNetCoreModule)
        )]
    public class ApiGatewayHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            // 不启用则使用本地配置文件的方式启动Ocelot
            if (configuration.GetValue<bool>("EnabledDynamicOcelot"))
            {
                context.Services.AddSingleton<IFileConfigurationRepository, ApiHttpClientFileConfigurationRepository>();
            }

            PreConfigure<CapOptions>(options =>
            {
                options
                .UseInMemoryStorage()
                .UseDashboard()
                .UseRabbitMQ(rabbitMQOptions =>
                {
                    configuration.GetSection("CAP:RabbitMQ").Bind(rabbitMQOptions);
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ApiGatewayMapperProfile>(validate: true);
            });

            Configure<ApiGatewayOptions>(configuration.GetSection("ApiGateway"));

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Host"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = configuration["AuthServer:ApiName"];
                    options.ApiSecret = configuration["AuthServer:ApiSecret"];
                });

            Configure<IdentityModelHttpRequestMessageOptions>(options =>
            {
                // See https://github.com/abpframework/abp/pull/4564
                options.ConfigureHttpRequestMessage = (requestMessage) => { };
            });

            context.Services
                .AddOcelot()
                .AddPolly()
                .AddSingletonDefinedAggregator<AbpApiDefinitionAggregator>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseAuditing();
            // 启用ws协议
            app.UseWebSockets();
            app.UseOcelot().Wait();
        }
    }
}
