using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Menus;

public interface IUserFavoriteMenuAppService : IApplicationService
{
    Task<UserFavoriteMenuDto> CreateAsync(Guid userId, UserFavoriteMenuCreateDto input);

    Task<UserFavoriteMenuDto> CreateMyFavoriteMenuAsync(UserFavoriteMenuCreateDto input);

    Task<UserFavoriteMenuDto> UpdateAsync(Guid userId, UserFavoriteMenuUpdateDto input);

    Task<UserFavoriteMenuDto> UpdateMyFavoriteMenuAsync(UserFavoriteMenuUpdateDto input);

    Task DeleteAsync(Guid userId, UserFavoriteMenuRemoveInput input);

    Task DeleteMyFavoriteMenuAsync(UserFavoriteMenuRemoveInput input);

    Task<ListResultDto<UserFavoriteMenuDto>> GetMyFavoriteMenuListAsync(UserFavoriteMenuGetListInput input);

    Task<ListResultDto<UserFavoriteMenuDto>> GetListAsync(Guid userId, UserFavoriteMenuGetListInput input);
}
