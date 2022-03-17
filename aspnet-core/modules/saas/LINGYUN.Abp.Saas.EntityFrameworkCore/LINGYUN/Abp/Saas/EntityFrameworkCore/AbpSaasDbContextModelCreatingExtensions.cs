using LINGYUN.Abp.Saas.Editions;
using LINGYUN.Abp.Saas.Tenants;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.Saas.EntityFrameworkCore;

public static class AbpSaasDbContextModelCreatingExtensions
{
    public static void ConfigureSaas(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        if (builder.IsTenantOnlyDatabase())
        {
            return;
        }

        builder.Entity<Edition>(b =>
        {
            b.ToTable(AbpSaasDbProperties.DbTablePrefix + "Editions", AbpSaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.DisplayName)
                .HasMaxLength(EditionConsts.MaxDisplayNameLength)
                .IsRequired();

            b.HasIndex(u => u.DisplayName);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Tenant>(b =>
        {
            b.ToTable(AbpSaasDbProperties.DbTablePrefix + "Tenants", AbpSaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

            b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

            b.HasIndex(u => u.Name);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<TenantConnectionString>(b =>
        {
            b.ToTable(AbpSaasDbProperties.DbTablePrefix + "TenantConnectionStrings", AbpSaasDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.HasKey(x => new { x.TenantId, x.Name });

            b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
            b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<SaasDbContext>();
    }
}
