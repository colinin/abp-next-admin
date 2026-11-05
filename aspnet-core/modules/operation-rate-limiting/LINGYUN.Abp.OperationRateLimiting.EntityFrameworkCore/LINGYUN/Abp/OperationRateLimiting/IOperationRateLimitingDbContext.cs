using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LINGYUN.Abp.OperationRateLimiting;

public interface IOperationRateLimitingDbContext : IEfCoreDbContext
{
    DbSet<OperationRateLimitingPolicyRecord> OperationRateLimitingPolicies { get; }

    DbSet<OperationRateLimitingRuleRecord> OperationRateLimitingPolicyRules { get; }
}
