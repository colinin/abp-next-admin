using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.Routes
{
    public class RouteManager : DomainService
    {
        protected IRouteRepository RouteRepository { get; }
        public RouteManager(
            IRouteRepository routeRepository)
        {
            RouteRepository = routeRepository;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(Route route)
        {
            route.Code = await GetNextChildCodeAsync(route.ParentId);
            await RouteRepository.InsertAsync(route);
        }

        public virtual async Task UpdateAsync(Route route)
        {
            await RouteRepository.UpdateAsync(route);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await RouteRepository.RemoveAllUsersRouteAsync(child);
                await RouteRepository.RemoveAllRolesRouteAsync(child);
                await RouteRepository.DeleteAsync(child);
            }

            var organizationUnit = await RouteRepository.GetAsync(id);

            await RouteRepository.RemoveAllUsersRouteAsync(organizationUnit);
            await RouteRepository.RemoveAllRolesRouteAsync(organizationUnit);
            await RouteRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var route = await RouteRepository.GetAsync(id);
            if (route.ParentId == parentId)
            {
                return;
            }

            var children = await FindChildrenAsync(id, true);

            var oldCode = route.Code;

            route.Code = await GetNextChildCodeAsync(parentId);
            route.ParentId = parentId;

            foreach (var child in children)
            {
                child.Code = Route.AppendCode(route.Code, Route.GetRelativeCode(child.Code, oldCode));
            }
        }

        public virtual async Task RemoveRoleFromRouteAsync(string roleName, Route route)
        {
            await RouteRepository.RemoveRoleRouteAsync(roleName, route);
        }

        public virtual async Task RemoveUserFromRouteAsync(Guid userId, Route route)
        {
            await RouteRepository.RemoveUserRouteAsync(userId, route);
        }

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return Route.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return Route.AppendCode(
                parentCode,
                Route.CreateCode(1)
            );
        }

        public virtual async Task<Route> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await RouteRepository.GetChildrenAsync(parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var ou = await RouteRepository.GetAsync(id);
            return ou?.Code;
        }

        public async Task<List<Route>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await RouteRepository.GetChildrenAsync(parentId);
            }

            if (!parentId.HasValue)
            {
                return await RouteRepository.GetListAsync();
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await RouteRepository.GetAllChildrenWithParentCodeAsync(code, parentId);
        }

        public virtual async Task<bool> IsInRouteAsync(Guid userId, Route route)
        {
            return await RouteRepository.IsInRouteAsync(userId, route);
        }

        public virtual async Task<bool> IsInRouteAsync(string roleName, Route route)
        {
            return await RouteRepository.IsInRouteAsync(roleName, route);
        }
    }
}
