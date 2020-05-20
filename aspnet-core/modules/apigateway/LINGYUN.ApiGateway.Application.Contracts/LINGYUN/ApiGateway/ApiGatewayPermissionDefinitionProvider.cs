using LINGYUN.ApiGateway.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.ApiGateway
{
    public class ApiGatewayPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(ApiGatewayPermissions.GroupName, L("Permissions:ApiGateway"), MultiTenancySides.Host);

            var router = group.AddPermission(ApiGatewayPermissions.RouteGroup.Default, L("Permissions:RouteGroup"), MultiTenancySides.Host);
            router.AddChild(ApiGatewayPermissions.RouteGroup.Create, L("Permissions:Create"), MultiTenancySides.Host);
            router.AddChild(ApiGatewayPermissions.RouteGroup.Update, L("Permissions:Update"), MultiTenancySides.Host);
            router.AddChild(ApiGatewayPermissions.RouteGroup.Export, L("Permissions:Export"), MultiTenancySides.Host);
            router.AddChild(ApiGatewayPermissions.RouteGroup.Import, L("Permissions:Import"), MultiTenancySides.Host);
            router.AddChild(ApiGatewayPermissions.RouteGroup.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            var global = group.AddPermission(ApiGatewayPermissions.Global.Default, L("Permissions:Global"), MultiTenancySides.Host);
            global.AddChild(ApiGatewayPermissions.Global.Create, L("Permissions:Create"), MultiTenancySides.Host);
            global.AddChild(ApiGatewayPermissions.Global.Update, L("Permissions:Update"), MultiTenancySides.Host);
            global.AddChild(ApiGatewayPermissions.Global.Export, L("Permissions:Export"), MultiTenancySides.Host);
            global.AddChild(ApiGatewayPermissions.Global.Import, L("Permissions:Import"), MultiTenancySides.Host);
            global.AddChild(ApiGatewayPermissions.Global.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            var route = group.AddPermission(ApiGatewayPermissions.Route.Default, L("Permissions:Route"), MultiTenancySides.Host);
            route.AddChild(ApiGatewayPermissions.Route.Create, L("Permissions:Create"), MultiTenancySides.Host);
            route.AddChild(ApiGatewayPermissions.Route.Update, L("Permissions:Update"), MultiTenancySides.Host);
            route.AddChild(ApiGatewayPermissions.Route.Export, L("Permissions:Export"), MultiTenancySides.Host);
            route.AddChild(ApiGatewayPermissions.Route.Import, L("Permissions:Import"), MultiTenancySides.Host);
            route.AddChild(ApiGatewayPermissions.Route.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            var dynamicRoute = group.AddPermission(ApiGatewayPermissions.DynamicRoute.Default, L("Permissions:DynamicRoute"), MultiTenancySides.Host);
            dynamicRoute.AddChild(ApiGatewayPermissions.DynamicRoute.Create, L("Permissions:Create"), MultiTenancySides.Host);
            dynamicRoute.AddChild(ApiGatewayPermissions.DynamicRoute.Update, L("Permissions:Update"), MultiTenancySides.Host);
            dynamicRoute.AddChild(ApiGatewayPermissions.DynamicRoute.Export, L("Permissions:Export"), MultiTenancySides.Host);
            dynamicRoute.AddChild(ApiGatewayPermissions.DynamicRoute.Import, L("Permissions:Import"), MultiTenancySides.Host);
            dynamicRoute.AddChild(ApiGatewayPermissions.DynamicRoute.Delete, L("Permissions:Delete"), MultiTenancySides.Host);

            var aggregateRoute = group.AddPermission(ApiGatewayPermissions.AggregateRoute.Default, L("Permissions:AggregateRoute"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.Create, L("Permissions:Create"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.Update, L("Permissions:Update"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.Export, L("Permissions:Export"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.Import, L("Permissions:Import"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.Delete, L("Permissions:Delete"), MultiTenancySides.Host);
            aggregateRoute.AddChild(ApiGatewayPermissions.AggregateRoute.ManageRouteConfig, L("Permissions:ManageRouteConfig"), MultiTenancySides.Host);
        }

        protected virtual LocalizableString L(string name)
        {
            return LocalizableString.Create<ApiGatewayResource>(name);
        }
    }
}
