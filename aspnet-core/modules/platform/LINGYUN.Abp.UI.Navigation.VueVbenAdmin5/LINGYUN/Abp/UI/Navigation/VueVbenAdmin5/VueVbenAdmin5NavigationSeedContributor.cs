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

namespace LINGYUN.Abp.UI.Navigation.VueVbenAdmin5;

public class VueVbenAdmin5NavigationSeedContributor : NavigationSeedContributor
{
    private static int _lastCodeNumber = 0;
    protected ICurrentTenant CurrentTenant { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IRouteDataSeeder RouteDataSeeder { get; }
    protected IDataDictionaryDataSeeder DataDictionaryDataSeeder { get; }
    protected IMenuRepository MenuRepository { get; }
    protected ILayoutRepository LayoutRepository { get; }
    protected AbpUINavigationVueVbenAdmin5Options Options { get; }

    public VueVbenAdmin5NavigationSeedContributor(
        ICurrentTenant currentTenant,
        IRouteDataSeeder routeDataSeeder,
        IMenuRepository menuRepository,
        ILayoutRepository layoutRepository,
        IGuidGenerator guidGenerator,
        IDataDictionaryDataSeeder dataDictionaryDataSeeder,
        IOptions<AbpUINavigationVueVbenAdmin5Options> options)
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

            var menuMeta = new Dictionary<string, object>(menu.ExtraProperties)
            {
                ["icon"] = menu.Icon ?? "",
                ["order"] = menu.Order
            };

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

            var menuMeta = new Dictionary<string, object>(item.ExtraProperties)
            {
                ["icon"] = item.Icon ?? "",
                ["order"] = item.Order
            };

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
        var menuMeta = new Dictionary<string, object>();
        foreach (var item in data.Items)
        {
            menuMeta[item.Name] = item.DefaultValue;
        }
        if (meta != null)
        {
            foreach (var item in meta)
            {
                menuMeta[item.Key] = item.Value;
            }
        }
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
            isPublic,
            menuMeta);

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
        var data = new Data(
            GuidGenerator.Create(),
            "UI Framework",
            CodeNumberGenerator.CreateCode(30),
            "UI框架",
            "UI Framework",
            null,
            tenantId)
        {
            IsStatic = true,
        };

        data.AddItem(
            GuidGenerator,
            Options.UI,
            Options.UI,
            Options.UI,
            ValueType.String,
            Options.UI,
            isStatic: true);

        await DataDictionaryDataSeeder.SeedAsync(data);

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
        var data = new Data(
            GuidGenerator.Create(),
            Options.LayoutName,
            CodeNumberGenerator.CreateCode(40),
            "Vben5 Admin 布局约束",
            "Vben5 Admin模板布局约束",
            null,
            tenantId)
        {
            IsStatic = true,
        };
        data.AddItem(
            GuidGenerator,
            "title",
            "标题",
            "",
            ValueType.String,
            "用于配置页面的标题，会在菜单和标签页中显示。一般会配合国际化使用。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "icon",
            "图标",
            "",
            ValueType.String,
            "用于配置页面的图标，会在菜单和标签页中显示。一般会配合图标库使用，如果是http链接，会自动加载图片。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "activeIcon",
            "激活图标",
            "",
            ValueType.String,
            "用于配置页面的激活图标，会在菜单中显示。一般会配合图标库使用，如果是http链接，会自动加载图片。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "keepAlive",
            "是否开启缓存",
            "true",
            ValueType.Boolean,
            "用于配置页面是否开启缓存，开启后页面会缓存，不会重新加载，仅在标签页启用时有效。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "hideInMenu",
            "是否在菜单中隐藏",
            "false",
            ValueType.Boolean,
            "用于配置页面是否在菜单中隐藏，隐藏后页面不会在菜单中显示。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "hideInTab",
            "是否在标签页中隐藏",
            "false",
            ValueType.Boolean,
            "用于配置页面是否在标签页中隐藏，隐藏后页面不会在标签页中显示。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "hideInBreadcrumb",
            "是否在面包屑中隐藏",
            "false",
            ValueType.Boolean,
            "用于配置页面是否在面包屑中隐藏，隐藏后页面不会在面包屑中显示。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "hideChildrenInMenu",
            "是否隐藏子菜单",
            "false",
            ValueType.Boolean,
            "用于配置页面的子页面是否在菜单中隐藏，隐藏后子页面不会在菜单中显示。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "authority",
            "页面权限",
            "",
            ValueType.Array,
            "用于配置页面的权限，只有拥有对应权限的用户才能访问页面，不配置则不需要权限。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "badge",
            "页面徽标",
            "",
            ValueType.String,
            "用于配置页面的徽标，会在菜单显示。",
            isStatic: true);
        data.AddItem(
           GuidGenerator,
           "badgeType",
           "徽标类型",
           "normal",
           ValueType.String,
           "用于配置页面的徽标类型，dot 为小红点，normal 为文本。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "badgeVariants",
            "徽标颜色",
            "success",
            ValueType.String,
            "用于配置页面的徽标颜色,'default' | 'destructive' | 'primary' | 'success' | 'warning' | string",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "activePath",
            "当前激活的菜单",
            "",
            ValueType.String,
            "用于配置当前激活的菜单，有时候页面没有显示在菜单内，需要激活父级菜单时使用。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "affixTab",
            "是否固定标签页",
            "false",
            ValueType.Boolean,
            "用于配置页面是否固定标签页，固定后页面不可关闭。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "affixTabOrder",
            "固定标签页排序,",
            "0",
            ValueType.Numeic,
            "用于配置页面固定标签页的排序, 采用升序排序。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "iframeSrc",
            "内嵌页面地址",
            "",
            ValueType.String,
            "用于配置内嵌页面的 iframe 地址，设置后会在当前页面内嵌对应的页面。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "ignoreAccess",
            "是否忽略权限",
            "false",
            ValueType.Boolean,
            "用于配置页面是否忽略权限，直接可以访问。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "link",
            "外链跳转路径",
            "",
            ValueType.String,
            "用于配置外链跳转路径，会在新窗口打开。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "maxNumOfOpenTab",
            "标签页最大打开数量",
            "-1",
            ValueType.Numeic,
            "用于配置标签页最大打开数量，设置后会在打开新标签页时自动关闭最早打开的标签页(仅在打开同名标签页时生效)。",
            isStatic: true);
        data.AddItem(
            GuidGenerator,
            "menuVisibleWithForbidden",
            "是否可见菜单无权限",
            "false",
            ValueType.Boolean,
            "用于配置页面在菜单可以看到，但是访问会被重定向到403。");
        data.AddItem(
            GuidGenerator,
            "openInNewWindow",
            "是否在新页面打开",
            "false",
            ValueType.Boolean,
            "设置为 true 时，会在新窗口打开页面。");
        data.AddItem(
            GuidGenerator,
            "order",
            "页面排序",
            "0",
            ValueType.Numeic,
            "用于配置页面的排序，用于路由到菜单排序。注意: 排序仅针对一级菜单有效，二级菜单的排序需要在对应的一级菜单中按代码顺序设置。");
        data.AddItem(
            GuidGenerator,
            "noBasicLayout",
            "是否不使用基础布局",
            "false",
            ValueType.Boolean,
            "用于配置当前路由不使用基础布局，仅在顶级时生效。默认情况下，所有的路由都会被包裹在基础布局中（包含顶部以及侧边等导航部件），如果你的页面不需要这些部件，可以设置 noBasicLayout 为 true。");

        await DataDictionaryDataSeeder.SeedAsync(data);

        return data;
    }

    private int GetNextCode()
    {
        Interlocked.Increment(ref _lastCodeNumber);
        return _lastCodeNumber;
    }
}
