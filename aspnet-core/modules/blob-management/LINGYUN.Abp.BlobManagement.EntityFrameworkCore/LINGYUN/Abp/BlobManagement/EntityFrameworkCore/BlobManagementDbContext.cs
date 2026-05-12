using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

[ConnectionStringName(BlobManagementDbProperties.ConnectionStringName)]
public class BlobManagementDbContext : AbpDbContext<BlobManagementDbContext>, IBlobManagementDbContext
{
    public DbSet<BlobContainer> BlobContainers { get; set; }

    public DbSet<Blob> Blobs { get; set; }

    public BlobManagementDbContext(
        DbContextOptions<BlobManagementDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureBlobManagement();
    }
}
