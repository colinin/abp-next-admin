using LINGYUN.Abp.PrintDesigner.CustomElements;
using LINGYUN.Abp.PrintDesigner.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;

[DependsOn(
    typeof(AbpPrintDesignerDomainModule),
    typeof(AbpEntityFrameworkCoreModule))]
public class AbpPrintDesignerEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PrintDesignerDbContext>(options =>
        {
            options.AddRepository<CustomElement, EfCoreCustomElementRepository>();
            options.AddRepository<Template, EfCoreTemplateRepository>();

            options.AddDefaultRepositories<IPrintDesignerDbContext>();
        });

        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<CustomElement>(e =>
            {
                e.DefaultWithDetailsFunc = x => x.Include(t => t.Data);
            });

            options.Entity<Template>(e =>
            {
                e.DefaultWithDetailsFunc = x => x.Include(t => t.Data);
            });
        });
    }
}
