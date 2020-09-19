using DotNetCore.CAP;
using LINGYUN.Abp.EventBus.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Extenssions;
using Ocelot.Middleware.Multiplexer;
using Ocelot.Provider.Polly;
using StackExchange.Redis;
using System;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.IdentityModel;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.ApiGateway
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientIdentityModelModule),
        typeof(AbpCachingStackExchangeRedisModule),
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
                .UseSqlite("Data Source=./event-bus-cap.db")
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

            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 最好统一命名,不然某个缓存变动其他应用服务有例外发生
                options.KeyPrefix = "LINGYUN.Abp.Application";
                // 滑动过期30天
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30);
                // 绝对过期60天
                options.GlobalCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60);
            });

            Configure<RedisCacheOptions>(options =>
            {
                var redisConfig = ConfigurationOptions.Parse(options.Configuration);
                // 单独一个缓存数据库
                var databaseConfig = configuration.GetSection("Redis:DefaultDatabase");
                if (databaseConfig.Exists())
                {
                    redisConfig.DefaultDatabase = databaseConfig.Get<int>();
                }
                options.ConfigurationOptions = redisConfig;
                options.InstanceName = configuration["Redis:InstanceName"];
            });

            // 加解密
            Configure<AbpStringEncryptionOptions>(options =>
            {
                var encryptionConfiguration = configuration.GetSection("Encryption");
                if (encryptionConfiguration.Exists())
                {
                    options.DefaultPassPhrase = encryptionConfiguration["PassPhrase"] ?? options.DefaultPassPhrase;
                    options.DefaultSalt = encryptionConfiguration.GetSection("Salt").Exists()
                        ? Encoding.ASCII.GetBytes(encryptionConfiguration["Salt"])
                        : options.DefaultSalt;
                    options.InitVectorBytes = encryptionConfiguration.GetSection("InitVector").Exists()
                        ? Encoding.ASCII.GetBytes(encryptionConfiguration["InitVector"])
                        : options.InitVectorBytes;
                }
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
