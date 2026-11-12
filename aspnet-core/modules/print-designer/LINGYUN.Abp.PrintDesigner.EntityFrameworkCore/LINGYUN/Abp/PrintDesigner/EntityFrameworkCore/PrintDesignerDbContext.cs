using LINGYUN.Abp.PrintDesigner.CustomElements;
using LINGYUN.Abp.PrintDesigner.Templates;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.PrintDesigner.EntityFrameworkCore;

[ConnectionStringName(PrintDesignerDbProperties.ConnectionStringName)]
public class PrintDesignerDbContext : AbpDbContext<PrintDesignerDbContext>, IPrintDesignerDbContext
{
    public DbSet<CustomElement> CustomElements { get; set; }
    public DbSet<Template> Templates { get; set; }

    public PrintDesignerDbContext(
        DbContextOptions<PrintDesignerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigurePrintDesigner();
    }
}
