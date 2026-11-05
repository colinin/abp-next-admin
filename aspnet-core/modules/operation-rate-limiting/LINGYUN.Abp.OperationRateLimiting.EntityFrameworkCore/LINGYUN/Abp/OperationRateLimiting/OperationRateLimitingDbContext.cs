using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.OperationRateLimiting;

/// <summary>
/// Dedicated DbContext of the operation rate limiting module. It can be used by a DbMigrator
/// or by applications that do not want to add the entities to their main DbContext.
/// </summary>
public class OperationRateLimitingDbContext :
    AbpDbContext<OperationRateLimitingDbContext>,
    IOperationRateLimitingDbContext
{
    public DbSet<OperationRateLimitingPolicyRecord> OperationRateLimitingPolicies { get; set; } = default!;

    public DbSet<OperationRateLimitingRuleRecord> OperationRateLimitingPolicyRules { get; set; } = default!;

    public OperationRateLimitingDbContext(DbContextOptions<OperationRateLimitingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureOperationRateLimiting();
    }
}
