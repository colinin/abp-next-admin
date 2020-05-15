using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Extenssions;
using Ocelot.Provider.Polly;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Http.Client;
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
        typeof(AbpAspNetCoreModule)
        )]
    public class ApiGatewayHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ApiGatewayMapperProfile>(validate: true);
            });

            Configure<ApiGatewayOptions>(configuration.GetSection("ApiGateway"));

            // 不启用则使用本地配置文件的方式启动Ocelot
            if (configuration.GetValue<bool>("EnabledDynamicOcelot"))
            {
                context.Services.AddSingleton<IFileConfigurationRepository, ApiHttpClientFileConfigurationRepository>();
                ConfigureCAP(context.Services, configuration);
            }

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Host"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = configuration["AuthServer:ApiName"];
                    options.ApiSecret = configuration["AuthServer:ApiSecret"];
                });

            context.Services.AddOcelot().AddPolly();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseAuditing();
            app.UseOcelot().Wait();
        }

        private void ConfigureCAP(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(x =>
            {
                x.UseInMemoryStorage();

                x.UseDashboard();

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = configuration.GetValue<string>("CAP:RabbitMQ:Connect:Host");
                    cfg.VirtualHost = configuration.GetValue<string>("CAP:RabbitMQ:Connect:VirtualHost");
                    cfg.Port = configuration.GetValue<int>("CAP:RabbitMQ:Connect:Port");
                    cfg.UserName = configuration.GetValue<string>("CAP:RabbitMQ:Connect:UserName");
                    cfg.Password = configuration.GetValue<string>("CAP:RabbitMQ:Connect:Password");
                    cfg.ExchangeName = configuration.GetValue<string>("CAP:RabbitMQ:Connect:ExchangeName");
                });

                x.FailedRetryCount = 5;
            });
        }
    }
}
