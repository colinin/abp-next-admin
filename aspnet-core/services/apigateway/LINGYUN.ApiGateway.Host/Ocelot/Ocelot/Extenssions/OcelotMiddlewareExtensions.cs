using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Configuration;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Errors;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Ocelot.Extenssions
{
    public static class OcelotMiddlewareExtensions
    {
        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder)
        {
            await builder.UseOcelot(new OcelotPipelineConfiguration());
            return builder;
        }

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder, Action<OcelotPipelineConfiguration> pipelineConfiguration)
        {
            OcelotPipelineConfiguration ocelotPipelineConfiguration = new OcelotPipelineConfiguration();
            pipelineConfiguration?.Invoke(ocelotPipelineConfiguration);
            return await builder.UseOcelot(ocelotPipelineConfiguration);
        }

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            await CreateConfiguration(builder);
            ConfigureDiagnosticListener(builder);
            return CreateOcelotPipeline(builder, pipelineConfiguration);
        }

        public static Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder app, Action<IOcelotPipelineBuilder, OcelotPipelineConfiguration> builderAction)
        {
            return app.UseOcelot(builderAction, new OcelotPipelineConfiguration());
        }

        public static async Task<IApplicationBuilder> UseOcelot(this IApplicationBuilder app, Action<IOcelotPipelineBuilder, OcelotPipelineConfiguration> builderAction, OcelotPipelineConfiguration configuration)
        {
            await CreateConfiguration(app);
            ConfigureDiagnosticListener(app);
            OcelotPipelineBuilder ocelotPipelineBuilder = new OcelotPipelineBuilder(app.ApplicationServices);
            builderAction?.Invoke(ocelotPipelineBuilder, configuration ?? new OcelotPipelineConfiguration());
            OcelotRequestDelegate ocelotDelegate = ocelotPipelineBuilder.Build();
            app.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";
            app.Use(async delegate (HttpContext context, Func<Task> task)
            {
                DownstreamContext downstreamContext = new DownstreamContext(context);
                await ocelotDelegate(downstreamContext);
            });
            return app;
        }

        private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
        {
            OcelotPipelineBuilder ocelotPipelineBuilder = new OcelotPipelineBuilder(builder.ApplicationServices);
            ocelotPipelineBuilder.BuildOcelotPipeline(pipelineConfiguration);
            OcelotRequestDelegate firstDelegate = ocelotPipelineBuilder.Build();
            builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";
            builder.Use(async delegate (HttpContext context, Func<Task> task)
            {
                DownstreamContext downstreamContext = new DownstreamContext(context);
                await firstDelegate(downstreamContext);
            });
            return builder;
        }

        private static async Task<IInternalConfiguration> CreateConfiguration(IApplicationBuilder builder)
        {
            /* 因为ABP框架中,Abp.HttpClient这个模块里面
             * RemoteServiceOptions 是用的IOptionsSnapshot注入的,这里会出现一个异常
             * 每个RemoteService服务必须在一个请求范围内
             * 解决方案为重写DynamicHttpProxyInterceptor类,替换IOptions<RemoteServiceOptions>
             * 网关不需要实现网关后台服务地址的实时更新
            */
            var fileConfigRepo = builder.ApplicationServices.GetRequiredService<IFileConfigurationRepository>();
            var fileConfig = await fileConfigRepo.Get();
            // IOptionsMonitor<FileConfiguration> fileConfig = builder.ApplicationServices.GetService<IOptionsMonitor<FileConfiguration>>();
            IInternalConfigurationCreator internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
            Response<IInternalConfiguration> response = await internalConfigCreator.Create(fileConfig.Data);
            if (response.IsError)
            {
                ThrowToStopOcelotStarting(response);
            }
            IInternalConfigurationRepository internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
            internalConfigRepo.AddOrReplace(response.Data);
            return GetOcelotConfigAndReturn(internalConfigRepo);
        }

        private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
        {
            Response<IInternalConfiguration> response = provider.Get();
            if (response?.Data == null || response.IsError)
            {
                ThrowToStopOcelotStarting(response);
            }
            return response.Data;
        }

        private static void ThrowToStopOcelotStarting(Response config)
        {
            throw new Exception("Unable to start Ocelot, errors are: " + string.Join(",", ((IEnumerable<Error>)config.Errors).Select((Func<Error, string>)((Error x) => x.ToString()))));
        }

        private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
        {
            builder.ApplicationServices.GetService<IWebHostEnvironment>();
            OcelotDiagnosticListener service = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
            builder.ApplicationServices.GetService<DiagnosticListener>().SubscribeWithAdapter(service);
        }
    }
}