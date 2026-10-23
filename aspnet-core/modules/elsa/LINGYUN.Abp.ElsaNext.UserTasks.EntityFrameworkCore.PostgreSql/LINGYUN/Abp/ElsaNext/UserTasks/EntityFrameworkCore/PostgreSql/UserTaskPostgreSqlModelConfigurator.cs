using Elsa.Persistence.EFCore;
using Elsa.UserTasks.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LINGYUN.Abp.ElsaNext.UserTasks.EntityFrameworkCore.PostgreSql;

/// <summary>
/// Npgsql 10 maps primitive collections (<c>List&lt;string&gt;</c>) to PostgreSQL arrays and no longer
/// reads them from a <c>text</c> column. Elsa 3.9 ships its User Tasks migration with a <c>text</c>
/// column for <c>TagsJson</c>, which makes task projection fail when reading the task back.
/// Map the column to <c>text[]</c> so both writes and reads use the array representation.
/// </summary>
public class UserTaskPostgreSqlModelConfigurator : IEntityModelCreatingHandler
{
    public virtual void Handle(ElsaDbContextBase dbContext, ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (entityType.ClrType != typeof(UserTaskRecord))
        {
            return;
        }

        modelBuilder
            .Entity(entityType.Name)
            .Property(nameof(UserTaskRecord.Tags))
            .HasColumnType("text[]");
    }
}
