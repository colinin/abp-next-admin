using Elsa.Persistence.EFCore;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.UserTasks.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.PostgreSql;

public class FixForPostgreSql : IEntityModelCreatingHandler
{
    public void Handle(ElsaDbContextBase dbContext, ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (!dbContext.Database.IsPostgres())
        {
            return;
        }

        if (entityType.ClrType == typeof(UserTaskRecord))
        {
            modelBuilder
                .Entity(entityType.Name)
                .Property(nameof(UserTaskRecord.Tags))
                .HasColumnType("text")
                .HasConversion(new JsonValueConverter<List<string>>());
        }
    }
}
