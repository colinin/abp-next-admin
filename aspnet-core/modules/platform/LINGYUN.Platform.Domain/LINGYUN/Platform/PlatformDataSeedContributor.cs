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

namespace LINGYUN.Platform
{
    public class PlatformDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IRouteDataSeeder RouteDataSeeder { get; }
        protected IDataDictionaryDataSeeder DataDictionaryDataSeeder { get; }
        protected IMenuRepository MenuRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }

        public PlatformDataSeedContributor(
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
                var data = await SeedDefaultDataDictionaryAsync(context.TenantId);
                // 预置
                var layout = await SeedDefaultLayoutAsync(data);
                // 首页
                await SeedHomeMenuAsync(layout, data);
                // 管理菜单预置菜单数据
                await SeedAdminMenuAsync(layout, data);
                // saas菜单数据
                await SeedSaasMenuAsync(layout, data);
                // 身份资源菜单数据
                await SeedIdentityServerMenuAsync(layout, data);
                // 审计日志菜单数据
                await SeedAuditingMenuAsync(layout, data);
                // 布局容器预置菜单数据
                await SeedContainerMenuAsync(layout, data);
                // 网关管理菜单数据
                await SeedApiGatewayMenuAsync(layout, data);
                // Oss对象管理菜单数据
                await SeedOssManagementMenuAsync(layout, data);
                // 本地化管理菜单数据
                await SeedLocalizationManagementMenuAsync(layout, data);
            }
        }

        private async Task<Data> SeedDefaultDataDictionaryAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    "Layout",
                    CodeNumberGenerator.CreateCode(1),
                    "Vue Admin Layout Meta Dictionary",
                    "Vue Admin Layout Meta Dictionary",
                    null,
                    tenantId);

            data.AddItem(
                GuidGenerator,
                "roles", // TODO: 是否需要把这一项写入到预置数据?
                "roles",
                "",
                Datas.ValueType.Array,
                "will control the page roles (allow setting multiple roles)");
            data.AddItem(
                GuidGenerator,
                "title",
                "title",
                "component",
                Datas.ValueType.String,
                "the name showed in subMenu and breadcrumb (recommend set)");
            data.AddItem(
                GuidGenerator,
                "icon",
                "icon",
                "icon",
                Datas.ValueType.String,
                "the icon showed in the sidebar");
            data.AddItem(
                GuidGenerator,
                "hidden",
                "hidden",
                "false",
                Datas.ValueType.Boolean,
                "if true, this route will not show in the sidebar (default is false)");
            data.AddItem(
                GuidGenerator,
                "alwaysShow",
                "alwaysShow",
                "false",
                Datas.ValueType.Boolean,
                "if true, will always show the root menu (default is false)");
            data.AddItem(
                GuidGenerator,
                "breadcrumb",
                "breadcrumb",
                "true",
                Datas.ValueType.Boolean,
                "if false, the item will be hidden in breadcrumb (default is true)");
            data.AddItem(
                GuidGenerator,
                "noCache",
                "noCache",
                "false",
                Datas.ValueType.Boolean,
                "if true, the page will not be cached (default is false)");
            data.AddItem(
                GuidGenerator,
                "affix",
                "affix",
                "false",
                Datas.ValueType.Boolean,
                "if true, the tag will affix in the tags-view");
            data.AddItem(
                GuidGenerator,
                "activeMenu",
                "activeMenu",
                "",
                Datas.ValueType.String,
                "if set path, the sidebar will highlight the path you set");

            return data;
        }

        private async Task<Layout> SeedDefaultLayoutAsync(Data data)
        {
            var layout = await RouteDataSeeder.SeedLayoutAsync(
                "Layout",
                "layout/index.vue",
                "Vue Admin Layout",
                data.Id,
                PlatformType.WebMvvm, // 针对当前的vue管理页
                "",
                "Vue Admin Layout",
                data.TenantId
                );

            return layout;
        }

        private async Task SeedHomeMenuAsync(Layout layout, Data data)
        {
            var adminMenu = await SeedMenuAsync(
                layout,
                data,
                "home",
                "/",
                CodeNumberGenerator.CreateCode(1),
                layout.Path,
                "Home",
                "/dashboard",
                "Home",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "home" },
                    { "icon", "home" },
                    { "alwaysShow", true }
                },
                // isPublic: true,
                isPublic: false); // 首页应该是共有的页面

            await SeedMenuAsync(
                layout,
                data,
                "dashboard",
                "dashboard",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/dashboard/index.vue",
                "Dashboard",
                "",
                "Dashboard",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "dashboard" },
                    { "icon", "dashboard" }
                },
                isPublic: false);
        }

        private async Task SeedAdminMenuAsync(Layout layout, Data data)
        {
            var adminMenu = await SeedMenuAsync(
                layout, 
                data,
                "admin", 
                "/admin",
                CodeNumberGenerator.CreateCode(2),
                layout.Path,
                "Admin",
                "",
                "Admin",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "admin" },
                    { "icon", "admin" },
                    { "alwaysShow", true }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "roles",
                "roles",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/admin/roles/index.vue",
                "Manage Roles",
                "",
                "Manage Roles",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "roles" },
                    { "icon", "role" },
                    { "roles", new string[] { "AbpIdentity.Roles" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "users",
                "users",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/admin/users/index.vue",
                "Manage Users",
                "",
                "Manage Users",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "users" },
                    { "icon", "users" },
                    { "roles", new string[] { "AbpIdentity.Users" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "organization-unit",
                "organization-unit",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(3)),
                "views/admin/organization-unit/index.vue",
                "Manage Organization Units",
                "",
                "Manage Organization Units",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "organization-unit" },
                    { "icon", "organization-unit" },
                    { "roles", new string[] { "AbpIdentity.OrganizationUnits" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "claim-type",
                "claim-type",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(4)),
                "views/admin/claim-type/index.vue",
                "Manage Claim Types",
                "",
                "Manage Claim Types",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "claim-type" },
                    { "icon", "claim-type" },
                    { "roles", new string[] { "AbpIdentity.IdentityClaimTypes" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "data-dictionary",
                "data-dictionary",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(5)),
                "views/admin/data-dictionary/index.vue",
                "Manage Data Dictionarys",
                "",
                "Manage Data Dictionarys",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "data-dictionary" },
                    { "icon", "data-dictionary" },
                    { "roles", new string[] { "Platform.DataDictionary" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "settings",
                "settings",
                CodeNumberGenerator.AppendCode(adminMenu.Code, CodeNumberGenerator.CreateCode(6)),
                "views/admin/settings/index.vue",
                "Manage Settings",
                "",
                "Manage Settings",
                adminMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "settings" },
                    { "icon", "settings" },
                    { "roles", new string[] { "AbpSettingManagement.Settings" } }
                },
                new string[] { "admin" });
        }

        private async Task SeedSaasMenuAsync(Layout layout, Data data)
        {
            var saasMenu = await SeedMenuAsync(
                layout,
                data,
                "saas",
                "/saas",
                CodeNumberGenerator.CreateCode(3),
                layout.Path,
                "Saas",
                "",
                "Saas",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "saas" },
                    { "icon", "saas" },
                    { "alwaysShow", true }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "editions",
                "editions",
                CodeNumberGenerator.AppendCode(saasMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/admin/edition/index.vue",
                "Manage Editions",
                "",
                "Manage Editions",
                saasMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "editions" },
                    { "icon", "editions" }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "tenants",
                "tenants",
                CodeNumberGenerator.AppendCode(saasMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/admin/tenants/index.vue",
                "Manage Tenants",
                "",
                "Manage Tenants",
                saasMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "tenants" },
                    { "icon", "tenants" }
                },
                new string[] { "admin" });
        }

        private async Task SeedIdentityServerMenuAsync(Layout layout, Data data)
        {
            var identityServerMenu = await SeedMenuAsync(
                layout,
                data,
                "identity-server",
                "/identity-server",
                CodeNumberGenerator.CreateCode(4),
                layout.Path,
                "Identity Server",
                "",
                "Identity Server",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "identity-server" },
                    { "icon", "identity-server" },
                    { "alwaysShow", true },
                    { "roles", new string[]{ "AbpIdentityServer.Clients", "AbpIdentityServer.ApiResources", "AbpIdentityServer.IdentityResources", "AbpIdentityServer.ApiScopes" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "clients",
                "clients",
                CodeNumberGenerator.AppendCode(identityServerMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/admin/identityServer/client/index.vue",
                "Manage Clients",
                "",
                "Manage Clients",
                identityServerMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "clients" },
                    { "icon", "client" },
                    { "roles", new string[]{ "AbpIdentityServer.Clients" } }
                },
                new string[] { "admin" });
            await SeedMenuAsync(
                layout,
                data,
                "api-resources",
                "api-resources",
                CodeNumberGenerator.AppendCode(identityServerMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/admin/identityServer/api-resources/index.vue",
                "Manage Api Resources",
                "",
                "Manage Api Resources",
                identityServerMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "api-resources" },
                    { "icon", "api" },
                    { "roles", new string[]{ "AbpIdentityServer.ApiResources" } }
                },
                new string[] { "admin" });
            await SeedMenuAsync(
                layout,
                data,
                "identity-resources",
                "identity-resources",
                CodeNumberGenerator.AppendCode(identityServerMenu.Code, CodeNumberGenerator.CreateCode(3)),
                "views/admin/identityServer/identity-resources/index.vue",
                "Manage Identity Resources",
                "",
                "Manage Identity Resources",
                identityServerMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "identity-resources" },
                    { "icon", "identity" },
                    { "roles", new string[]{ "AbpIdentityServer.IdentityResources" } }
                },
                new string[] { "admin" });
            await SeedMenuAsync(
                layout,
                data,
                "api-scopes",
                "api-scopes",
                CodeNumberGenerator.AppendCode(identityServerMenu.Code, CodeNumberGenerator.CreateCode(4)),
                "views/admin/identityServer/api-scopes/index.vue",
                "Manage Api Scopes",
                "",
                "Manage Api Scopes",
                identityServerMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "api-scopes" },
                    { "icon", "api-scopes" },
                    { "roles", new string[]{ "AbpIdentityServer.ApiScopes" } }
                },
                new string[] { "admin" });
        }

        private async Task SeedAuditingMenuAsync(Layout layout, Data data)
        {
            var auditingMenu = await SeedMenuAsync(
                layout,
                data,
                "auditing",
                "/auditing",
                CodeNumberGenerator.CreateCode(5),
                layout.Path,
                "Auditing",
                "",
                "Auditing",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "auditing" },
                    { "icon", "auditing" },
                    { "alwaysShow", true },
                    { "roles", new string[]{ "AbpAuditing.AuditLog", "AbpAuditing.SecurityLog" } }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
                layout,
                data,
                "audit-log",
                "audit-log",
                CodeNumberGenerator.AppendCode(auditingMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/admin/auditing/audit-log/index.vue",
                "Manage AuditLog",
                "",
                "Manage AuditLog",
                auditingMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "audit-log" },
                    { "icon", "audit-log" },
                    { "roles", new string[]{ "AbpAuditing.AuditLog" } }
                },
                new string[] { "admin" });
            await SeedMenuAsync(
                layout,
                data,
                "security-log",
                "security-log",
                CodeNumberGenerator.AppendCode(auditingMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/admin/auditing/security-log/index.vue",
                "Manage SecurityLog",
                "",
                "Manage SecurityLog",
                auditingMenu.Id,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "security-log" },
                    { "icon", "security-log" },
                    { "roles", new string[]{ "AbpAuditing.SecurityLog" } }
                },
                new string[] { "admin" });
        }

        private async Task SeedContainerMenuAsync(Layout layout, Data data)
        {
            var containerRoot = await SeedMenuAsync(
                layout,
                data,
                "container",
                "/container",
                CodeNumberGenerator.CreateCode(6),
                layout.Path,
                "Container",
                "",
                "Manage Container",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "container" },
                    { "icon", "container" },
                    { "alwaysShow", true }
                },
                new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "layouts",
                "layouts",
                CodeNumberGenerator.AppendCode(containerRoot.Code, CodeNumberGenerator.CreateCode(1)),
                "views/container/layouts/index.vue",
                "Manage Layouts",
                "",
                "Manage Layouts",
                containerRoot.Id,
                containerRoot.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "layouts" },
                    { "icon", "layout" }
               },
               new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "menus",
                "menus",
                CodeNumberGenerator.AppendCode(containerRoot.Code, CodeNumberGenerator.CreateCode(2)),
                "views/container/menus/index.vue",
                "Manage Menus",
                "",
                "Manage Menus",
                containerRoot.Id,
                containerRoot.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "menus" },
                    { "icon", "menu" }
               },
               new string[] { "admin" });
        }

        private async Task SeedApiGatewayMenuAsync(Layout layout, Data data)
        {
            var apiGatewayMenu = await SeedMenuAsync(
                layout,
                data,
                "apigateway",
                "/apigateway",
                CodeNumberGenerator.CreateCode(7),
                layout.Path,
                "Manage Api Gateway",
                "/group",
                "Manage Api Gateway",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "api-gateway" },
                    { "icon", "api-gateway" },
                    { "alwaysShow", true },
                    { "roles", new string[] { "ApiGateway.RouteGroup", "ApiGateway.Global", "ApiGateway.Route", "ApiGateway.DynamicRoute", "ApiGateway.AggregateRoute" } },
                },
                new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "group",
                "group",
                CodeNumberGenerator.AppendCode(apiGatewayMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/admin/apigateway/group.vue",
                "Manage Groups",
                "",
                "Manage Groups",
                apiGatewayMenu.Id,
                apiGatewayMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "group" },
                    { "icon", "group" },
                    { "roles", new string[] {  "ApiGateway.RouteGroup" } }
               },
               new string[] { "admin" });
            await SeedMenuAsync(
               layout,
               data,
               "global",
                "global",
                CodeNumberGenerator.AppendCode(apiGatewayMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/admin/apigateway/global.vue",
                "Manage Globals",
                "",
                "Manage Globals",
                apiGatewayMenu.Id,
                apiGatewayMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "global" },
                    { "icon", "global-setting" },
                    { "roles", new string[] { "ApiGateway.Global" } }
               },
               new string[] { "admin" });
            await SeedMenuAsync(
               layout,
               data,
               "route",
                "route",
                CodeNumberGenerator.AppendCode(apiGatewayMenu.Code, CodeNumberGenerator.CreateCode(3)),
                "views/admin/apigateway/route.vue",
                "Manage Routes",
                "",
                "Manage Routes",
                apiGatewayMenu.Id,
                apiGatewayMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "route" },
                    { "icon", "route" },
                    { "roles", new string[] { "ApiGateway.Route" } }
               },
               new string[] { "admin" });
            await SeedMenuAsync(
              layout,
              data,
              "aggregate-route",
               "aggregate-route",
               CodeNumberGenerator.AppendCode(apiGatewayMenu.Code, CodeNumberGenerator.CreateCode(4)),
               "views/admin/apigateway/aggregateRoute.vue",
               "Manage Aggregate Routes",
               "",
               "Manage Aggregate Routes",
               apiGatewayMenu.Id,
               apiGatewayMenu.TenantId,
              new Dictionary<string, object>()
              {
                    { "title", "aggregate-route" },
                    { "icon", "aggregate" },
                    { "roles", new string[] { "ApiGateway.AggregateRoute " } }
              },
              new string[] { "admin" });
        }

        private async Task SeedOssManagementMenuAsync(Layout layout, Data data)
        {
            var ossManagementMenu = await SeedMenuAsync(
                layout,
                data,
                "oss-management",
                "/oss-management",
                CodeNumberGenerator.CreateCode(8),
                layout.Path,
                "Manage Object Storage",
                "/oss-manager",
                "Manage Object Storage",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "oss-management" },
                    { "icon", "file-system" },
                    { "alwaysShow", true },
                    { "roles", new string[] { "AbpOssManagement.Container", "AbpOssManagement.OssObject" } },
                },
                new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "oss-manager",
                "oss-manager",
                CodeNumberGenerator.AppendCode(ossManagementMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/oss-management/index.vue",
                "Manage Oss Object",
                "",
                "Manage Oss Object",
                ossManagementMenu.Id,
                ossManagementMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "oss-objects" },
                    { "icon", "file-system" },
                    { "roles", new string[] { "AbpOssManagement.OssObject" } }
               },
               new string[] { "admin" });
        }

        private async Task SeedLocalizationManagementMenuAsync(Layout layout, Data data)
        {
            var localizationManagementMenu = await SeedMenuAsync(
                layout,
                data,
                "localization-management",
                "/localization-management",
                CodeNumberGenerator.CreateCode(9),
                layout.Path,
                "Manage Localization",
                "/localization",
                "Manage Localization",
                null,
                layout.TenantId,
                new Dictionary<string, object>()
                {
                    { "title", "localization" },
                    { "icon", "localization" },
                    { "alwaysShow", true },
                    { "roles", new string[] { "LocalizationManagement.Resource", "LocalizationManagement.Language", "LocalizationManagement.Text" } },
                },
                new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "resource",
                "resource",
                CodeNumberGenerator.AppendCode(localizationManagementMenu.Code, CodeNumberGenerator.CreateCode(1)),
                "views/localization-management/resources/index.vue",
                "Manage Resource",
                "",
                "Manage Resource",
                localizationManagementMenu.Id,
                localizationManagementMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "resource" },
                    { "icon", "resource" },
                    { "roles", new string[] { "LocalizationManagement.Resource" } }
               },
               new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "language",
                "language",
                CodeNumberGenerator.AppendCode(localizationManagementMenu.Code, CodeNumberGenerator.CreateCode(2)),
                "views/localization-management/languages/index.vue",
                "Manage Language",
                "",
                "Manage Language",
                localizationManagementMenu.Id,
                localizationManagementMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "language" },
                    { "icon", "language" },
                    { "roles", new string[] { "LocalizationManagement.Language" } }
               },
               new string[] { "admin" });

            await SeedMenuAsync(
               layout,
               data,
               "text",
                "text",
                CodeNumberGenerator.AppendCode(localizationManagementMenu.Code, CodeNumberGenerator.CreateCode(3)),
                "views/localization-management/texts/index.vue",
                "Manage Text",
                "",
                "Manage Text",
                localizationManagementMenu.Id,
                localizationManagementMenu.TenantId,
               new Dictionary<string, object>()
               {
                    { "title", "text" },
                    { "icon", "text" },
                    { "roles", new string[] { "LocalizationManagement.Text" } }
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
