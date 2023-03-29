using Elsa.Server.Api;
using Elsa.Server.Api.Mapping;
using Elsa.Server.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Elsa;

[DependsOn(
    typeof(AbpElsaModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpElsaServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services
            .AddSingleton<ConnectionConverter>()
            .AddSingleton<ActivityBlueprintConverter>()
            .AddScoped<IWorkflowBlueprintMapper, WorkflowBlueprintMapper>()
            .AddSingleton<IEndpointContentSerializerSettingsProvider, EndpointContentSerializerSettingsProvider>()
            .AddSignalR();

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(ElsaApiOptions).Assembly);
        });

        PreConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(
                typeof(ElsaApiOptions).Assembly,
                controller =>
                {
                    controller.ApiVersions.Add(ApiVersion.Default);
                    //controller.ApiVersionConfigurer += (version =>
                    //{
                    //    version.ReportApiVersions = true;
                    //    version.DefaultApiVersion = ApiVersion.Default;
                    //    version.AssumeDefaultVersionWhenUnspecified = true;
                    //});
                });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AutoMapperProfile>(validate: false);
            options.AddProfile<AbpElsaServerAutoMapperProfile>(validate: false);
        });
    }
}
