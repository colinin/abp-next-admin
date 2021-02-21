using LINGYUN.Platform.Menus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace LINGYUN.Platform
{
    public class PlatformTestsDataBuilder : ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;

        private readonly MenuManager _menuManager;
        private readonly IMenuRepository _menuRepository;

        public PlatformTestsDataBuilder(
            ICurrentTenant currentTenant,
            MenuManager menuManager,
            IMenuRepository menuRepository)
        {
            _currentTenant = currentTenant;

            _menuManager = menuManager;
            _menuRepository = menuRepository;
        }

        [UnitOfWork]
        public async Task BuildAsync()
        {
            var adminMenu = await _menuRepository.FindByNameAsync("admin");

            var saasMenu = await _menuRepository.FindByNameAsync("saas");
            await SetRoleMenusAsync(PlatformTestsConsts.Role1Name, new Guid[] { saasMenu.Id });
            await SetRoleMenusAsync(PlatformTestsConsts.Role2Name, new Guid[] { adminMenu.Id, saasMenu.Id });

            await SetUserMenusAsync(PlatformTestsConsts.User1Id, new Guid[] { saasMenu.Id });


            using (_currentTenant.Change(PlatformTestsConsts.TenantId))
            {
                var tenantAdminMenu = await _menuRepository.FindByNameAsync("admin");
                await SetUserMenusAsync(PlatformTestsConsts.User2Id, new Guid[] { tenantAdminMenu.Id });
                await SetRoleMenusAsync(PlatformTestsConsts.Role1Name, new Guid[] { tenantAdminMenu.Id });
            }
        }

        private async Task SetUserMenusAsync(Guid userId, IEnumerable<Guid> menusIds)
        {
            await _menuManager
                .SetUserMenusAsync(
                    userId,
                    menusIds);
        }

        private async Task SetRoleMenusAsync(string roleName, IEnumerable<Guid> menusIds)
        {
            await _menuManager
                .SetRoleMenusAsync(
                    roleName,
                    menusIds);
        }
    }
}
