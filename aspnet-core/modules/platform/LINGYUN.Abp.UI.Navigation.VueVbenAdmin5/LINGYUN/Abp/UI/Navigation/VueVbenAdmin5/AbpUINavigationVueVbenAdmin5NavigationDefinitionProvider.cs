using System.Security.Principal;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin5;

public class AbpUINavigationVueVbenAdmin5NavigationDefinitionProvider : NavigationDefinitionProvider
{
    public override void Define(INavigationDefinitionContext context)
    {
        context.Add(GetDashboard());
        context.Add(GetAccount());
        context.Add(GetManage());
        context.Add(GetSaas());
        context.Add(GetPlatform());
        context.Add(GetOssManagement());
        context.Add(GetTaskManagement());
        context.Add(GetWebhooksManagement());
        context.Add(GetTextTemplating());
        context.Add(GetVbenDemos());
    }

    private static NavigationDefinition[] GetVbenDemos()
    {
        var project = new ApplicationMenu(
            name: "VbenProject",
            displayName: "项目",
            url: "/vben-admin",
            component: "",
            description: "项目",
            order: 9998,
            icon: "https://unpkg.com/@vbenjs/static-source@0.1.7/source/logo-v1.webp")
            .SetProperty("badgeType", "dot")
            .SetProperty("title", "demos.vben.title");
        project.AddItem(
            new ApplicationMenu(
                name: "VbenDocument",
                displayName: "文档",
                url: "/vben-admin/document",
                component: "",
                icon: "lucide:book-open-text",
                description: "文档")
            .SetProperty("link", "https://doc.vben.pro")
            .SetProperty("title", "demos.vben.document")
         );
        project.AddItem(
            new ApplicationMenu(
                name: "VbenGithub",
                displayName: "文档",
                url: "/vben-admin/github",
                component: "",
                icon: "mdi:github",
                description: "文档")
            .SetProperty("link", "https://github.com/vbenjs/vue-vben-admin")
            .SetProperty("title", "Github")
         );
        project.AddItem(
            new ApplicationMenu(
                name: "VbenNaive",
                displayName: "Naive UI 版本",
                url: "/vben-admin/naive",
                component: "",
                icon: "logos:naiveui",
                description: "Naive UI 版本")
            .SetProperty("badgeType", "dot")
            .SetProperty("link", "https://naive.vben.pro")
            .SetProperty("title", "demos.vben.naive-ui")
         );
        project.AddItem(
            new ApplicationMenu(
                name: "VbenElementPlus",
                displayName: "Element Plus 版本",
                url: "/vben-admin/ele",
                component: "",
                icon: "logos:element",
                description: "Element Plus 版本")
            .SetProperty("badgeType", "dot")
            .SetProperty("link", "https://ele.vben.pro")
            .SetProperty("title", "demos.vben.element-plus")
         );

        var about = new ApplicationMenu(
            name: "VbenAbout",
            displayName: "关于",
            url: "/vben-admin/about",
            component: "/_core/about/index",
            description: "关于",
            order: 9999,
            icon: "lucide:copyright")
            .SetProperty("title", "demos.vben.about");

        return new NavigationDefinition[2]
        {
            new NavigationDefinition(project),
            new NavigationDefinition(about),
        };
    }

    private static NavigationDefinition GetAccount()
    {
        var account = new ApplicationMenu(
            name: "Vben5Account",
            displayName: "账户管理",
            url: "/account",
            component: "",
            description: "账户管理",
            icon: "mdi:account-outline")
            .SetProperty("hideInMenu", "true")
            .SetProperty("title", "abp.account.title");

        account.AddItem(
            new ApplicationMenu(
                name: "Vben5AccountMySettings",
                displayName: "个人设置",
                url: "/account/my-settings",
                component: "/account/my-settings/index",
                icon: "tdesign:user-setting",
                description: "个人设置")
            .SetProperty("title", "abp.account.settings.title")
         );

        return new NavigationDefinition(account);
    }

    private static NavigationDefinition GetDashboard()
    {
        var dashboard = new ApplicationMenu(
            name: "Vben5Dashboard",
            displayName: "仪表盘",
            url: "/dashboard",
            component: "",
            description: "仪表盘",
            icon: "lucide:layout-dashboard",
            order: -1)
            .SetProperty("title", "page.dashboard.title");

        dashboard.AddItem(
            new ApplicationMenu(
                name: "Vben5Analysis",
                displayName: "分析页",
                url: "/analytics",
                component: "/dashboard/analytics/index",
                icon: "lucide:area-chart",
                description: "分析页")
            .SetProperty("affixTab", "true")
            .SetProperty("title", "page.dashboard.analytics")
         );

        dashboard.AddItem(
           new ApplicationMenu(
               name: "Vben5Workbench",
               displayName: "工作台",
               url: "/workspace",
               component: "/dashboard/workspace/index",
               icon: "carbon:workspace",
               description: "工作台")
           .SetProperty("title", "page.dashboard.workspace")
        );

        return new NavigationDefinition(dashboard);
    }

    private static NavigationDefinition GetManage()
    {
        var manage = new ApplicationMenu(
            name: "Vben5Manage",
            displayName: "管理",
            url: "/manage",
            component: "",
            description: "管理",
            icon: "arcticons:activity-manager")
            .SetProperty("title", "abp.manage.title");

        var openIddict = manage.AddItem(
                new ApplicationMenu(
                    name: "Vben5OpenIddict",
                    displayName: "身份认证服务器",
                    url: "/manage/openiddict",
                    component: "",
                    icon: "mdi:openid",
                    description: "身份认证服务器(OpenIddict)",
                    multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.openiddict.title"));
        openIddict.AddItem(
            new ApplicationMenu(
                name: "Vben5OpenIddictApplications",
                displayName: "应用管理",
                url: "/manage/openiddict/applications",
                component: "/openiddict/applications/index",
                icon: "carbon:application",
                description: "应用管理",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.openiddict.applications"));
        openIddict.AddItem(
            new ApplicationMenu(
                name: "Vben5OpenIddictAuthorizations",
                displayName: "授权管理",
                url: "/manage/openiddict/authorizations",
                component: "/openiddict/authorizations/index",
                icon: "arcticons:ente-authenticator",
                description: "授权管理",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.openiddict.authorizations"));
        openIddict.AddItem(
            new ApplicationMenu(
                name: "Vben5OpenIddictScopes",
                displayName: "范围管理",
                url: "/manage/openiddict/scopes",
                component: "/openiddict/scopes/index",
                icon: "et:scope",
                description: "范围管理",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.openiddict.scopes"));
        openIddict.AddItem(
            new ApplicationMenu(
                name: "Vben5OpenIddictTokens",
                displayName: "授权令牌",
                url: "/manage/openiddict/tokens",
                component: "/openiddict/tokens/index",
                icon: "oui:token-key",
                description: "授权令牌",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.openiddict.tokens"));

        var identity = manage.AddItem(
            new ApplicationMenu(
                name: "Vben5Identity",
                displayName: "身份认证管理",
                url: "/manage/identity",
                component: "",
                icon: "teenyicons:id-outline",
                description: "身份认证管理")
            .SetProperty("title", "abp.manage.identity.title"));
        identity.AddItem(
          new ApplicationMenu(
              name: "Vben5IdentityUsers",
              displayName: "用户管理",
              url: "/manage/identity/users",
              component: "/identity/users/index",
              icon: "mdi:user-outline",
              description: "用户管理")
          .SetProperty("title", "abp.manage.identity.user"));
        identity.AddItem(
          new ApplicationMenu(
              name: "Vben5IdentityRoles",
              displayName: "角色管理",
              url: "/manage/identity/roles",
              component: "/identity/roles/index",
              icon: "carbon:user-role",
              description: "角色管理")
          .SetProperty("title", "abp.manage.identity.role"));
        identity.AddItem(
          new ApplicationMenu(
              name: "Vben5IdentityClaimTypes",
              displayName: "身份标识",
              url: "/manage/identity/claim-types",
              component: "/identity/claim-types/index",
              icon: "la:id-card-solid",
              description: "身份标识",
              multiTenancySides: MultiTenancySides.Host)
          .SetProperty("title", "abp.manage.identity.claimTypes"));
        identity.AddItem(
          new ApplicationMenu(
              name: "Vben5IdentityOrganizationUnits",
              displayName: "组织机构",
              url: "/manage/identity/organization-units",
              component: "/identity/organization-units/index",
              icon: "clarity:organization-line",
              description: "组织机构")
          .SetProperty("title", "abp.manage.identity.organizationUnits"));
        identity.AddItem(
          new ApplicationMenu(
              name: "SecurityLogs",
              displayName: "安全日志",
              url: "/manage/identity/security-logs",
              component: "/identity/security-logs/index",
              icon: "carbon:security",
              description: "安全日志")
          .SetProperty("title", "abp.manage.identity.securityLogs")
          .SetProperty("requiredFeatures", "AbpAuditing.Logging.SecurityLog"));
        identity.AddItem(
          new ApplicationMenu(
              name: "Vben5IdentitySessions",
              displayName: "会话管理",
              url: "/manage/identity/sessions",
              component: "/identity/sessions/index",
              icon: "carbon:prompt-session",
              description: "会话管理")
          .SetProperty("title", "abp.manage.identity.sessions"));

        var permissionManagement = manage.AddItem(new ApplicationMenu(
              name: "Vben5Permissions",
              displayName: "权限管理",
              url: "/manage/permissions",
              component: "",
              description: "权限管理",
              icon: "arcticons:permissionsmanager",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.permissions.title"));
        permissionManagement.AddItem(new ApplicationMenu(
               name: "Vben5PermissionsGroupDefinitions",
               displayName: "权限分组",
               url: "/manage/permissions/groups",
               component: "/permissions/groups/index",
               icon: "lucide:group",
               description: "权限分组",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.permissions.groups"));
        permissionManagement.AddItem(new ApplicationMenu(
               name: "Vben5PermissionsDefinitions",
               displayName: "权限定义",
               url: "/manage/permissions/definitions",
               component: "/permissions/definitions/index",
               icon: "icon-park-outline:permissions",
               description: "权限定义",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.permissions.definitions"));


        var featureManagement = manage.AddItem(new ApplicationMenu(
               name: "Vben5Features",
               displayName: "功能管理",
               url: "/manage/features",
               component: "",
               description: "功能管理",
               icon: "ant-design:gold-outlined",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.features.title"));
        featureManagement.AddItem(new ApplicationMenu(
               name: "Vben5FeaturesGroupDefinitions",
               displayName: "功能分组",
               url: "/manage/features/groups",
               component: "/features/groups/index",
               icon: "lucide:group",
               description: "功能分组",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.features.groups"));
        featureManagement.AddItem(new ApplicationMenu(
               name: "Vben5FeaturesDefinitions",
               displayName: "功能定义",
               url: "/manage/features/definitions",
               component: "/features/definitions/index",
               icon: "pajamas:feature-flag",
               description: "功能定义",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.features.definitions"));

        var settingManagement = manage.AddItem(new ApplicationMenu(
               name: "Vben5Settings",
               displayName: "设置管理",
               url: "/manage/settings",
               component: "",
               description: "设置管理",
               icon: "ic:outline-settings")
            .SetProperty("title", "abp.manage.settings.title")
            // 此路由需要依赖设置管理特性
            .SetProperty("requiredFeatures", "SettingManagement.Enable"));
        settingManagement.AddItem(new ApplicationMenu(
               name: "Vben5SettingsSystem",
               displayName: "系统设置",
               url: "/manage/settings/system",
               component: "/settings/system/index",
               icon: "tabler:settings-cog",
               description: "系统设置")
            .SetProperty("title", "abp.manage.settings.system")
            // 此路由需要依赖设置管理特性
            .SetProperty("requiredFeatures", "SettingManagement.Enable"));
        settingManagement.AddItem(new ApplicationMenu(
               name: "Vben5SettingsDefinitions",
               displayName: "设置定义",
               url: "/manage/settings/definitions",
               component: "/settings/definitions/index",
               icon: "codicon:settings",
               description: "设置定义",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.settings.definitions"));

        var localization = manage.AddItem(new ApplicationMenu(
            name: "Vben5Localizations",
            displayName: "本地化管理",
            url: "/manage/localization",
            component: "",
            description: "本地化管理",
            icon: "ion:globe-outline",
            multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.localization.title"));
        localization.AddItem(
          new ApplicationMenu(
              name: "Vben5LocalizationsLanguages",
              displayName: "语言管理",
              url: "/manage/localization/languages",
              component: "/localization/languages/index",
              icon: "cil:language",
              description: "语言管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.localization.languages")
            );
        localization.AddItem(
          new ApplicationMenu(
              name: "Vben5LocalizationsResources",
              displayName: "资源管理",
              url: "/manage/localization/resources",
              component: "/localization/resources/index",
              icon: "grommet-icons:resources",
              description: "资源管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.localization.resources")
            );
        localization.AddItem(
          new ApplicationMenu(
              name: "Vben5LocalizationsTexts",
              displayName: "文档管理",
              url: "/manage/localization/texts",
              component: "/localization/texts/index",
              icon: "mi:text",
              description: "文档管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.localization.texts")
            );

        var dataProtection = manage.AddItem(new ApplicationMenu(
              name: "Vben5DataProtection",
              displayName: "数据保护",
              url: "/manage/data-protection",
              component: "",
              description: "数据保护",
              icon: "icon-park-outline:protect",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.dataProtection.title"));
        dataProtection.AddItem(new ApplicationMenu(
               name: "Vben5DataProtectionEntityTypeInfos",
               displayName: "实体管理",
               url: "/manage/data-protection/entity-type-infos",
               component: "/data-protection/entity-type-infos/index",
               icon: "iconamoon:type",
               description: "实体管理",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.dataProtection.entityTypeInfos"));

        manage.AddItem(new ApplicationMenu(
               name: "Vben5AuditingAuditLogs",
               displayName: "审计日志",
               url: "/manage/audit-logs",
               component: "/auditing/audit-logs/index",
               icon: "fluent-mdl2:compliance-audit",
               description: "审计日志")
            .SetProperty("title", "abp.manage.auditLogs")
            // 此路由需要依赖审计日志特性
            .SetProperty("requiredFeatures", "AbpAuditing.Logging.AuditLog"));

        manage.AddItem(
            new ApplicationMenu(
                name: "Vben5AuditingLoggings",
                displayName: "系统日志",
                url: "/manage/sys-logs",
                component: "/auditing/loggings/index",
                icon: "icon-park-outline:log",
                description: "系统日志",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.loggings"));

        var notificationManagement = manage.AddItem(new ApplicationMenu(
              name: "Vben5Notifications",
              displayName: "通知管理",
              url: "/manage/notifications",
              component: "",
              description: "通知管理",
              icon: "tabler:notification")
            .SetProperty("title", "abp.manage.notifications.title"));
        notificationManagement.AddItem(new ApplicationMenu(
               name: "Vben5NotificationsMyNotifilers",
               displayName: "我的通知",
               url: "/manage/notifications/my-notifilers",
               component: "/notifications/my-notifilers/index",
               icon: "ant-design:notification-outlined",
               description: "我的通知")
            .SetProperty("title", "abp.manage.notifications.myNotifilers"));
        notificationManagement.AddItem(new ApplicationMenu(
               name: "Vben5NotificationsGroupDefinitions",
               displayName: "通知分组",
               url: "/manage/notifications/groups",
               component: "/notifications/groups/index",
               icon: "lucide:group",
               description: "通知分组",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.notifications.groups"));
        notificationManagement.AddItem(new ApplicationMenu(
               name: "NotificationsDefinitions",
               displayName: "通知定义",
               url: "/manage/notifications/definitions",
               component: "/notifications/definitions/index",
               icon: "nimbus:notification",
               description: "通知定义",
               multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.notifications.definitions"));

        manage.AddItem(
            new ApplicationMenu(
                name: "Vben5ApiDocument",
                displayName: "Api 文档",
                url: "/manage/openapi",
                component: "IFrame",
                description: "Api 文档",
                multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.manage.openApi")
            // TODO: 注意在部署完毕之后手动修改此菜单iframe地址
            .SetProperty("iframeSrc", "http://127.0.0.1:30000/swagger/index.html"));

        manage.AddItem(
            new ApplicationMenu(
                name: "Vben5Caches",
                displayName: "缓存管理",
                url: "/manage/cache",
                component: "/caching/caches/index",
                description: "缓存管理")
            .SetProperty("title", "abp.manage.cache"));

        return new NavigationDefinition(manage);
    }

    private static NavigationDefinition GetSaas()
    {
        var saas = new ApplicationMenu(
            name: "Vben5Saas",
            displayName: "Saas",
            url: "/saas",
            component: "",
            description: "Saas",
            icon: "ant-design:cloud-server-outlined",
            multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.saas.title");
        saas.AddItem(
          new ApplicationMenu(
              name: "Vben5SaasTenants",
              displayName: "租户管理",
              url: "/saas/tenants",
              component: "/saas/tenants/index",
              icon: "arcticons:tenantcloud-pro",
              description: "租户管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.saas.tenants"));
        saas.AddItem(
          new ApplicationMenu(
              name: "Vben5SaasEditions",
              displayName: "版本管理",
              url: "/saas/editions",
              component: "/saas/editions/index",
              icon: "icon-park-outline:multi-rectangle",
              description: "版本管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.saas.editions"));

        return new NavigationDefinition(saas);
    }

    private static NavigationDefinition GetPlatform()
    {
        var platform = new ApplicationMenu(
            name: "Vben5Platform",
            displayName: "平台管理",
            url: "/platform",
            component: "",
            description: "平台管理",
            icon: "ep:platform")
            .SetProperty("title", "abp.platform.title");
        platform.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformDataDictionaries",
              displayName: "数据字典",
              url: "/platform/data-dictionaries",
              component: "/platform/data-dictionaries/index",
              icon: "material-symbols:dictionary-outline",
              description: "数据字典")
            .SetProperty("title", "abp.platform.dataDictionaries"));
        platform.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformLayouts",
              displayName: "布局管理",
              url: "/platform/layouts",
              component: "/platform/layouts/index",
              icon: "material-symbols-light:responsive-layout",
              description: "布局管理")
            .SetProperty("title", "abp.platform.layouts"));
        platform.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformMenus",
              displayName: "菜单管理",
              url: "/platform/menus",
              component: "/platform/menus/index",
              icon: "material-symbols-light:menu",
              description: "菜单管理")
            .SetProperty("title", "abp.platform.menus"));

        var messages = platform.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformMessages",
              displayName: "消息管理",
              url: "/platform/messages",
              component: "",
              icon: "tabler:message-cog",
              description: "消息管理",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.platform.messages.title"));
        messages.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformEmailMessages",
              displayName: "邮件消息",
              url: "/platform/messages/email",
              component: "/platform/messages/email/index",
              icon: "material-symbols:attach-email-outline",
              description: "邮件消息",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.platform.messages.email"));
        messages.AddItem(
          new ApplicationMenu(
              name: "Vben5PlatformSmsMessages",
              displayName: "短信消息",
              url: "/platform/messages/sms",
              component: "/platform/messages/sms/index",
              icon: "material-symbols:sms-outline",
              description: "短信消息",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.platform.messages.sms"));

        return new NavigationDefinition(platform);
    }

    private static NavigationDefinition GetOssManagement()
    {
        var oss = new ApplicationMenu(
            name: "Vben5Oss",
            displayName: "对象存储",
            url: "/oss",
            component: "",
            description: "对象存储",
            icon: "icon-park-outline:cloud-storage")
            .SetProperty("title", "abp.oss.title");
        oss.AddItem(
          new ApplicationMenu(
              name: "Vben5OssContainers",
              displayName: "容器管理",
              url: "/oss/containers",
              component: "/oss/containers/index",
              icon: "mdi:bucket-outline",
              description: "容器管理")
            .SetProperty("title", "abp.oss.containers"));
        oss.AddItem(
          new ApplicationMenu(
              name: "Vben5OssObjects",
              displayName: "文件管理",
              url: "/oss/objects",
              component: "/oss/objects/index",
              icon: "mdi-light:file",
              description: "文件管理")
            .SetProperty("title", "abp.oss.objects"));

        return new NavigationDefinition(oss);
    }

    private static NavigationDefinition GetTaskManagement()
    {
        var task = new ApplicationMenu(
            name: "Vben5Tasks",
            displayName: "任务管理",
            url: "/task-management",
            component: "",
            description: "任务管理",
            icon: "eos-icons:background-tasks")
            .SetProperty("title", "abp.tasks.title");
        task.AddItem(
          new ApplicationMenu(
              name: "Vben5TasksJobInfos",
              displayName: "任务队列",
              url: "/task-management/background-jobs",
              component: "/tasks/job-infos/index",
              icon: "eos-icons:job",
              description: "任务队列")
            .SetProperty("title", "abp.tasks.jobInfo.title"));

        return new NavigationDefinition(task);
    }

    private static NavigationDefinition GetWebhooksManagement()
    {
        var webhooks = new ApplicationMenu(
            name: "Vben5Webhooks",
            displayName: "WebHooks",
            url: "/webhooks",
            component: "",
            description: "WebHooks",
            icon: "material-symbols:webhook",
            multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.webhooks.title");
        webhooks.AddItem(
          new ApplicationMenu(
              name: "Vben5WebhooksGroupDefinitions",
              displayName: "Webhook分组",
              url: "/webhooks/groups",
              component: "/webhooks/groups/index",
              icon: "lucide:group",
              description: "Webhook分组",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.webhooks.groups"));
        webhooks.AddItem(
          new ApplicationMenu(
              name: "Vben5WebhooksDefinitions",
              displayName: "Webhook定义",
              url: "/webhooks/definitions",
              component: "/webhooks/definitions/index",
              icon: "material-symbols:webhook",
              description: "Webhook定义",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.webhooks.definitions"));
        webhooks.AddItem(
          new ApplicationMenu(
              name: "Vben5WebhooksSubscriptions",
              displayName: "管理订阅",
              url: "/webhooks/subscriptions",
              component: "/webhooks/subscriptions/index",
              icon: "material-symbols:subscriptions",
              description: "管理订阅",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.webhooks.subscriptions"));
        webhooks.AddItem(
          new ApplicationMenu(
              name: "Vben5WebhooksSendAttempts",
              displayName: "管理记录",
              url: "/webhooks/send-attempts",
              component: "/webhooks/send-attempts/index",
              icon: "material-symbols:history",
              description: "管理记录",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.webhooks.sendAttempts"));

        return new NavigationDefinition(webhooks);
    }

    private static NavigationDefinition GetTextTemplating()
    {
        var textTemplating = new ApplicationMenu(
            name: "Vben5TextTemplating",
            displayName: "模板管理",
            url: "/text-templating",
            component: "",
            description: "模板管理",
            icon: "tdesign:template",
            multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.textTemplating.title");
        textTemplating.AddItem(
          new ApplicationMenu(
              name: "Vben5TextTemplatingDefinitions",
              displayName: "模板定义",
              url: "/text-templating/definitions",
              component: "/text-templating/definitions/index",
              icon: "qlementine-icons:template-16",
              description: "模板定义",
              multiTenancySides: MultiTenancySides.Host)
            .SetProperty("title", "abp.textTemplating.definitions"));

        return new NavigationDefinition(textTemplating);
    }
}
