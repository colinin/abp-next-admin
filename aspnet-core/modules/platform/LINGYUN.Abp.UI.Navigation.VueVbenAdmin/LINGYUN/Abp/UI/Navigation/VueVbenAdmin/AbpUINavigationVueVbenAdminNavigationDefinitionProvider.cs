using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin
{
    public class AbpUINavigationVueVbenAdminNavigationDefinitionProvider : NavigationDefinitionProvider
    {
        public override void Define(INavigationDefinitionContext context)
        {
            context.Add(GetDashboard());
            context.Add(GetManage());
            context.Add(GetSaas());
            context.Add(GetPlatform());
            context.Add(GetApiGateway());
            context.Add(GetLocalization());
            context.Add(GetOssManagement());
            context.Add(GetTaskManagement());
        }

        private static NavigationDefinition GetDashboard()
        {
            var dashboard = new ApplicationMenu(
                name: "Vben Dashboard",
                displayName: "仪表盘",
                url: "/dashboard",
                component: "",
                description: "仪表盘",
                icon: "ion:grid-outline",
                redirect: "/dashboard/workbench");

            dashboard.AddItem(
                new ApplicationMenu(
                    name: "Analysis",
                    displayName: "分析页",
                    url: "/dashboard/analysis",
                    component: "/dashboard/analysis/index",
                    description: "分析页"));
            dashboard.AddItem(
               new ApplicationMenu(
                   name: "Workbench",
                   displayName: "工作台",
                   url: "/dashboard/workbench",
                   component: "/dashboard/workbench/index",
                   description: "工作台"));


            return new NavigationDefinition(dashboard);
        }

        private static NavigationDefinition GetManage()
        {
            var manage = new ApplicationMenu(
                name: "Manage",
                displayName: "管理",
                url: "/manage",
                component: "",
                description: "管理",
                icon: "ant-design:control-outlined");

            var identity = manage.AddItem(
                new ApplicationMenu(
                    name: "Identity",
                    displayName: "身份认证管理",
                    url: "/manage/identity",
                    component: "",
                    description: "身份认证管理"));
            identity.AddItem(
              new ApplicationMenu(
                  name: "User",
                  displayName: "用户",
                  url: "/manage/identity/user",
                  component: "/identity/user/index",
                  description: "用户"));
            identity.AddItem(
              new ApplicationMenu(
                  name: "Role",
                  displayName: "角色",
                  url: "/manage/identity/role",
                  component: "/identity/role/index",
                  description: "角色"));
            identity.AddItem(
              new ApplicationMenu(
                  name: "Claim",
                  displayName: "身份标识",
                  url: "/manage/identity/claim-types",
                  component: "/identity/claim-types/index",
                  description: "身份标识",
                  multiTenancySides: MultiTenancySides.Host));
            identity.AddItem(
              new ApplicationMenu(
                  name: "OrganizationUnits",
                  displayName: "组织机构",
                  url: "/manage/identity/organization-units",
                  component: "/identity/organization-units/index",
                  description: "组织机构"));
            identity.AddItem(
              new ApplicationMenu(
                  name: "SecurityLogs",
                  displayName: "安全日志",
                  url: "/manage/identity/security-logs",
                  component: "/identity/security-logs/index",
                  description: "安全日志")
                // 此路由需要依赖安全日志特性
                .SetProperty("requiredFeatures", "AbpAuditing.Logging.SecurityLog"));

            manage.AddItem(new ApplicationMenu(
                   name: "AuditLogs",
                   displayName: "审计日志",
                   url: "/manage/audit-logs",
                   component: "/auditing/index",
                   description: "审计日志")
                // 此路由需要依赖审计日志特性
                .SetProperty("requiredFeatures", "AbpAuditing.Logging.AuditLog"));

            manage.AddItem(new ApplicationMenu(
                   name: "Settings",
                   displayName: "设置",
                   url: "/manage/settings",
                   component: "/sys/settings/index",
                   description: "设置")
                // 此路由需要依赖设置管理特性
                .SetProperty("requiredFeatures", "SettingManagement.Enable"));

            var identityServer = manage.AddItem(
                new ApplicationMenu(
                    name: "IdentityServer",
                    displayName: "身份认证服务器",
                    url: "/manage/identity-server",
                    component: "",
                    description: "身份认证服务器",
                    multiTenancySides: MultiTenancySides.Host));
            identityServer.AddItem(
                new ApplicationMenu(
                    name: "Clients",
                    displayName: "客户端",
                    url: "/manage/identity-server/clients",
                    component: "/identity-server/clients/index",
                    description: "客户端",
                    multiTenancySides: MultiTenancySides.Host));
            identityServer.AddItem(
                new ApplicationMenu(
                    name: "ApiResources",
                    displayName: "Api 资源",
                    url: "/manage/identity-server/api-resources",
                    component: "/identity-server/api-resources/index",
                    description: "Api 资源",
                    multiTenancySides: MultiTenancySides.Host));
            identityServer.AddItem(
                new ApplicationMenu(
                    name: "IdentityResources",
                    displayName: "身份资源",
                    url: "/manage/identity-server/identity-resources",
                    component: "/identity-server/identity-resources/index",
                    description: "身份资源",
                    multiTenancySides: MultiTenancySides.Host));
            identityServer.AddItem(
                new ApplicationMenu(
                    name: "ApiScopes",
                    displayName: "Api 范围",
                    url: "/manage/identity-server/api-scopes",
                    component: "/identity-server/api-scopes/index",
                    description: "Api 范围",
                    multiTenancySides: MultiTenancySides.Host));
            identityServer.AddItem(
                new ApplicationMenu(
                    name: "PersistedGrants",
                    displayName: "持久授权",
                    url: "/manage/identity-server/persisted-grants",
                    component: "/identity-server/persisted-grants/index",
                    description: "持久授权",
                    multiTenancySides: MultiTenancySides.Host));


            manage.AddItem(
                new ApplicationMenu(
                    name: "Logs",
                    displayName: "系统日志",
                    url: "/sys/logs",
                    component: "/sys/logging/index",
                    description: "系统日志"));

            manage.AddItem(
                new ApplicationMenu(
                    name: "ApiDocument",
                    displayName: "Api 文档",
                    url: "/openapi",
                    component: "IFrame",
                    description: "Api 文档",
                    multiTenancySides: MultiTenancySides.Host)
                // TODO: 注意在部署完毕之后手动修改此菜单iframe地址
                .SetProperty("frameSrc", "http://127.0.0.1:30000/swagger/index.html"));

            return new NavigationDefinition(manage);
        }

        private static NavigationDefinition GetSaas()
        {
            var saas = new ApplicationMenu(
                name: "Saas",
                displayName: "Saas",
                url: "/saas",
                component: "",
                description: "Saas",
                icon: "ant-design:cloud-server-outlined",
                multiTenancySides: MultiTenancySides.Host);
            saas.AddItem(
              new ApplicationMenu(
                  name: "Tenants",
                  displayName: "租户管理",
                  url: "/saas/tenants",
                  component: "/saas/tenant/index",
                  description: "租户管理",
                  multiTenancySides: MultiTenancySides.Host));

            return new NavigationDefinition(saas);
        }

        private static NavigationDefinition GetPlatform()
        {
            var platform = new ApplicationMenu(
                name: "Platform",
                displayName: "平台管理",
                url: "/platform",
                component: "",
                description: "平台管理");
            platform.AddItem(
              new ApplicationMenu(
                  name: "DataDictionary",
                  displayName: "数据字典",
                  url: "/platform/data-dic",
                  component: "/platform/dataDic/index",
                  description: "数据字典"));
            platform.AddItem(
              new ApplicationMenu(
                  name: "Layout",
                  displayName: "布局",
                  url: "/platform/layout",
                  component: "/platform/layout/index",
                  description: "布局"));
            platform.AddItem(
              new ApplicationMenu(
                  name: "Menu",
                  displayName: "菜单",
                  url: "/platform/menu",
                  component: "/platform/menu/index",
                  description: "菜单"));

            return new NavigationDefinition(platform);
        }

        private static NavigationDefinition GetApiGateway()
        {
            var apiGateway = new ApplicationMenu(
                name: "ApiGateway",
                displayName: "网关管理",
                url: "/api-gateway",
                component: "",
                description: "网关管理",
                icon: "ant-design:gateway-outlined",
                multiTenancySides: MultiTenancySides.Host);
            apiGateway.AddItem(
              new ApplicationMenu(
                  name: "RouteGroup",
                  displayName: "路由分组",
                  url: "/api-gateway/group",
                  component: "/api-gateway/group/index",
                  description: "路由分组",
                  multiTenancySides: MultiTenancySides.Host));
            apiGateway.AddItem(
              new ApplicationMenu(
                  name: "GlobalConfiguration",
                  displayName: "公共配置",
                  url: "/api-gateway/global",
                  component: "/api-gateway/global/index",
                  description: "公共配置",
                  multiTenancySides: MultiTenancySides.Host));
            apiGateway.AddItem(
              new ApplicationMenu(
                  name: "Route",
                  displayName: "路由管理",
                  url: "/api-gateway/route",
                  component: "/api-gateway/route/index",
                  description: "路由管理",
                  multiTenancySides: MultiTenancySides.Host));
            apiGateway.AddItem(
             new ApplicationMenu(
                 name: "AggregateRoute",
                 displayName: "聚合路由",
                 url: "/api-gateway/aggregate",
                 component: "/api-gateway/aggregate/index",
                 description: "聚合路由",
                 multiTenancySides: MultiTenancySides.Host));

            return new NavigationDefinition(apiGateway);
        }

        private static NavigationDefinition GetLocalization()
        {
            var localization = new ApplicationMenu(
                name: "Localization",
                displayName: "本地化管理",
                url: "/localization",
                component: "",
                description: "本地化管理",
                icon: "ant-design:translation-outlined",
                multiTenancySides: MultiTenancySides.Host);
            localization.AddItem(
              new ApplicationMenu(
                  name: "Languages",
                  displayName: "语言管理",
                  url: "/localization/languages",
                  component: "/localization/languages/index",
                  description: "语言管理",
                  multiTenancySides: MultiTenancySides.Host)
                );
            localization.AddItem(
              new ApplicationMenu(
                  name: "Resources",
                  displayName: "资源管理",
                  url: "/localization/resources",
                  component: "/localization/resources/index",
                  description: "资源管理",
                  multiTenancySides: MultiTenancySides.Host)
                );
            localization.AddItem(
              new ApplicationMenu(
                  name: "Texts",
                  displayName: "文档管理",
                  url: "/localization/texts",
                  component: "/localization/texts/index",
                  description: "文档管理",
                  multiTenancySides: MultiTenancySides.Host)
                );

            return new NavigationDefinition(localization);
        }

        private static NavigationDefinition GetOssManagement()
        {
            var oss = new ApplicationMenu(
                name: "OssManagement",
                displayName: "对象存储",
                url: "/oss",
                component: "",
                description: "对象存储",
                icon: "ant-design:file-twotone");
            oss.AddItem(
              new ApplicationMenu(
                  name: "Containers",
                  displayName: "容器管理",
                  url: "/oss/containers",
                  component: "/oss-management/containers/index",
                  description: "容器管理"));
            oss.AddItem(
              new ApplicationMenu(
                  name: "Objects",
                  displayName: "文件管理",
                  url: "/oss/objects",
                  component: "/oss-management/objects/index",
                  description: "文件管理"));

            return new NavigationDefinition(oss);
        }

        private static NavigationDefinition GetTaskManagement()
        {
            var task = new ApplicationMenu(
                name: "TaskManagement",
                displayName: "任务调度平台",
                url: "/task-management",
                component: "",
                description: "任务调度平台",
                icon: "");
            task.AddItem(
              new ApplicationMenu(
                  name: "BackgroundJobs",
                  displayName: "任务管理",
                  url: "/task-management/background-jobs",
                  component: "/task-management/background-jobs/index",
                  description: "任务管理"));

            return new NavigationDefinition(task);
        }
    }
}
