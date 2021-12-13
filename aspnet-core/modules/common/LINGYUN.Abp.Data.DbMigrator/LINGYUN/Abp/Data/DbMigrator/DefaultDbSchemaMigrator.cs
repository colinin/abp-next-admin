using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.Data.DbMigrator
{
    public class DefaultDbSchemaMigrator : IDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AbpDbConnectionOptions _dbConnectionOptions;

        public DefaultDbSchemaMigrator(
            IServiceProvider serviceProvider,
            IOptions<AbpDbConnectionOptions> dbConnectionOptions)
        {
            _serviceProvider = serviceProvider;
            _dbConnectionOptions = dbConnectionOptions.Value;
        }

        public virtual async Task MigrateAsync<TDbContext>(
            [NotNull] Func<string, DbContextOptionsBuilder<TDbContext>, TDbContext> configureDbContext) 
            where TDbContext : AbpDbContext<TDbContext>
        {
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionStringResolver = _serviceProvider
                .GetRequiredService<IConnectionStringResolver>();

            var connectionString = await connectionStringResolver.ResolveAsync(connectionStringName);
            var defaultConnectionString = _dbConnectionOptions.GetConnectionStringOrNull(connectionStringName);
            // 租户连接字符串与默认连接字符串相同,则不执行迁移脚本
            if (string.Equals(
                connectionString,
                defaultConnectionString,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var dbContextBuilder = new DbContextOptionsBuilder<TDbContext>();
            using var dbContext = configureDbContext(connectionString, dbContextBuilder);

            await dbContext.Database.MigrateAsync();
        }
    }
}
