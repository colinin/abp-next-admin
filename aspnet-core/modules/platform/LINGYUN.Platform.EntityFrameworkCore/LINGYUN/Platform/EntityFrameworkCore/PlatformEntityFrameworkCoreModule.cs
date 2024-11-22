using LINGYUN.Platform.Datas;
using LINGYUN.Platform.Feedbacks;
using LINGYUN.Platform.Layouts;
using LINGYUN.Platform.Menus;
using LINGYUN.Platform.Packages;
using LINGYUN.Platform.Portal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Platform.EntityFrameworkCore;

[DependsOn(
    typeof(PlatformDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class PlatformEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PlatformDbContext>(options =>
        {
            options.AddRepository<Data, EfCoreDataRepository>();
            options.AddRepository<Menu, EfCoreMenuRepository>();
            options.AddRepository<UserMenu, EfCoreUserMenuRepository>();
            options.AddRepository<RoleMenu, EfCoreRoleMenuRepository>();
            options.AddRepository<UserFavoriteMenu, EfCoreUserFavoriteMenuRepository>();
            options.AddRepository<Layout, EfCoreLayoutRepository>();
            options.AddRepository<Package, EfCorePackageRepository>();
            options.AddRepository<Enterprise, EfCoreEnterpriseRepository>();

            options.AddRepository<Feedback, EfCoreFeedbackRepository>();

            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}
