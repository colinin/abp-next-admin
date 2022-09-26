using LINGYUN.Platform.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace LINGYUN.Platform.Menus;

[Authorize]
public class UserFavoriteMenuAppService : PlatformApplicationServiceBase, IUserFavoriteMenuAppService
{
    protected IStandardMenuConverter StandardMenuConverter => LazyServiceProvider.LazyGetRequiredService<IStandardMenuConverter>();
    protected IMenuRepository MenuRepository { get; }
    protected IUserFavoriteMenuRepository UserFavoriteMenuRepository { get; }

    public UserFavoriteMenuAppService(
        IMenuRepository menuRepository, 
        IUserFavoriteMenuRepository userFavoriteMenuRepository)
    {
        MenuRepository = menuRepository;
        UserFavoriteMenuRepository = userFavoriteMenuRepository;
    }

    [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
    public async virtual Task<UserFavoriteMenuDto> CreateAsync(Guid userId, UserFavoriteMenuCreateDto input)
    {
        if (!await UserFavoriteMenuRepository.CheckExistsAsync(input.Framework, userId, input.MenuId))
        {
            throw new BusinessException(PlatformErrorCodes.UserDuplicateFavoriteMenu);
        }

        var menu = await MenuRepository.GetAsync(input.MenuId);
        var standardMenu = StandardMenuConverter.Convert(menu);
        var userFavoriteMenu = new UserFavoriteMenu(
            GuidGenerator.Create(),
            input.MenuId,
            userId,
            input.Framework,
            standardMenu.Name,
            standardMenu.DisplayName,
            standardMenu.Path,
            standardMenu.Icon,
            input.Color,
            input.AliasName,
            CurrentTenant.Id);

        userFavoriteMenu = await UserFavoriteMenuRepository.InsertAsync(userFavoriteMenu);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<UserFavoriteMenu, UserFavoriteMenuDto>(userFavoriteMenu);
    }

    public async virtual Task<UserFavoriteMenuDto> CreateMyFavoriteMenuAsync(UserFavoriteMenuCreateDto input)
    {
        var userId = CurrentUser.GetId();
        if (!await UserFavoriteMenuRepository.CheckExistsAsync(input.Framework, userId, input.MenuId))
        {
            throw new BusinessException(PlatformErrorCodes.UserDuplicateFavoriteMenu);
        }

        var menu = await MenuRepository.GetAsync(input.MenuId);
        var standardMenu = StandardMenuConverter.Convert(menu);
        var userFavoriteMenu = new UserFavoriteMenu(
            GuidGenerator.Create(),
            input.MenuId,
            userId,
            input.Framework,
            standardMenu.Name,
            standardMenu.DisplayName,
            standardMenu.Path,
            standardMenu.Icon,
            input.Color,
            input.AliasName,
            CurrentTenant.Id);

        userFavoriteMenu = await UserFavoriteMenuRepository.InsertAsync(userFavoriteMenu);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<UserFavoriteMenu, UserFavoriteMenuDto>(userFavoriteMenu);
    }

    [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
    public async virtual Task DeleteAsync(Guid userId, UserFavoriteMenuRemoveInput input)
    {
        var userFavoriteMenu = await GetUserMenuAsync(userId, input.MenuId);

        await UserFavoriteMenuRepository.DeleteAsync(userFavoriteMenu);
    }

    public async virtual Task DeleteMyFavoriteMenuAsync(UserFavoriteMenuRemoveInput input)
    {
        var userFavoriteMenu = await GetUserMenuAsync(CurrentUser.GetId(), input.MenuId);

        await UserFavoriteMenuRepository.DeleteAsync(userFavoriteMenu);
    }

    [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
    public async virtual Task<ListResultDto<UserFavoriteMenuDto>> GetListAsync(Guid userId, UserFavoriteMenuGetListInput input)
    {
        var userFacoriteMenus = await UserFavoriteMenuRepository.GetFavoriteMenusAsync(
                  userId, input.Framework);

        return new ListResultDto<UserFavoriteMenuDto>(
            ObjectMapper.Map<List<UserFavoriteMenu>, List<UserFavoriteMenuDto>>(userFacoriteMenus));
    }

    public async virtual Task<ListResultDto<UserFavoriteMenuDto>> GetMyFavoriteMenuListAsync(UserFavoriteMenuGetListInput input)
    {
        var userFacoriteMenus = await UserFavoriteMenuRepository.GetFavoriteMenusAsync(
                  CurrentUser.GetId(), input.Framework);

        return new ListResultDto<UserFavoriteMenuDto>(
            ObjectMapper.Map<List<UserFavoriteMenu>, List<UserFavoriteMenuDto>>(userFacoriteMenus));
    }

    [Authorize(PlatformPermissions.Menu.ManageUserFavorites)]
    public async virtual Task<UserFavoriteMenuDto> UpdateAsync(Guid userId, UserFavoriteMenuUpdateDto input)
    {
        var userFavoriteMenu = await GetUserMenuAsync(userId, input.MenuId);

        UpdateByInput(userFavoriteMenu, input);

        userFavoriteMenu = await UserFavoriteMenuRepository.UpdateAsync(userFavoriteMenu);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<UserFavoriteMenu, UserFavoriteMenuDto>(userFavoriteMenu);
    }

    public async virtual Task<UserFavoriteMenuDto> UpdateMyFavoriteMenuAsync(UserFavoriteMenuUpdateDto input)
    {
        var userFavoriteMenu = await GetUserMenuAsync(CurrentUser.GetId(), input.MenuId);

        UpdateByInput(userFavoriteMenu, input);

        userFavoriteMenu = await UserFavoriteMenuRepository.UpdateAsync(userFavoriteMenu);

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<UserFavoriteMenu, UserFavoriteMenuDto>(userFavoriteMenu);
    }

    protected async virtual Task<UserFavoriteMenu> GetUserMenuAsync(Guid userId, Guid menuId)
    {
        var userFavoriteMenu = await UserFavoriteMenuRepository.FindByUserMenuAsync(userId, menuId);

        return userFavoriteMenu ?? throw new BusinessException(PlatformErrorCodes.UserFavoriteMenuNotFound);
    }

    protected virtual void UpdateByInput(UserFavoriteMenu userFavoriteMenu, UserFavoriteMenuCreateOrUpdateDto input)
    {
        if (!string.Equals(userFavoriteMenu.Color, input.Color, StringComparison.CurrentCultureIgnoreCase))
        {
            userFavoriteMenu.Color = Check.Length(input.Color, nameof(UserFavoriteMenuCreateOrUpdateDto.Color), UserFavoriteMenuConsts.MaxColorLength);
        }

        if (!string.Equals(userFavoriteMenu.AliasName, input.AliasName, StringComparison.CurrentCultureIgnoreCase))
        {
            userFavoriteMenu.AliasName = Check.Length(input.AliasName, nameof(UserFavoriteMenuCreateOrUpdateDto.AliasName), UserFavoriteMenuConsts.MaxAliasNameLength);
        }

        if (!string.Equals(userFavoriteMenu.Icon, input.Icon, StringComparison.CurrentCultureIgnoreCase))
        {
            userFavoriteMenu.Icon = Check.Length(input.Icon, nameof(UserFavoriteMenuCreateOrUpdateDto.Icon), UserFavoriteMenuConsts.MaxIconLength);
        }
    }
}
