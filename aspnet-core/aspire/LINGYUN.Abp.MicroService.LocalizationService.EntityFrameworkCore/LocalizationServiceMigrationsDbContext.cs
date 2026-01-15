using LINGYUN.Abp.LocalizationManagement;
using LINGYUN.Abp.LocalizationManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.MicroService.LocalizationService;

[ConnectionStringName("Default")]
public class LocalizationServiceMigrationsDbContext : 
    AbpDbContext<LocalizationServiceMigrationsDbContext>,
    ILocalizationDbContext
{
    public DbSet<Resource> Resources { get; set; }

    public DbSet<Language> Languages { get; set; }

    public DbSet<Text> Texts { get; set; }

    public LocalizationServiceMigrationsDbContext(DbContextOptions<LocalizationServiceMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureLocalization();
    }
}
