using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Users;

namespace LINGYUN.Platform.Menus
{
    [Authorize]
    public class MenuAppService : PlatformApplicationServiceBase, IMenuAppService
    {
        protected IStandardMenuConverter StandardMenuConverter => LazyServiceProvider.LazyGetRequiredService<IStandardMenuConverter>();

        protected DataItemMappingOptions DataItemMapping { get; }
        protected MenuManager MenuManager { get; }
        protected IMenuRepository MenuRepository { get; }
        protected IUserMenuRepository UserMenuRepository { get; }
        protected IUserFavoriteMenuRepository UserFavoriteMenuRepository { get; }
        protected IRoleMenuRepository RoleMenuRepository { get; }
        protected IDataRepository DataRepository { get; }
        protected ILayoutRepository LayoutRepository { get; }

        public MenuAppService(
            MenuManager menuManager,
            IMenuRepository menuRepository,
            IDataRepository dataRepository,
            ILayoutRepository layoutRepository,
            IUserMenuRepository userMenuRepository,
            IRoleMenuRepository roleMenuRepository,
            IUserFavoriteMenuRepository userFavoriteMenuRepository,
            IOptions<DataItemMappingOptions> options)
        {
            MenuManager = menuManager;
            MenuRepository = menuRepository;
            DataRepository = dataRepository;
            LayoutRepository = layoutRepository;
            UserMenuRepository = userMenuRepository;
            RoleMenuRepository = roleMenuRepository;
            UserFavoriteMenuRepository = userFavoriteMenuRepository;
            DataItemMapping = options.Value;
        }

        public async virtual Task<ListResultDto<MenuDto>> GetCurrentUserMenuListAsync(GetMenuInput input)
        {
            var myMenus = await MenuRepository.GetUserMenusAsync(
                CurrentUser.GetId(), 
                CurrentUser.Roles,
                input.Framework);

            var menus = ObjectMapper.Map<List<Menu>, List<MenuDto>>(myMenus);

            var startupMenu = await UserMenuRepository.GetStartupMenuAsync(
                CurrentUser.GetId());

            if (startupMenu == null && CurrentUser.Roles.Any())
            {
                startupMenu = await RoleMenuRepository.GetStartupMenuAsync(CurrentUser.Roles);
            }

            if (startupMenu != null)
            {
                var findMenu = menus.FirstOrDefault(x => x.Id.Equals(startupMenu.Id));

                if (findMenu != null)
                {
                    findMenu.Startup = true;
                }
            }
            

            return new ListResultDto<MenuDto>(menus);
        }

        [Authorize(PlatformPermissions.Menu.Default)]
        public async virtual Task<MenuDto> GetAsync(Guid id)
        {
            var menu = await MenuRepository.GetAsync(id);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(PlatformPermissions.Menu.Default)]
        public async virtual Task<ListResultDto<MenuDto>> GetAllAsync(MenuGetAllInput input)
        {
            var menus = await MenuRepository.GetAllAsync(
                input.Filter, input.Sorting, input.Reverse,
                input.Framework, input.ParentId, input.LayoutId);

            return new ListResultDto<MenuDto>(
                ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus));
        }

        [Authorize(PlatformPermissions.Menu.Default)]
        public async virtual Task<PagedResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
        {
            var count = await MenuRepository.GetCountAsync(input.Filter, input.Framework, input.ParentId, input.LayoutId);

            var menus = await MenuRepository.GetListAsync(
                input.Filter, input.Sorting, input.Reverse,
                input.Framework, input.ParentId, input.LayoutId,
                input.SkipCount, input.MaxResultCount);

            return new PagedResultDto<MenuDto>(count,
                ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus));
        }

        [Authorize(PlatformPermissions.Menu.Create)]
        public async virtual Task<MenuDto> CreateAsync(MenuCreateDto input)
        {
            var layout = await LayoutRepository.GetAsync(input.LayoutId);
            var data = await DataRepository.GetAsync(layout.DataId);

            var menu = await MenuManager.CreateAsync(
                layout,
                GuidGenerator.Create(),
                input.Path,
                input.Name,
                input.Component,
                input.DisplayName,
                input.Redirect,
                input.Description,
                input.ParentId,
                CurrentTenant.Id,
                input.IsPublic);

            // 利用布局约定的数据字典来校验必须的路由元数据,元数据的加入是为了适配多端路由
            foreach (var dataItem in data.Items)
            {
                if (!input.Meta.TryGetValue(dataItem.Name, out object meta))
                {
                    if (!dataItem.AllowBeNull)
                    {
                        throw new BusinessException(PlatformErrorCodes.MenuMissingMetadata)
                            .WithData("Name", dataItem.DisplayName)
                            .WithData("DataName", data.DisplayName);
                    }
                    // 是否需要设定默认值
                    menu.SetProperty(dataItem.Name, dataItem.DefaultValue);
                }
                else
                {
                    // 需要检查参数是否有效
                    menu.SetProperty(dataItem.Name, DataItemMapping.MapToString(dataItem.ValueType, meta));
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(PlatformPermissions.Menu.Update)]
        public async virtual Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input)
        {
            var menu = await MenuRepository.GetAsync(id);

            // 利用布局约定的数据字典来校验必须的路由元数据,元数据的加入是为了适配多端路由
            var layout = await LayoutRepository.GetAsync(menu.LayoutId);
            var data = await DataRepository.GetAsync(layout.DataId);
            foreach (var dataItem in data.Items)
            {
                if (!input.Meta.TryGetValue(dataItem.Name, out object meta))
                {
                    if (!dataItem.AllowBeNull)
                    {
                        throw new BusinessException(PlatformErrorCodes.MenuMissingMetadata)
                            .WithData("Name", dataItem.DisplayName)
                            .WithData("DataName", data.DisplayName);
                    }
                    // 是否需要设定默认值?
                    menu.SetProperty(dataItem.Name, dataItem.DefaultValue);
                }
                else
                {
                    // 与现有的数据做对比
                    var menuMeta = menu.GetProperty(dataItem.Name);
                    if (menuMeta != null && menuMeta.Equals(meta))
                    {
                        continue;
                    }
                    // 需要检查参数是否有效
                    menu.SetProperty(dataItem.Name, DataItemMapping.MapToString(dataItem.ValueType, meta));
                }
            }

            if (!string.Equals(menu.Name, input.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.Name = input.Name;
            }
            if (!string.Equals(menu.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.DisplayName = input.DisplayName;
            }
            if (!string.Equals(menu.Description, input.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.Description = input.Description;
            }
            if (!string.Equals(menu.Path, input.Path, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.Path = input.Path;
            }
            if (!string.Equals(menu.Redirect, input.Redirect, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.Redirect = input.Redirect;
            }
            if (!string.Equals(menu.Component, input.Component, StringComparison.InvariantCultureIgnoreCase))
            {
                menu.Component = input.Component;
            }

            menu.ParentId = input.ParentId;
            menu.IsPublic = input.IsPublic;

            await MenuManager.UpdateAsync(menu);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(PlatformPermissions.Menu.Delete)]
        public async virtual Task DeleteAsync(Guid id)
        {
            var childrens = await MenuRepository.GetChildrenAsync(id);
            if (childrens.Any())
            {
                throw new BusinessException(PlatformErrorCodes.DeleteMenuHaveChildren);
            }

            var menu = await MenuRepository.GetAsync(id);
            await MenuRepository.DeleteAsync(menu);
        }

        [Authorize(PlatformPermissions.Menu.ManageUsers)]
        public async virtual Task<ListResultDto<MenuDto>> GetUserMenuListAsync(MenuGetByUserInput input)
        {
            var menus = await MenuRepository.GetUserMenusAsync(input.UserId, input.Roles, input.Framework);

            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);

            var startupMenu = await UserMenuRepository.GetStartupMenuAsync(input.UserId);

            if (startupMenu == null)
            {
                startupMenu = await RoleMenuRepository.GetStartupMenuAsync(input.Roles);
            }

            if (startupMenu != null)
            {
                var findMenu = menuDtos.FirstOrDefault(x => x.Id.Equals(startupMenu.Id));

                if (findMenu != null)
                {
                    findMenu.Startup = true;
                }
            }

            return new ListResultDto<MenuDto>(menuDtos);
        }

        [Authorize(PlatformPermissions.Menu.ManageUsers)]
        public async virtual Task SetUserMenusAsync(UserMenuInput input)
        {
            await MenuManager.SetUserMenusAsync(input.UserId, input.MenuIds);
        }

        [Authorize(PlatformPermissions.Menu.ManageUsers)]
        public async virtual Task SetUserStartupAsync(Guid id, UserMenuStartupInput input)
        {
            await MenuManager.SetUserStartupMenuAsync(input.UserId, id);
        }

        [Authorize(PlatformPermissions.Menu.ManageRoles)]
        public async virtual Task SetRoleMenusAsync(RoleMenuInput input)
        {
            await MenuManager.SetRoleMenusAsync(input.RoleName, input.MenuIds);
        }

        [Authorize(PlatformPermissions.Menu.ManageRoles)]
        public async virtual Task SetRoleStartupAsync(Guid id, RoleMenuStartupInput input)
        {
            await MenuManager.SetRoleStartupMenuAsync(input.RoleName, id);
        }

        [Authorize(PlatformPermissions.Menu.ManageRoles)]
        public async virtual Task<ListResultDto<MenuDto>> GetRoleMenuListAsync(MenuGetByRoleInput input)
        {
            var menus = await MenuRepository.GetRoleMenusAsync(new string[] { input.Role }, input.Framework);

            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);

            var startupMenu = await RoleMenuRepository.GetStartupMenuAsync(new string[] { input.Role });

            if (startupMenu != null)
            {
                var findMenu = menuDtos.FirstOrDefault(x => x.Id.Equals(startupMenu.Id));

                if (findMenu != null)
                {
                    findMenu.Startup = true;
                }
            }

            return new ListResultDto<MenuDto>(menuDtos);
        }

        [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
        public async virtual Task SetUserFavoriteMenusAsync(Guid userId, UserFavoriteMenuSetInput input)
        {
            await SetUserFavoriteMenuListAsync(userId, input);
        }

        public async virtual Task SetCurrentUserFavoriteMenuListAsync(UserFavoriteMenuSetInput input)
        {
            await SetUserFavoriteMenuListAsync(CurrentUser.GetId(), input);
        }

        [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
        public async virtual Task<ListResultDto<UserFavoriteMenuDto>> GetUserFavoriteMenuListAsync(Guid userId, UserFavoriteMenuGetListInput input)
        {
            var userFacoriteMenus = await UserFavoriteMenuRepository.GetFavoriteMenusAsync(
                userId, input.Framework);

            return new ListResultDto<UserFavoriteMenuDto>(
                ObjectMapper.Map<List<UserFavoriteMenu>, List<UserFavoriteMenuDto>>(userFacoriteMenus));
        }

        public async virtual Task<ListResultDto<UserFavoriteMenuDto>> GetCurrentUserFavoriteMenuListAsync(UserFavoriteMenuGetListInput input)
        {
            var userFacoriteMenus = await UserFavoriteMenuRepository.GetFavoriteMenusAsync(
                CurrentUser.GetId(), input.Framework);

            return new ListResultDto<UserFavoriteMenuDto>(
                ObjectMapper.Map<List<UserFavoriteMenu>, List<UserFavoriteMenuDto>>(userFacoriteMenus));
        }

        protected async virtual Task SetUserFavoriteMenuListAsync(Guid userId, UserFavoriteMenuSetInput input)
        {
            var userFacoriteMenus = await UserFavoriteMenuRepository.GetFavoriteMenusAsync(
                userId, input.Framework);

            var wellDeleteMenus = userFacoriteMenus.Where(um => !input.MenuIds.Any(id => id == um.MenuId));
            var wellInsertMenus = input.MenuIds.Where(id => !userFacoriteMenus.Any(um => um.MenuId == id));

            if (wellInsertMenus.Any())
            {
                var insertMenus = new List<UserFavoriteMenu>();
                var menus = await MenuRepository.GetListAsync(wellInsertMenus.Select(id => id));

                foreach (var menu in menus)
                {
                    var standardMenu = StandardMenuConverter.Convert(menu);

                    insertMenus.Add(
                        new UserFavoriteMenu(
                            GuidGenerator.Create(),
                            menu.Id,
                            userId,
                            menu.Framework,
                            standardMenu.Name,
                            standardMenu.DisplayName,
                            standardMenu.Path,
                            standardMenu.Icon,
                            CurrentTenant.Id));
                }

                await UserFavoriteMenuRepository.InsertManyAsync(insertMenus);
            }

            await UserFavoriteMenuRepository.DeleteManyAsync(wellDeleteMenus);

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
