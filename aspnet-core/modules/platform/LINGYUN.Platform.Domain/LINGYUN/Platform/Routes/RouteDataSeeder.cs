using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace LINGYUN.Platform.Routes
{
    public class RouteDataSeeder : IRouteDataSeeder, ITransientDependency
    {
        protected IGuidGenerator GuidGenerator { get; }
        protected ILayoutRepository LayoutRepository { get; }
        protected IMenuRepository MenuRepository { get; }
        protected IUserMenuRepository UserMenuRepository { get; }
        protected IRoleMenuRepository RoleMenuRepository { get; }

        public RouteDataSeeder(
            IGuidGenerator guidGenerator,
            IMenuRepository menuRepository,
            ILayoutRepository layoutRepository,
            IUserMenuRepository userMenuRepository,
            IRoleMenuRepository roleMenuRepository)
        {
            GuidGenerator = guidGenerator;
            MenuRepository = menuRepository;
            LayoutRepository = layoutRepository;
            UserMenuRepository = userMenuRepository;
            RoleMenuRepository = roleMenuRepository;
        }

        public virtual async Task<Layout> SeedLayoutAsync(
            string name, 
            string path, 
            string displayName,
            Guid dataId, 
            PlatformType platformType = PlatformType.None, 
            string redirect = "", 
            string description = "",
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            var layout = await LayoutRepository.FindByNameAsync(name, cancellationToken: cancellationToken);
            if (layout == null)
            {
                layout = new Layout(
                    GuidGenerator.Create(),
                    path,
                    name,
                    displayName,
                    dataId,
                    platformType,
                    redirect,
                    description,
                    tenantId);
                layout = await LayoutRepository.InsertAsync(layout, cancellationToken: cancellationToken);
            }
            return layout;
        }

        public virtual async Task<Menu> SeedMenuAsync(
            Layout layout,
            string name,
            string path, 
            string code,
            string component,
            string displayName,
            string redirect = "", 
            string description = "",
            Guid? parentId = null,
            Guid? tenantId = null,
            bool isPublic = false,
            CancellationToken cancellationToken = default)
        {
            if (parentId.HasValue)
            {
                var children = await MenuRepository.GetChildrenAsync(parentId);
                var childMenu = children.FirstOrDefault(x => x.Name == name);
                if (childMenu != null)
                {
                    return childMenu;
                }
            }
            var menu = await MenuRepository.FindByNameAsync(name, cancellationToken: cancellationToken);
            if (menu == null)
            {
                menu = new Menu(
                    GuidGenerator.Create(),
                    layout.Id,
                    path,
                    name,
                    code,
                    component,
                    displayName,
                    redirect,
                    description,
                    layout.PlatformType,
                    parentId,
                    tenantId)
                {
                    IsPublic = isPublic
                };

                menu = await MenuRepository.InsertAsync(menu, cancellationToken: cancellationToken);
            }

            return menu;
        }

        public virtual async Task SeedRoleMenuAsync(
            string roleName,
            Menu menu,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (! await RoleMenuRepository.RoleHasInMenuAsync(roleName, menu.Name, cancellationToken))
            {
                var roleMenu = new RoleMenu(menu.Id, roleName, tenantId);
                await RoleMenuRepository.InsertAsync(roleMenu);

                var childrens = await MenuRepository.GetChildrenAsync(menu.Id);
                foreach (var children in childrens)
                {
                    await SeedRoleMenuAsync(roleName, children, tenantId, cancellationToken);
                }
            }
        }

        public virtual async Task SeedUserMenuAsync(
            Guid userId,
            Menu menu,
            Guid? tenantId = null, 
            CancellationToken cancellationToken = default)
        {
            if (!await UserMenuRepository.UserHasInMenuAsync(userId, menu.Name, cancellationToken))
            {
                var userMenu = new UserMenu(menu.Id, userId, tenantId);
                await UserMenuRepository.InsertAsync(userMenu);

                var childrens = await MenuRepository.GetChildrenAsync(menu.Id);
                foreach (var children in childrens)
                {
                    await SeedUserMenuAsync(userId, children, tenantId, cancellationToken);
                }
            }
        }
    }
}
