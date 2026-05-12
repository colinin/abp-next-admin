using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpBlobManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpBlobManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<BlobManagementDbContext>(options =>
        {
            options.AddRepository<BlobContainer, EfCoreBlobContainerRepository>();
            options.AddRepository<Blob, EfCoreBlobRepository>();
        });

        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<Blob>(orderOptions =>
            {
                orderOptions.DefaultWithDetailsFunc = query => query.Include(o => o.Blobs);
            });
        });
    }
}
