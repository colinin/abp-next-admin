using LINGYUN.Abp.MultiTenancy;
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
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.EventBus.Handlers
{
    public class TenantSynchronizer :
        IDistributedEventHandler<CreateEventData>,
        ITransientDependency
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IRouteDataSeeder RouteDataSeeder { get; }
        protected IDataDictionaryDataSeeder DataDictionaryDataSeeder { get; }
        protected IMenuRepository MenuRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }

        public TenantSynchronizer(
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

        /// <summary>
        /// 租户创建之后需要预置租户平台数据
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public virtual async Task HandleEventAsync(CreateEventData eventData)
        {
            using (CurrentTenant.Change(eventData.Id))
            {
                var data = await SeedDefaultDataDictionaryAsync(eventData.Id);
                // 预置
                var layout = await SeedDefaultLayoutAsync(data);
                // 首页
                await SeedHomeMenuAsync(layout, data);
                // 管理菜单预置菜单数据
                await SeedAdminMenuAsync(layout, data);
                // 审计日志菜单数据
                await SeedAuditingMenuAsync(layout, data);
                // 布局容器预置菜单数据
                await SeedContainerMenuAsync(layout, data);
            }
        }

        /// <summary>
        /// 租户删除之后删除租户平台数据
        /// TODO: 不应删除用户数据
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        //public virtual async Task HandleEventAsync(DeleteEventData eventData)
        //{
        //    //await MenuRepository.GetAllAsync();
        //}

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
