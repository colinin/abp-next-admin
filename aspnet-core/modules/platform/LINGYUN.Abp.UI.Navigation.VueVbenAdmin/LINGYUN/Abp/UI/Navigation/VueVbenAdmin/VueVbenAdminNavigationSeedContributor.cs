using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Routes;
using LINGYUN.Platform.Utils;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using ValueType = LINGYUN.Platform.Datas.ValueType;

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin
{
    public class VueVbenAdminNavigationSeedContributor : NavigationSeedContributor
    {
        private static int _lastCodeNumber = 0;
        protected ICurrentTenant CurrentTenant { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IRouteDataSeeder RouteDataSeeder { get; }
        protected IDataDictionaryDataSeeder DataDictionaryDataSeeder { get; }
        protected IMenuRepository MenuRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }
        protected AbpUINavigationVueVbenAdminOptions Options { get; }

        public VueVbenAdminNavigationSeedContributor(
            ICurrentTenant currentTenant,
            IRouteDataSeeder routeDataSeeder,
            IMenuRepository menuRepository,
            ILayoutRepository layoutRepository,
            IGuidGenerator guidGenerator,
            IDataDictionaryDataSeeder dataDictionaryDataSeeder,
            IOptions<AbpUINavigationVueVbenAdminOptions> options)
        {
            CurrentTenant = currentTenant;
            GuidGenerator = guidGenerator;
            RouteDataSeeder = routeDataSeeder;
            MenuRepository = menuRepository;
            LayoutRepository = layoutRepository;
            DataDictionaryDataSeeder = dataDictionaryDataSeeder;

            Options = options.Value;
        }

        public override async Task SeedAsync(NavigationSeedContext context)
        {
            var uiDataItem = await SeedUIFrameworkDataAsync(CurrentTenant.Id);

            var layoutData = await SeedLayoutDataAsync(CurrentTenant.Id);

            var layout = await SeedDefaultLayoutAsync(layoutData, uiDataItem);

            var latMenu = await MenuRepository.GetLastMenuAsync();

            if (int.TryParse(CodeNumberGenerator.GetLastCode(latMenu?.Code ?? "0"), out int _lastNumber))
            {
                Interlocked.Exchange(ref _lastCodeNumber, _lastNumber);
            }

            await SeedDefinitionMenusAsync(layout, layoutData, context.Menus, context.MultiTenancySides);
        }

        private async Task SeedDefinitionMenusAsync(
            Layout layout,
            Data data, 
            IReadOnlyCollection<ApplicationMenu> menus,
            MultiTenancySides multiTenancySides)
        {
            foreach (var menu in menus)
            {
                if (!menu.MultiTenancySides.HasFlag(multiTenancySides))
                {
                    continue;
                }

                var menuMeta = new Dictionary<string, object>()
                {
                    { "title", menu.DisplayName },
                    { "icon", menu.Icon ?? "" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                };
                menuMeta.AddIfNotContains(menu.ExtraProperties);

                var seedMenu = await SeedMenuAsync(
                    layout:         layout,
                    data:           data,
                    name:           menu.Name,
                    path:           menu.Url,
                    code:           CodeNumberGenerator.CreateCode(GetNextCode()),
                    component:      layout.Path,
                    displayName:    menu.DisplayName,
                    redirect:       menu.Redirect,
                    description:    menu.Description,
                    parentId:       null,
                    tenantId:       layout.TenantId,
                    meta:           menuMeta,
                    roles:          new string[] { "admin" });

                await SeedDefinitionMenuItemsAsync(layout, data, seedMenu, menu.Items, multiTenancySides);
            }
        }

        private async Task SeedDefinitionMenuItemsAsync(
            Layout layout, 
            Data data, 
            Menu menu, 
            ApplicationMenuList items,
            MultiTenancySides multiTenancySides)
        {
            int index = 1;
            foreach (var item in items)
            {
                if (!item.MultiTenancySides.HasFlag(multiTenancySides))
                {
                    continue;
                }

                var menuMeta = new Dictionary<string, object>()
                {
                    { "title", item.DisplayName },
                    { "icon", item.Icon ?? "" },
                    { "hideTab", false },
                    { "ignoreAuth", false },
                };
                menuMeta.AddIfNotContains(item.ExtraProperties);

                var seedMenu = await SeedMenuAsync(
                    layout: layout,
                    data: data,
                    name: item.Name,
                    path: item.Url,
                    code: CodeNumberGenerator.AppendCode(menu.Code, CodeNumberGenerator.CreateCode(index)),
                    component: item.Component.IsNullOrWhiteSpace() ? layout.Path : item.Component,
                    displayName: item.DisplayName,
                    redirect: item.Redirect,
                    description: item.Description,
                    parentId: menu.Id,
                    tenantId: menu.TenantId,
                    meta: menuMeta,
                    roles: new string[] { "admin" });

                await SeedDefinitionMenuItemsAsync(layout, data, seedMenu, item.Items, multiTenancySides);

                index++;
            }
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

        private async Task<DataItem> SeedUIFrameworkDataAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    "UI Framework",
                    CodeNumberGenerator.CreateCode(10),
                    "UI框架",
                    "UI Framework",
                    null,
                    tenantId,
                    true);

            data.AddItem(
                GuidGenerator,
                Options.UI,
                Options.UI,
                Options.UI,
                ValueType.String,
                Options.UI,
                isStatic: true);

            return data.FindItem(Options.UI);
        }

        private async Task<Layout> SeedDefaultLayoutAsync(Data data, DataItem uiDataItem)
        {
            var layout = await RouteDataSeeder.SeedLayoutAsync(
               Options.LayoutName,
               Options.LayoutPath, // 路由层面已经处理好了,只需要传递LAYOUT可自动引用布局
               Options.LayoutName,
               data.Id,
               uiDataItem.Name,
               "",
               Options.LayoutName,
               data.TenantId
               );

            return layout;
        }

        private async Task<Data> SeedLayoutDataAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    Options.LayoutName,
                    CodeNumberGenerator.CreateCode(10),
                    "Vben Admin 布局约束",
                    "Vben Admin Layout Meta Dictionary",
                    null,
                    tenantId,
                    true);

            data.AddItem(
                GuidGenerator,
                "hideMenu",
                "不在菜单显示",
                "false",
                ValueType.Boolean,
                "当前路由不在菜单显示",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "icon",
                "图标",
                "",
                ValueType.String,
                "图标，也是菜单图标",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "currentActiveMenu",
                "当前激活的菜单",
                "",
                ValueType.String,
                "用于配置详情页时左侧激活的菜单路径",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "ignoreKeepAlive",
                "KeepAlive缓存",
                "false",
                ValueType.Boolean,
                "是否忽略KeepAlive缓存",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "frameSrc",
                "IFrame地址",
                "",
                ValueType.String,
                "内嵌iframe的地址",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "transitionName",
                "路由切换动画",
                "",
                ValueType.String,
                "指定该路由切换的动画名",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "roles",
                "可以访问的角色",
                "",
                ValueType.Array,
                "可以访问的角色，只在权限模式为Role的时候有效",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "title",
                "路由标题",
                "",
                ValueType.String,
                "路由title 一般必填",
                false,
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "carryParam",
                "在tab页显示",
                "false",
                ValueType.Boolean,
                "如果该路由会携带参数，且需要在tab页上面显示。则需要设置为true",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideBreadcrumb",
                "隐藏面包屑",
                "false",
                ValueType.Boolean,
                "隐藏该路由在面包屑上面的显示",
                isStatic: true);
            data.AddItem(
               GuidGenerator,
               "ignoreAuth",
               "忽略权限",
               "false",
               ValueType.Boolean,
               "是否忽略权限，只在权限模式为Role的时候有效",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideChildrenInMenu",
                "隐藏所有子菜单",
                "false",
                ValueType.Boolean,
                "隐藏所有子菜单",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "hideTab",
                "不在标签页显示",
                "false",
                ValueType.Boolean,
                "当前路由不在标签页显示",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "affix",
                "固定标签页",
                "false",
                ValueType.Boolean,
                "是否固定标签页",
                isStatic: true);
            data.AddItem(
                GuidGenerator,
                "frameFormat",
                "格式化IFrame",
                "false",
                ValueType.Boolean,
                "扩展的格式化frame，{token}: 在打开的iframe页面传递token请求头");

            return data;
        }

        private int GetNextCode()
        {
            Interlocked.Increment(ref _lastCodeNumber);
            return _lastCodeNumber;
        }
    }
}
