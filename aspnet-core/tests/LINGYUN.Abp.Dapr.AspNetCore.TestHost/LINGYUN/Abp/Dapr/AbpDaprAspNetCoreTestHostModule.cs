using LINGYUN.Abp.AspNetCore.Mvc.Wrapper;
using LINGYUN.Abp.Dapr.Actors.AspNetCore;
using LINGYUN.Abp.Dapr.ServiceInvocation;
using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Dapr
{
    [DependsOn(
        typeof(AbpDaprActorsAspNetCoreModule),
        typeof(AbpAspNetCoreMvcWrapperModule)
        )]
    public class AbpDaprAspNetCoreTestHostModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            Configure<IMvcBuilder>(builder =>
            {
                builder.AddApplicationPart(typeof(AbpDaprAspNetCoreTestHostModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpWrapperOptions>(options =>
            {
                options.IsEnabled = true;

                options.IgnoreReturnTypes.TryAdd<NameValue>();
                options.IgnoreReturnTypes.TryAdd<ListResultDto<NameValue>>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseRouting();
            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }
    }
}
