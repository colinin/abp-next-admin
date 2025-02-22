using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Gdpr.EntityFrameworkCore;

[ConnectionStringName(GdprDbProterties.ConnectionStringName)]
public interface IGdprDbContext : IEfCoreDbContext
{
    DbSet<GdprRequest> Requests { get; }
}
