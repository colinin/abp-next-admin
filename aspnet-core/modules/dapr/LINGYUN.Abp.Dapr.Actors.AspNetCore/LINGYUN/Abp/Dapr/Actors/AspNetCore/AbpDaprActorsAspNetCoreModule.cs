using Dapr.Actors;
using Dapr.Actors.Runtime;
using LINGYUN.Abp.Wrapper;
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
        typeof(AbpAspNetCoreModule),
        typeof(AbpWrapperModule))]
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

            Configure<AbpWrapperOptions>(options =>
            {
                options.IgnoredInterfaces.TryAdd<IActor>();
            });
        }

        private static void AddDefinitionActor(IServiceCollection services)
        {
            var actorRegistrations = new List<ActorRegistration>();

            services.OnRegistred(context =>
            {
                if (typeof(IActor).IsAssignableFrom(context.ImplementationType) &&
                    !actorRegistrations.Contains(context.ImplementationType))
                {
                    var actorRegistration = new ActorRegistration(context.ImplementationType.GetActorTypeInfo());

                    actorRegistrations.Add(actorRegistration);
                }
            });

            services.AddActors(options =>
            {
                options.Actors.AddIfNotContains(actorRegistrations);
            });
        }
    }
}
