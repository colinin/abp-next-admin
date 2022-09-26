using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Menus;

public interface IUserFavoriteMenuRepository : IBasicRepository<UserFavoriteMenu, Guid>
{
    Task<bool> CheckExistsAsync(
        string framework,
        Guid userId,
        Guid menuId,
        CancellationToken cancellationToken = default);

    Task<UserFavoriteMenu> FindByUserMenuAsync(
        Guid userId,
        Guid menuId,
        CancellationToken cancellationToken = default);

    Task<List<UserFavoriteMenu>> GetListByMenuIdAsync(
        Guid menuId,
        CancellationToken cancellationToken = default);

    Task<List<UserFavoriteMenu>> GetFavoriteMenusAsync(
        Guid userId,
        string framework = null,
        Guid? menuId = null,
            CancellationToken cancellationToken = default);
}
