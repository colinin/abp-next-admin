using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpLocalizationManagementDomainModule))]
    public class AbpLocalizationManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<LocalizationDbContext>(options =>
            {
                options.AddRepository<Text, EfCoreTextRepository>();
                options.AddRepository<Language, EfCoreLanguageRepository>();
                options.AddRepository<Resource, EfCoreResourceRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
