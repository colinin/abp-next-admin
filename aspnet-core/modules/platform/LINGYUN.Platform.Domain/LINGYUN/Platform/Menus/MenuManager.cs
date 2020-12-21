using LINGYUN.Platform.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.Menus
{
    public class MenuManager : DomainService
    {
        protected IMenuRepository MenuRepository { get; }
        protected IUserMenuRepository UserMenuRepository { get; }
        protected IRoleMenuRepository RoleMenuRepository { get; }

        public MenuManager(
            IMenuRepository menuRepository,
            IUserMenuRepository userMenuRepository,
            IRoleMenuRepository roleMenuRepository)
        {
            MenuRepository = menuRepository;
            UserMenuRepository = userMenuRepository;
            RoleMenuRepository = roleMenuRepository;
        }

        [UnitOfWork]
        public virtual async Task<Menu> CreateAsync(
            Guid id,
            Guid layoutId,
            string path,
            string name,
            string component,
            string displayName,
            string redirect = "",
            string description = "",
            PlatformType platformType = PlatformType.None,
            Guid? parentId = null,
            Guid? tenantId = null,
            bool isPublic = false)
        {
            var code = await GetNextChildCodeAsync(parentId);
            if (code.Length > MenuConsts.MaxCodeLength)
            {
                throw new BusinessException(PlatformErrorCodes.MenuAchieveMaxDepth)
                    .WithData("Depth", MenuConsts.MaxDepth);
            }
            var menu = new Menu(
                id,
                layoutId,
                path,
                name,
                code,
                component,
                displayName,
                redirect,
                description,
                platformType,
                parentId,
                tenantId)
            {
                IsPublic = isPublic
            };
            await ValidateMenuAsync(menu);
            await MenuRepository.InsertAsync(menu);

            return menu;
        }

        public virtual async Task UpdateAsync(Menu menu)
        {
            await ValidateMenuAsync(menu);
            await MenuRepository.UpdateAsync(menu);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await MenuRepository.RemoveAllMembersAsync(child);
                await MenuRepository.RemoveAllRolesAsync(child);
                await MenuRepository.DeleteAsync(child);
            }

            var menu = await MenuRepository.GetAsync(id);
            await MenuRepository.RemoveAllMembersAsync(menu);
            await MenuRepository.RemoveAllRolesAsync(menu);

            await MenuRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var menu = await MenuRepository.GetAsync(id);
            if (menu.ParentId == parentId)
            {
                return;
            }

            var children = await FindChildrenAsync(id, true);

            var oldCode = menu.Code;

            menu.Code = await GetNextChildCodeAsync(parentId);
            menu.ParentId = parentId;

            await ValidateMenuAsync(menu);

            foreach (var child in children)
            {
                child.Code = CodeNumberGenerator.AppendCode(menu.Code, CodeNumberGenerator.GetRelativeCode(child.Code, oldCode));
            }
        }

        public virtual async Task SetUserMenusAsync(Guid userId, IEnumerable<Guid> menuIds)
        {
            await UserMenuRepository.SetMemberMenusAsync(userId, menuIds);
        }

        [UnitOfWork]
        public virtual async Task SetRoleMenusAsync(string roleName, IEnumerable<Guid> menuIds)
        {
            await RoleMenuRepository.SetRoleMenusAsync(roleName, menuIds);
        }

        protected virtual async Task ValidateMenuAsync(Menu menu)
        {
            var siblings = (await FindChildrenAsync(menu.ParentId))
                .Where(x => x.Id != menu.Id)
                .ToList();

            if (siblings.Any(ou => ou.Name == menu.Name))
            {
                throw new BusinessException(PlatformErrorCodes.DuplicateMenu)
                    .WithData("Name", menu.Name);
            }
        }

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return CodeNumberGenerator.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return CodeNumberGenerator.AppendCode(
                parentCode,
                CodeNumberGenerator.CreateCode(1)
            );
        }

        public virtual async Task<Menu> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await MenuRepository.GetChildrenAsync(parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public async Task<List<Menu>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await MenuRepository.GetChildrenAsync(parentId);
            }

            if (!parentId.HasValue)
            {
                return await MenuRepository.GetListAsync(includeDetails: true);
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await MenuRepository.GetAllChildrenWithParentCodeAsync(code, parentId);
        }

        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var menu = await MenuRepository.GetAsync(id);
            return menu?.Code;
        }
    }
}
