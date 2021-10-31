using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.Data.DbMigrator
{
    public interface IDbSchemaMigrator
    {
        Task MigrateAsync<TDbContext>(
            [NotNull] Func<string, DbContextOptionsBuilder<TDbContext>, TDbContext> configureDbContext)
            where TDbContext : AbpDbContext<TDbContext>;
    }
}
