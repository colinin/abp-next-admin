using Elsa.Extensions;
using Elsa.Features.Services;
using LINGYUN.Abp.ElsaNext.Localization;
using LINGYUN.Abp.ElsaNext.Permissions;
using LINGYUN.Abp.ElsaNext.Server.Permissions;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace LINGYUN.Abp.ElsaNext.Server;

[DependsOn(typeof(AbpElsaNextModule))]
public class AbpElsaNextServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Configuration.GetSection("Elsa");

        PreConfigure<IModule>(elsa =>
        {
            // see: https://v3.elsaworkflows.io/docs/installation/elsa-server

            // Use timer activities.
            elsa.UseScheduling();

            // Enable JavaScript workflow expressions.
            elsa.UseJavaScript();

            // Enable Liquid workflow expressions.
            elsa.UseLiquid();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            // Enable HTTP activities.
            elsa.UseHttp(http =>
            {
                http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options);
            });

            // Expose Elsa API endpoints.
            elsa.UseWorkflowsApi();

            // Operational dashboard API 
            elsa.UseDashboardApi();

            if (configuration["Server:EnableRealTimeWorkflows"] != "false")
            {
                elsa.UseRealTimeWorkflows();
            }

            // Register custom activities from the application, if any.
            elsa.AddActivitiesFrom<AbpElsaNextServerModule>();

            // Register custom workflows from the application, if any.
            elsa.AddWorkflowsFrom<AbpElsaNextServerModule>();
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpElsaNextServerModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ElsaNextResource>()
                .AddVirtualJson("/LINGYUN/Abp/ElsaNext/Server/Localization/Resources");
        });

        Configure<AbpElsaPermissionMapOptions>(options =>
        {
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Default,
                ElsaWorkflowPermissions.Definitions.Read);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Delete,
                ElsaWorkflowPermissions.Definitions.Delete);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Execute,
                ElsaWorkflowPermissions.Definitions.Execute);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Publish,
                ElsaWorkflowPermissions.Definitions.Publish);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Retract,
                ElsaWorkflowPermissions.Definitions.Retract);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Retract,
                ElsaWorkflowPermissions.Definitions.Retract);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Update,
                ElsaWorkflowPermissions.Definitions.Write);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Create,
                ElsaWorkflowPermissions.Definitions.Write);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Refresh,
                ElsaWorkflowPermissions.Definitions.Refresh);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Reload,
                ElsaWorkflowPermissions.Definitions.Reload);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Versions.Default,
                ElsaWorkflowPermissions.Definitions.Read);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Versions.Delete,
                ElsaWorkflowPermissions.Definitions.Delete);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Definitions.Versions.Revert,
                ElsaWorkflowPermissions.Definitions.Publish);

            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Default,
                ElsaWorkflowPermissions.Instances.Read);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Delete,
                ElsaWorkflowPermissions.Instances.Delete);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Create,
                ElsaWorkflowPermissions.Instances.Write);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Update,
                ElsaWorkflowPermissions.Instances.Write);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Cancel,
                ElsaWorkflowPermissions.Instances.Cancel);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Variables.Default,
                ElsaWorkflowPermissions.Instances.Read);
            options.MapPermission(
                ElsaWorkflowPermissionNames.Instances.Variables.Update,
                ElsaWorkflowPermissions.Instances.Write);
        });
    }
}
