using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.Menus;

public class MenuChangeHandler : 
    IDistributedEventHandler<EntityUpdatedEto<MenuEto>>,
    IDistributedEventHandler<EntityDeletedEto<MenuEto>>,
    IDistributedEventHandler<EntityDeletedEto<UserMenuEto>>,
    ITransientDependency
{
    private readonly IMenuRepository _menuRepository;
    private readonly IStandardMenuConverter _standardMenuConverter;
    private readonly IUserFavoriteMenuRepository _userFavoriteMenuRepository;

    public MenuChangeHandler(
        IMenuRepository menuRepository,
        IStandardMenuConverter standardMenuConverter,
        IUserFavoriteMenuRepository userFavoriteMenuRepository)
    {
        _menuRepository = menuRepository;
        _standardMenuConverter = standardMenuConverter;
        _userFavoriteMenuRepository = userFavoriteMenuRepository;
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityUpdatedEto<MenuEto> eventData)
    {
        // 菜单变更同步变更收藏菜单
        var menu = await _menuRepository.GetAsync(eventData.Entity.Id);
        var favoriteMenus = await _userFavoriteMenuRepository.GetListByMenuIdAsync(menu.Id);

        var standardMenu = _standardMenuConverter.Convert(menu);

        foreach (var favoriteMenu in favoriteMenus)
        {
            favoriteMenu.Framework = menu.Framework;

            favoriteMenu.Name = standardMenu.Name;
            favoriteMenu.Path = standardMenu.Path;
            favoriteMenu.Icon = standardMenu.Icon;
            favoriteMenu.DisplayName = standardMenu.DisplayName;
        }

        await _userFavoriteMenuRepository.UpdateManyAsync(favoriteMenus);
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityDeletedEto<MenuEto> eventData)
    {
        // 菜单删除同步删除收藏菜单

        var favoriteMenus = await _userFavoriteMenuRepository.GetListByMenuIdAsync(eventData.Entity.Id);

        await _userFavoriteMenuRepository.DeleteManyAsync(favoriteMenus);
    }

    [UnitOfWork]
    public async virtual Task HandleEventAsync(EntityDeletedEto<UserMenuEto> eventData)
    {
        // 用户菜单删除同步删除收藏菜单

        var favoriteMenus = await _userFavoriteMenuRepository.GetListByMenuIdAsync(
            eventData.Entity.UserId);

        await _userFavoriteMenuRepository.DeleteManyAsync(favoriteMenus);
    }
}
