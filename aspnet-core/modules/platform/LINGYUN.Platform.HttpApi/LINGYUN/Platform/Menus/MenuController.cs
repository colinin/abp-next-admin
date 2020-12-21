using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace LINGYUN.Platform.Menus
{
    [RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
    [Area("platform")]
    [Route("api/platform/menus")]
    public class MenuController : AbpController, IMenuAppService
    {
        protected IMenuAppService MenuAppService { get; }
        protected IUserRoleFinder UserRoleFinder { get; }

        public MenuController(
            IMenuAppService menuAppService,
            IUserRoleFinder userRoleFinder)
        {
            MenuAppService = menuAppService;
            UserRoleFinder = userRoleFinder;
        }

        [HttpGet]
        [Route("by-current-user")]
        public virtual async Task<ListResultDto<MenuDto>> GetCurrentUserMenuListAsync(GetMenuInput input)
        {
            return await MenuAppService.GetCurrentUserMenuListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<MenuDto> GetAsync(Guid id)
        {
            return await MenuAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<ListResultDto<MenuDto>> GetAllAsync(MenuGetAllInput input)
        {
            return await MenuAppService.GetAllAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
        {
            return await MenuAppService.GetListAsync(input);
        }

        [HttpPost]
        public virtual async Task<MenuDto> CreateAsync(MenuCreateDto input)
        {
            return await MenuAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<MenuDto> UpdateAsync(Guid id, MenuUpdateDto input)
        {
            return await MenuAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await MenuAppService.DeleteAsync(id);
        }

        [HttpPut]
        [Route("by-user")]
        public virtual async Task SetUserMenusAsync(UserMenuInput input)
        {
            await MenuAppService.SetUserMenusAsync(input);
        }

        [HttpGet]
        [Route("by-user")]
        public virtual async Task<ListResultDto<MenuDto>> GetUserMenuListAsync(MenuGetByUserInput input)
        {
            return await MenuAppService.GetUserMenuListAsync(input);
        }

        [HttpGet]
        [Route("by-user/{userId}/{platformType}")]
        public virtual async Task<ListResultDto<MenuDto>> GetUserMenuListAsync(Guid userId, PlatformType platformType)
        {
            var userRoles = await UserRoleFinder.GetRolesAsync(userId);

            var getMenuByUser = new MenuGetByUserInput
            {
                UserId = userId,
                Roles = userRoles,
                PlatformType = platformType
            };
            return await MenuAppService.GetUserMenuListAsync(getMenuByUser);
        }

        [HttpPut]
        [Route("by-role")]
        public virtual async Task SetRoleMenusAsync(RoleMenuInput input)
        {
            await MenuAppService.SetRoleMenusAsync(input);
        }

        [HttpGet]
        [Route("by-role")]
        public virtual async Task<ListResultDto<MenuDto>> GetRoleMenuListAsync(MenuGetByRoleInput input)
        {
            return await MenuAppService.GetRoleMenuListAsync(input);
        }

        [HttpGet]
        [Route("by-role/{role}/{platformType}")]
        public virtual async Task<ListResultDto<MenuDto>> GetRoleMenuListAsync(string role, PlatformType platformType)
        {
            return await MenuAppService.GetRoleMenuListAsync(new MenuGetByRoleInput
            {
                Role = role,
                PlatformType = platformType
            });
        }
    }
}
