using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Menus
{
    public interface IMenuAppService : 
        ICrudAppService<
            MenuDto,
            Guid,
            MenuGetListInput,
            MenuCreateDto,
            MenuUpdateDto>
    {
        Task<ListResultDto<MenuDto>> GetAllAsync(MenuGetAllInput input);

        Task<ListResultDto<MenuDto>> GetUserMenuListAsync(MenuGetByUserInput input);

        Task<ListResultDto<MenuDto>> GetRoleMenuListAsync(MenuGetByRoleInput input);

        Task SetUserMenusAsync(UserMenuInput input);

        Task SetUserStartupAsync(Guid id, UserMenuStartupInput input);

        Task SetUserFavoriteMenusAsync(Guid userId, UserFavoriteMenuSetInput input);

        Task SetRoleMenusAsync(RoleMenuInput input);

        Task SetRoleStartupAsync(Guid id, RoleMenuStartupInput input);

        Task<ListResultDto<MenuDto>> GetCurrentUserMenuListAsync(GetMenuInput input);

        Task SetCurrentUserFavoriteMenuListAsync(UserFavoriteMenuSetInput input);

        Task<ListResultDto<UserFavoriteMenuDto>> GetCurrentUserFavoriteMenuListAsync(UserFavoriteMenuGetListInput input);

        Task<ListResultDto<UserFavoriteMenuDto>> GetUserFavoriteMenuListAsync(Guid userId, UserFavoriteMenuGetListInput input);
    }
}
