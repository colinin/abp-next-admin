using Elsa.Persistence.EFCore;
using Elsa.UserTasks.Persistence.EFCore;
using Elsa.UserTasks.Persistence.EFCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq;

namespace Elsa.UserTasks.Persistence.EFCore.MySql.Migrations.UserTasks;

[DbContext(typeof(UserTasksElsaDbContext))]
[Migration("20260817120000_Initial")]
public partial class Initial : Migration
{
    private readonly IElsaDbContextSchema schema;
    public Initial(IElsaDbContextSchema schema) => this.schema = schema ?? throw new ArgumentNullException(nameof(schema));

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        UserTasksSchemaMigration.Up(migrationBuilder, schema.Schema);

        // MySQL enforces a 65,535-byte row limit and a 3,072-byte key limit.
        // Keep only indexed string columns short; large display/diagnostic fields
        // retain their original lengths, with long text stored as LONGTEXT.
        var indexedColumns = migrationBuilder.Operations
            .OfType<CreateIndexOperation>()
            .Where(x => x.Table.StartsWith("UserTask", System.StringComparison.Ordinal))
            .SelectMany(x => x.Columns)
            .ToHashSet(System.StringComparer.OrdinalIgnoreCase);

        foreach (var table in migrationBuilder.Operations
            .OfType<CreateTableOperation>()
            .Where(x => x.Name.StartsWith("UserTask", System.StringComparison.Ordinal)))
        {
            foreach (var column in table.Columns)
            {
                if (column.Name is "Summary" or "HealthMessage")
                {
                    column.ColumnType = "longtext";
                }
                else if (column.MaxLength.HasValue && indexedColumns.Contains(column.Name))
                {
                    column.ColumnType = "varchar(96)";
                    column.MaxLength = 96;
                }
            }
        }
    }

    protected override void Down(MigrationBuilder migrationBuilder) => UserTasksSchemaMigration.Down(migrationBuilder, schema.Schema);
}
