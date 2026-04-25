using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

public static class BlobManagementDbContextModelCreatingExtensions
{
    public static void ConfigureBlobManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<BlobContainer>(b =>
        {
            b.ToTable(BlobManagementDbProperties.DbTablePrefix + "BlobContainers", BlobManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(p => p.Name).IsRequired().HasMaxLength(BlobContainerConsts.MaxNameLength);
            b.Property(p => p.Provider).HasMaxLength(BlobContainerConsts.MaxProviderLength);

            b.HasMany<Blob>().WithOne().HasForeignKey(p => p.ContainerId);

            b.HasIndex(x => new { x.TenantId, x.Name });

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Blob>(b =>
        {
            b.ToTable(BlobManagementDbProperties.DbTablePrefix + "Blobs", BlobManagementDbProperties.DbSchema);

            b.ConfigureByConvention();

            b.Property(p => p.ContainerId).IsRequired();
            b.Property(p => p.Name).IsRequired().HasMaxLength(BlobConsts.MaxNameLength);
            b.Property(p => p.FullName).IsRequired().HasMaxLength(BlobConsts.MaxFullNameLength);
            b.Property(p => p.ContentType).HasMaxLength(BlobConsts.MaxContentTypeLength);
            b.Property(p => p.Provider).HasMaxLength(BlobConsts.MaxProviderLength);

            b.HasOne<BlobContainer>().WithMany().HasForeignKey(p => p.ContainerId);
            b.HasMany(x => x.Blobs).WithOne().HasForeignKey(p => p.ParentId);

            b.HasIndex(x => new { x.TenantId, x.ContainerId, x.FullName });

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<BlobManagementDbContext>();
    }
}
