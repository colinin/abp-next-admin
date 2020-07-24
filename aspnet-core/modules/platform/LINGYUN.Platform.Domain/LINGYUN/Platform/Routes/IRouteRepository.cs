using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LINGYUN.Platform.Routes
{
    public interface IRouteRepository : IBasicRepository<Route, Guid>
    {
        Task<List<Route>> GetChildrenAsync(
            Guid? parentId,
            CancellationToken cancellationToken = default
        );

        Task<List<Route>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            CancellationToken cancellationToken = default
        );

        Task<Route> GetAsync(
            string displayName,
            CancellationToken cancellationToken = default
        );

        Task<List<Route>> GetPagedListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Route>> GetRolesRouteAsync(
            string roleName,
            CancellationToken cancellationToken = default
        );

        Task<List<Route>> GetUsersRouteAsync(
            Guid userId,
            CancellationToken cancellationToken = default
        );

        Task RemoveAllUsersRouteAsync(
            Route route,
            CancellationToken cancellationToken = default
        );

        Task RemoveUserRouteAsync(
            Guid userId,
            Route route,
            CancellationToken cancellationToken = default
        );

        Task RemoveAllRolesRouteAsync(
            Route route,
            CancellationToken cancellationToken = default
        );

        Task RemoveRoleRouteAsync(
            string roleName,
            Route route,
            CancellationToken cancellationToken = default
        );

        Task<bool> IsInRouteAsync(
            string roleName,
            Route route,
            CancellationToken cancellationToken = default
        );

        Task<bool> IsInRouteAsync(
            Guid userId,
            Route route,
            CancellationToken cancellationToken = default
        );
    }
}
