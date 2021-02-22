using LINGYUN.Platform.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Platform.Menus
{
    public class EfCoreUserMenuRepositoryTests : PlatformEntityFrameworkCoreTestBase
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IUserMenuRepository Repository { get; }
        protected IMenuRepository MenuRepository { get; }
        protected MenuManager MenuManager { get; }

        public EfCoreUserMenuRepositoryTests()
        {
            MenuManager = GetRequiredService<MenuManager>();
            Repository = GetRequiredService<IUserMenuRepository>();
            MenuRepository = GetRequiredService<IMenuRepository>();

            CurrentTenant = GetRequiredService<ICurrentTenant>();
        }

        [Fact]
        public async Task UserHasInMenuAsync_Test()
        {
            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User1Id, "saas")).ShouldBeTrue();
            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "admin")).ShouldBeFalse();

            using (CurrentTenant.Change(PlatformTestsConsts.TenantId))
            {
                (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "admin")).ShouldBeTrue();
            }
        }

        [Fact]
        public async Task SetMemberMenusAsync_Test()
        {
            var adminMenu = await MenuRepository.FindByNameAsync("admin");
            var saasMenu = await MenuRepository.FindByNameAsync("saas");

            await MenuManager.SetUserMenusAsync(PlatformTestsConsts.User1Id, new Guid[] { adminMenu.Id});
            await MenuManager.SetUserMenusAsync(PlatformTestsConsts.User2Id, new Guid[] { adminMenu.Id, saasMenu.Id });

            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User1Id, "admin")).ShouldBeTrue();
            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "admin")).ShouldBeTrue();
            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "saas")).ShouldBeTrue();

            using (CurrentTenant.Change(PlatformTestsConsts.TenantId))
            {
                // 在租户范围不能查询到宿主数据
                (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User1Id, "admin")).ShouldBeFalse();
                (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "saas")).ShouldBeFalse();

                var tenantSaasMenu = await MenuRepository.FindByNameAsync("saas");
                await MenuManager.SetUserMenusAsync(PlatformTestsConsts.User2Id, new Guid[] { tenantSaasMenu.Id });

                (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "admin")).ShouldBeFalse();
                (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "saas")).ShouldBeTrue();
            }
            // 在租户范围内处理了菜单数据不应该影响到宿主
            (await Repository.UserHasInMenuAsync(PlatformTestsConsts.User2Id, "admin")).ShouldBeTrue();
        }
    }
}
