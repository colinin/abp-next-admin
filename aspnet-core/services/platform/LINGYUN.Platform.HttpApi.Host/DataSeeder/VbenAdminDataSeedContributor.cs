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
            }
        }

        private async Task<DataItem> SeedUIFrameworkDataAsync(Guid? tenantId)
        {
            var data = await DataDictionaryDataSeeder
                .SeedAsync(
                    "UI Framewark",
                    CodeNumberGenerator.CreateCode(2),
                    "UI框架",
                    "UI Framewark",
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
