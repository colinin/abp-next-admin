using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.DataSeeder
{
    public class VbenAdminDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IRouteDataSeeder RouteDataSeeder { get; }
        protected IDataDictionaryDataSeeder DataDictionaryDataSeeder { get; }
        protected IMenuRepository MenuRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }

        public VbenAdminDataSeedContributor(
            ICurrentTenant currentTenant,
            IRouteDataSeeder routeDataSeeder,
            IMenuRepository menuRepository,
            ILayoutRepository layoutRepository,
            IGuidGenerator guidGenerator,
            IDataDictionaryDataSeeder dataDictionaryDataSeeder)
        {
            CurrentTenant = currentTenant;
            GuidGenerator = guidGenerator;
            RouteDataSeeder = routeDataSeeder;
            MenuRepository = menuRepository;
            LayoutRepository = layoutRepository;
            DataDictionaryDataSeeder = dataDictionaryDataSeeder;
        }

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            using (CurrentTenant.Change(context.TenantId))
            {
                var uiDataItem = await SeedUIFrameworkDataAsync(context.TenantId);

                var layoutData = await SeedLayoutDataAsync(context.TenantId);

                var layout = await SeedDefaultLayoutAsync(layoutData, uiDataItem);

                // 首页数据
                await SeedHomeMenuAsync(layout, layoutData);
                // 仪表盘
                await SeedDashboardMenuAsync(layout, layoutData);
                // 管理菜单
                await SeedManageMenuAsync(layout, layoutData);
                // Saas菜单
                await SeedSaasMenuAsync(layout, layoutData);
                // 平台菜单
                await SeedPlatformMenuAsync(layout, layoutData);
                // 网关菜单
                await SeedApiGatewayMenuAsync(layout, layoutData);
                // 多语言菜单
                await SeedLocalizationMenuAsync(layout, layoutData);
                // 对象存储菜单
                await SeedOssManagementMenuAsync(layout, layoutData);
            }
        }

        private async Task<DataItem> SeedUIFrameworkDataAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    "UI Framework",
                    CodeNumberGenerator.CreateCode(2),
                    "UI框架",
                    "UI Framework",
                    null,
                    tenantId,
                    true);

            data.AddItem(
                GuidGenerator,
                "Vue Vben Admin",
                "Vue Vben Admin",
                "Vue Vben Admin",
                Datas.ValueType.String,
                "Vue Vben Admin",
                isStatic: true);

            return data.FindItem("Vue Vben Admin");
        }

        private async Task<Layout> SeedDefaultLayoutAsync(Data data, DataItem uiDataItem)
        {
            var layout = await RouteDataSeeder.SeedLayoutAsync(
               "Vben Admin Layout",
               "LAYOUT", // 路由层面已经处理好了,只需要传递LAYOUT可自动引用布局
               "Vben Admin Layout",
               data.Id,
               uiDataItem.Name,
               "",
               "Vben Admin Layout",
               data.TenantId
               );

            return layout;
        }

        private async Task<Data> SeedLayoutDataAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    "Vben Admin Layout",
                    CodeNumberGenerator.CreateCode(3),
                    "Vben Admin布局约束",
                    "Vben Admin Layout Meta Dictionary",
                    null,
                    tenantId,
                    true);

            data.AddItem(
                GuidGenerator,
                "hideMenu",
                "不在菜单显示",
                "false",
                Datas.ValueType.Boolean,
                "当前路由不在菜单显示",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "icon",
                "图标",
                "",
                Datas.ValueType.String,
                "图标，也是菜单图标",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "currentActiveMenu",
                "当前激活的菜单",
                "",
                Datas.ValueType.String,
                "用于配置详情页时左侧激活的菜单路径",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "ignoreKeepAlive",
                "KeepAlive缓存",
                "false",
                Datas.ValueType.Boolean,
                "是否忽略KeepAlive缓存",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "frameSrc",
                "IFrame地址",
                "",
                Datas.ValueType.String,
                "内嵌iframe的地址",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "transitionName",
                "路由切换动画",
                "",
                Datas.ValueType.String,
                "指定该路由切换的动画名",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "roles",
                "可以访问的角色",
                "",
                Datas.ValueType.Array,
                "可以访问的角色，只在权限模式为Role的时候有效",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "title",
                "路由标题",
                "",
                Datas.ValueType.String,
                "路由title 一般必填",
                false,
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "carryParam",
                "在tab页显示",
                "false",
                Datas.ValueType.Boolean,
                "如果该路由会携带参数，且需要在tab页上面显示。则需要设置为true",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideBreadcrumb",
                "隐藏面包屑",
                "false",
                Datas.ValueType.Boolean,
                "隐藏该路由在面包屑上面的显示",
                isStatic: true);
            data.AddItem(
               GuidGenerator,
               "ignoreAuth",
               "忽略权限",
               "false",
               Datas.ValueType.Boolean,
               "是否忽略权限，只在权限模式为Role的时候有效",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideChildrenInMenu",
                "隐藏所有子菜单",
                "false",
                Datas.ValueType.Boolean,
                "隐藏所有子菜单",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideTab",
                "不在标签页显示",
                "false",
                Datas.ValueType.Boolean,
                "当前路由不在标签页显示",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "affix",
                "固定标签页",
                "false",
                Datas.ValueType.Boolean,
                "是否固定标签页",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "frameFormat",
                "格式化IFrame",
                "false",
                Datas.ValueType.Boolean,
                "扩展的格式化frame，{token}: 在打开的iframe页面传递token请求头");

            return data;
        }

        private async Task SeedHomeMenuAsync(Layout layout, Data data)
        {
            await SeedMenuAsync(
                layout,
                data,
                "Vben Home",
                "/home",
                CodeNumberGenerator.CreateCode(20),
                "/dashboard/welcome/index",
                "Home",
                "",
                "Home",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "routes.dashboard.welcome" },
                    { "icon", "ant-design:home-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", true },
                },
                isPublic: false);
        }

        private async Task SeedDashboardMenuAsync(Layout layout, Data data)
        {
            var menu = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Vben Dashboard",    //name
                "/dashboard",   //path
                CodeNumberGenerator.CreateCode(21), //code
                layout.Path,    //component
                "Dashboard", //displayName
                "/dashboard/analysis",     //redirect
                "Dashboard", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "routes.dashboard.welcome" },
                    { "icon", "ant-design:home-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", true },
                },
                new string[] { "admin" });

            var analysis = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Analysis",    //name
                "/dashboard/analysis",   //path
                CodeNumberGenerator.AppendCode(menu.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/dashboard/analysis/index",    //component
                "Analysis", //displayName
                "",     //redirect
                "Analysis", //description 
                menu.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "routes.dashboard.analysis" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });

            var workbench = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Workbench",    //name
                "/dashboard/workbench",   //path
                CodeNumberGenerator.AppendCode(menu.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/dashboard/workbench/index",    //component
                "Workbench", //displayName
                "",     //redirect
                "Workbench", //description 
                menu.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "routes.dashboard.workbench" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task SeedManageMenuAsync(Layout layout, Data data)
        {
            var manage = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Manage",    //name
                "/manage",   //path
                CodeNumberGenerator.CreateCode(22), //code
                layout.Path,    //component
                "Manage", //displayName
                "",     //redirect
                "Manage", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Manage" },
                    { "icon", "ant-design:control-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });

            var identity = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Identity",    //name
                "/manage/identity",   //path
                CodeNumberGenerator.AppendCode(manage.Code, CodeNumberGenerator.CreateCode(1)), //code
                layout.Path,    //component
                "Identity", //displayName
                "",     //redirect
                "Identity", //description 
                manage.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Identity" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var user = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "User",    //name
                "/manage/identity/user",   //path
                CodeNumberGenerator.AppendCode(identity.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/identity/user/index",    //component
                "User", //displayName
                "",     //redirect
                "User", //description 
                identity.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Users" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var role = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Role",    //name
                "/manage/identity/role",   //path
                CodeNumberGenerator.AppendCode(identity.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/identity/role/index",    //component
                "Role", //displayName
                "",     //redirect
                "Role", //description 
                identity.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Roles" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var claimTypes = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Claim",    //name
                "/manage/identity/claim-types",   //path
                CodeNumberGenerator.AppendCode(identity.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/identity/claim-types/index",    //component
                "Claim", //displayName
                "",     //redirect
                "Claim", //description 
                identity.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Claim Types" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var organizationUnits = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "OrganizationUnits",    //name
                "/manage/identity/organization-units",   //path
                CodeNumberGenerator.AppendCode(identity.Code, CodeNumberGenerator.CreateCode(4)), //code
                "/identity/organization-units/index",    //component
                "Organization Units", //displayName
                "",     //redirect
                "Organization Units", //description 
                identity.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Organization Units" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var securityLog = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "SecurityLogs",    //name
                "/manage/identity/security-logs",   //path
                CodeNumberGenerator.AppendCode(identity.Code, CodeNumberGenerator.CreateCode(5)), //code
                "/identity/security-logs/index",    //component
                "Security Logs", //displayName
                "",     //redirect
                "Security Logs", //description 
                identity.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Security Logs" },
                    { "icon", "" },
                    { "hideTab", false },
                    { "requiredFeatures", "AbpAuditing.Logging.SecurityLog" } // 此路由需要依赖安全日志特性
                },
                new string[] { "admin" });

            var auditLogs = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "AuditLogs",    //name
                "/manage/audit-logs",   //path
                CodeNumberGenerator.AppendCode(manage.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/auditing/index",    //component
                "Audit Logs", //displayName
                "",     //redirect
                "Audit Logs", //description 
                manage.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Audit Logs" },
                    { "icon", "" },
                    { "hideTab", false },
                    { "requiredFeatures", "AbpAuditing.Logging.AuditLog" } // 此路由需要依赖审计日志特性
                },
                new string[] { "admin" });

            var settings = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Settings",    //name
                "/manage/settings",   //path
                CodeNumberGenerator.AppendCode(manage.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/sys/settings/index",    //component
                "Settings", //displayName
                "",     //redirect
                "Settings", //description 
                manage.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Settings" },
                    { "icon", "" },
                    { "hideTab", false },
                    { "requiredFeatures", "SettingManagement.Enable" } // 此路由需要依赖设置管理特性
                },
                new string[] { "admin" });

            var identityServer = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "IdentityServer",    //name
                "/manage/identity-server",   //path
                CodeNumberGenerator.AppendCode(manage.Code, CodeNumberGenerator.CreateCode(4)), //code
                layout.Path,    //component
                "Identity Server", //displayName
                "",     //redirect
                "Identity Server", //description 
                manage.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Identity Server" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var clients = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Clients",    //name
                "/manage/identity-server/clients",   //path
                CodeNumberGenerator.AppendCode(identityServer.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/identity-server/clients/index",    //component
                "Clients", //displayName
                "",     //redirect
                "Clients", //description 
                identityServer.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Clients" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var apiResource = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "ApiResources",    //name
                "/manage/identity-server/api-resources",   //path
                CodeNumberGenerator.AppendCode(identityServer.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/identity-server/api-resources/index",    //component
                "Api Resources", //displayName
                "",     //redirect
                "Api Resources", //description 
                identityServer.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Api Resources" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var identityResources = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "IdentityResources",    //name
                "/manage/identity-server/identity-resources",   //path
                CodeNumberGenerator.AppendCode(identityServer.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/identity-server/identity-resources/index",    //component
                "Identity Resources", //displayName
                "",     //redirect
                "Identity Resources", //description 
                identityServer.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Identity Resources" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var apiScopes = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "ApiScopes",    //name
                "/manage/identity-server/api-scopes",   //path
                CodeNumberGenerator.AppendCode(identityServer.Code, CodeNumberGenerator.CreateCode(4)), //code
                "/identity-server/api-scopes/index",    //component
                "Api Scopes", //displayName
                "",     //redirect
                "Api Scopes", //description 
                identityServer.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Api Scopes" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var persistedGrants = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "PersistedGrants",    //name
                "/manage/identity-server/persisted-grants",   //path
                CodeNumberGenerator.AppendCode(identityServer.Code, CodeNumberGenerator.CreateCode(5)), //code
                "/identity-server/persisted-grants/index",    //component
                "Persisted Grants", //displayName
                "",     //redirect
                "Persisted Grants", //description 
                identityServer.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Persisted Grants" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });

        }

        private async Task SeedSaasMenuAsync(Layout layout, Data data)
        {
            var saas = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Saas",    //name
                "/saas",   //path
                CodeNumberGenerator.CreateCode(23), //code
                layout.Path,    //component
                "Saas", //displayName
                "",     //redirect
                "Saas", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Saas" },
                    { "icon", "ant-design:cloud-server-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });
            var tenants = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Tenants",    //name
                "/saas/tenants",   //path
                CodeNumberGenerator.AppendCode(saas.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/saas/tenant/index",    //component
                "Tenants", //displayName
                "",     //redirect
                "Tenants", //description 
                saas.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Tenants" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task SeedPlatformMenuAsync(Layout layout, Data data)
        {
            var platform = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Platform",    //name
                "/platform",   //path
                CodeNumberGenerator.CreateCode(24), //code
                layout.Path,    //component
                "Platform", //displayName
                "",     //redirect
                "Platform", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Platform" },
                    { "icon", "" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });
            var dataDictionary = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "DataDictionary",    //name
                "/platform/data-dic",   //path
                CodeNumberGenerator.AppendCode(platform.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/platform/dataDic/index",    //component
                "Data Dictionary", //displayName
                "",     //redirect
                "Data Dictionary", //description 
                platform.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Data Dictionary" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var layouts = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Layout",    //name
                "/platform/layout",   //path
                CodeNumberGenerator.AppendCode(platform.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/platform/layout/index",    //component
                "Layout", //displayName
                "",     //redirect
                "Layout", //description 
                platform.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Layout" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var menus = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Menu",    //name
                "/platform/menu",   //path
                CodeNumberGenerator.AppendCode(platform.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/platform/menu/index",    //component
                "Menu", //displayName
                "",     //redirect
                "Menu", //description 
                platform.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Menu" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task SeedApiGatewayMenuAsync(Layout layout, Data data)
        {
            var apiGateway = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "ApiGateway",    //name
                "/api-gateway",   //path
                CodeNumberGenerator.CreateCode(25), //code
                layout.Path,    //component
                "ApiGateway", //displayName
                "",     //redirect
                "ApiGateway", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "ApiGateway" },
                    { "icon", "ant-design:gateway-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });
            var routeGroup = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "RouteGroup",    //name
                "/api-gateway/group",   //path
                CodeNumberGenerator.AppendCode(apiGateway.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/api-gateway/group/index",    //component
                "Route Group", //displayName
                "",     //redirect
                "Route Group", //description 
                apiGateway.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Route Group" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var global = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "GlobalConfiguration",    //name
                "/api-gateway/global",   //path
                CodeNumberGenerator.AppendCode(apiGateway.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/api-gateway/global/index",    //component
                "Global Configuration", //displayName
                "",     //redirect
                "Global Configuration", //description 
                apiGateway.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Global Configuration" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var route = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Route",    //name
                "/api-gateway/route",   //path
                CodeNumberGenerator.AppendCode(apiGateway.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/api-gateway/route/index",    //component
                "Route", //displayName
                "",     //redirect
                "Route", //description 
                apiGateway.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Route" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var aggregate = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "AggregateRoute",    //name
                "/api-gateway/aggregate",   //path
                CodeNumberGenerator.AppendCode(apiGateway.Code, CodeNumberGenerator.CreateCode(4)), //code
                "/api-gateway/aggregate/index",    //component
                "Aggregate Route", //displayName
                "",     //redirect
                "Aggregate Route", //description 
                apiGateway.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Aggregate Route" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task SeedLocalizationMenuAsync(Layout layout, Data data)
        {
            var localization = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Localization",    //name
                "/localization",   //path
                CodeNumberGenerator.CreateCode(26), //code
                layout.Path,    //component
                "Localization", //displayName
                "",     //redirect
                "Localization", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Localization" },
                    { "icon", "ant-design:translation-outlined" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });
            var languages = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Languages",    //name
                "/localization/languages",   //path
                CodeNumberGenerator.AppendCode(localization.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/localization/languages/index",    //component
                "Languages", //displayName
                "",     //redirect
                "Languages", //description 
                localization.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Languages" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var resources = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Resources",    //name
                "/localization/resources",   //path
                CodeNumberGenerator.AppendCode(localization.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/localization/resources/index",    //component
                "Resources", //displayName
                "",     //redirect
                "Resources", //description 
                localization.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Resources" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var texts = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Texts",    //name
                "/localization/texts",   //path
                CodeNumberGenerator.AppendCode(localization.Code, CodeNumberGenerator.CreateCode(3)), //code
                "/localization/texts/index",    //component
                "Texts", //displayName
                "",     //redirect
                "Texts", //description 
                localization.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Texts" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task SeedOssManagementMenuAsync(Layout layout, Data data)
        {
            var oss = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "OssManagement",    //name
                "/oss",   //path
                CodeNumberGenerator.CreateCode(27), //code
                layout.Path,    //component
                "Oss Management", //displayName
                "",     //redirect
                "Oss Management", //description 
                null,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Oss Management" },
                    { "icon", "ant-design:file-twotone" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                },
                new string[] { "admin" });
            var containers = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Containers",    //name
                "/oss/containers",   //path
                CodeNumberGenerator.AppendCode(oss.Code, CodeNumberGenerator.CreateCode(1)), //code
                "/oss-management/containers/index",    //component
                "Containers", //displayName
                "",     //redirect
                "Containers", //description 
                oss.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Containers" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
            var objects = await SeedMenuAsync(
                layout, //layout
                data,   //data
                "Objects",    //name
                "/oss/objects",   //path
                CodeNumberGenerator.AppendCode(oss.Code, CodeNumberGenerator.CreateCode(2)), //code
                "/oss-management/objects/index",    //component
                "Objects", //displayName
                "",     //redirect
                "Objects", //description 
                oss.Id,   //parentId
                layout.TenantId,    //tenantId
                new Dictionary<string, object>()    //meta
                {
                    { "title", "Objects" },
                    { "icon", "" },
                    { "hideTab", false },
                },
                new string[] { "admin" });
        }

        private async Task<Menu> SeedMenuAsync(
            Layout layout,
            Data data,
            string name,
            string path,
            string code,
            string component,
            string displayName,
            string redirect = "",
            string description = "",
            Guid? parentId = null,
            Guid? tenantId = null,
            Dictionary<string, object> meta = null,
            string[] roles = null,
            Guid[] users = null,
            bool isPublic = false
            )
        {
            var menu = await RouteDataSeeder.SeedMenuAsync(
                layout,
                name,
                path,
                code,
                component,
                displayName,
                redirect,
                description,
                parentId,
                tenantId,
                isPublic
                );
            foreach (var item in data.Items)
            {
                menu.SetProperty(item.Name, item.DefaultValue);
            }
            if (meta != null)
            {
                foreach (var item in meta)
                {
                    menu.SetProperty(item.Key, item.Value);
                }
            }

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    await RouteDataSeeder.SeedRoleMenuAsync(role, menu, tenantId);
                }
            }

            if (users != null)
            {
                foreach (var user in users)
                {
                    await RouteDataSeeder.SeedUserMenuAsync(user, menu, tenantId);
                }
            }

            return menu;
        }
    }
}
