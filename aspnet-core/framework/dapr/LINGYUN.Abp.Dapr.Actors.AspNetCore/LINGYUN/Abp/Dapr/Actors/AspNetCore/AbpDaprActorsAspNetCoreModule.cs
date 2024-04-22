using Dapr.Actors;
using Dapr.Actors.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr.Actors.AspNetCore
{
    [DependsOn(
        typeof(AbpAspNetCoreModule))]
    public class AbpDaprActorsAspNetCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            AddDefinitionActor(context.Services);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpEndpointRouterOptions>(options =>
            {
                options.EndpointConfigureActions.Add(endpointContext =>
                {
                    endpointContext.Endpoints.MapActorsHandlers();
                });
            });
        }

        private static void AddDefinitionActor(IServiceCollection services)
        {
            var actorRegistrations = new List<ActorRegistration>();

            services.OnRegistered(context =>
            {
                if (typeof(IActor).IsAssignableFrom(context.ImplementationType) &&
                    !actorRegistrations.Contains(context.ImplementationType))
                {
                    var actorRegistration = new ActorRegistration(context.ImplementationType.GetActorTypeInfo());

                    actorRegistrations.Add(actorRegistration);
                }
            });
            // 使Actor Runtime可配置
            var preActions = services.GetPreConfigureActions<ActorRuntimeOptions>();
            services.AddActors(options =>
            {
                preActions.Configure(options);
                options.Actors.AddIfNotContains(actorRegistrations);
            });
        }
    }
}
