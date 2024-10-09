using LINGYUN.Abp.DataProtection.EntityFrameworkCore;
using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Demo.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDemoDomainModule),
    typeof(AbpDataProtectionEntityFrameworkCoreModule))]
public class AbpDemoEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DemoDbContext>(options =>
        {
            // 
            options.AddRepository<Author, EfCoreAuthorRepository>();
            options.AddRepository<Book, EfCoreBookRepository>();

            options.AddDefaultRepositories();
        });
    }
}
