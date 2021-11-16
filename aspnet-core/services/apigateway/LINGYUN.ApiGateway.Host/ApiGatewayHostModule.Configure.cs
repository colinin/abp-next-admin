using DotNetCore.CAP;
using LINGYUN.Abp.Serilog.Enrichers.Application;
using LINGYUN.ApiGateway.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Middleware.Multiplexer;
using Ocelot.Provider.Polly;
using StackExchange.Redis;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.ApiGateway
{
    public partial class ApiGatewayHostModule
    {
        private void PreConfigureApp()
        {
            AbpSerilogEnrichersConsts.ApplicationName = "ApiGateWay";
        }

        private void PreConfigureCAP(IConfiguration configuration)
        {
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

        private void PreConfigureApiGateway(IServiceCollection services, IConfiguration configuration)
        {
            // 不启用则使用本地配置文件的方式启动Ocelot
            if (configuration.GetValue<bool>("EnabledDynamicOcelot"))
            {
                services.AddSingleton<IFileConfigurationRepository, ApiHttpClientFileConfigurationRepository>();
            }
        }

        private void ConfigureKestrelServer(IConfiguration configuration, bool isDevelopment = true)
        {
            // fix: 不限制请求体大小，解决上传文件问题
            Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null;
                options.Limits.MaxRequestBufferSize = null;
            });

            if (!isDevelopment)
            {
                // Ssl证书
                var sslOptions = configuration.GetSection("App:SslOptions");
                if (sslOptions.Exists())
                {
                    var fileName = sslOptions["FileName"];
                    var password = sslOptions["Password"];
                    Configure<KestrelServerOptions>(options =>
                    {
                        options.ConfigureEndpointDefaults(cfg =>
                        {
                            cfg.UseHttps(fileName, password);
                        });
                    });
                }
            }
        }

        private void ConfigureAbpAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ApiGatewayMapperProfile>(validate: true);
            });
        }

        private void ConfigureJsonSerializer()
        {
            // 中文序列化的编码问题
            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
        }

        private void ConfigureVirtualFileSystem()
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ApiGatewayHostModule>();
            });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));

                options.Resources
                    .Get<ApiGatewayResource>()
                    .AddVirtualJson("/Localization/Host");
            });
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddApplicationPart(typeof(ApiGatewayHostModule).Assembly);

            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                    endpointContext.Endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
            });
        }

        private void ConfigureOcelot(IServiceCollection services)
        {
            services
                .AddOcelot()
                .AddPolly()
                .AddSingletonDefinedAggregator<AbpApiDefinitionAggregator>();
        }

        private void ConfigureCaching(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                // 最好统一命名,不然某个缓存变动其他应用服务有例外发生
                options.KeyPrefix = "LINGYUN.Abp.Application";
                // 滑动过期30天
                options.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromDays(30d);
                // 绝对过期60天
                options.GlobalCacheEntryOptions.AbsoluteExpiration = DateTimeOffset.Now.AddDays(60d);
            });

            Configure<RedisCacheOptions>(options =>
            {
                var redisConfig = ConfigurationOptions.Parse(options.Configuration);
                options.ConfigurationOptions = redisConfig;
                options.InstanceName = configuration["Redis:InstanceName"];
            });
        }

        private void ConfigureApiGateway(IConfiguration configuration)
        {
            Configure<ApiGatewayOptions>(configuration.GetSection("ApiGateway"));
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Open API Document", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "bearer",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new string[] { }
                        }
                    });
                    options.OperationFilter<TenantHeaderParamter>();
                });
        }

        private void ConfigureSecurity(IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = configuration["AuthServer:ApiName"];
                });

            if (!isDevelopment)
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                services
                    .AddDataProtection()
                    .SetApplicationName("LINGYUN.Abp.Application")
                    .PersistKeysToStackExchangeRedis(redis, "LINGYUN.Abp.Application:DataProtection:Protection-Keys");
            }
        }
    }
}
