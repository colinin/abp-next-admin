using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.BlobManagement.EntityFrameworkCore;

[ConnectionStringName(BlobManagementDbProperties.ConnectionStringName)]
public interface IBlobManagementDbContext : IEfCoreDbContext
{
    DbSet<BlobContainer> BlobContainers { get; }
    DbSet<Blob> Blobs { get; }
}
