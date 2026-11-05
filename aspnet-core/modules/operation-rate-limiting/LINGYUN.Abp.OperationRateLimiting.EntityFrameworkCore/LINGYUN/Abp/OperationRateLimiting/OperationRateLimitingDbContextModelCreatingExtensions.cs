using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.OperationRateLimiting;

public static class OperationRateLimitingDbContextModelCreatingExtensions
{
    /// <summary>
    /// Adds the operation rate limiting tables to a DbContext.
    /// </summary>
    public static void ConfigureOperationRateLimiting(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<OperationRateLimitingPolicyRecord>(entity =>
        {
            entity.ToTable("OperationRateLimitingPolicies");
            entity.ConfigureByConvention();

            entity.Property(policy => policy.Name)
                .IsRequired()
                .HasMaxLength(OperationRateLimitingPolicyRecordConsts.MaxNameLength);

            entity.Property(policy => policy.DisplayName)
                .HasMaxLength(OperationRateLimitingPolicyRecordConsts.MaxDisplayNameLength);

            entity.Property(policy => policy.ErrorCode)
                .HasMaxLength(OperationRateLimitingPolicyRecordConsts.MaxErrorCodeLength);

            entity.HasIndex(policy => new { policy.TenantId, policy.Name }).IsUnique();

            entity.HasMany(policy => policy.Rules)
                .WithOne()
                .HasForeignKey(rule => rule.PolicyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OperationRateLimitingRuleRecord>(entity =>
        {
            entity.ToTable("OperationRateLimitingPolicyRules");
            entity.ConfigureByConvention();

            entity.Property(rule => rule.Algorithm)
                .IsRequired()
                .HasMaxLength(OperationRateLimitingRuleRecordConsts.MaxAlgorithmNameLength);

            entity.Property(rule => rule.Name)
                .HasMaxLength(OperationRateLimitingRuleRecordConsts.MaxNameLength);

            entity.Property(rule => rule.PartitionResolverName)
                .HasMaxLength(OperationRateLimitingRuleRecordConsts.MaxPartitionResolverNameLength);

            entity.Property(rule => rule.ErrorCode)
                .HasMaxLength(OperationRateLimitingRuleRecordConsts.MaxErrorCodeLength);

            entity.HasIndex(rule => new { rule.PolicyId, rule.SortOrder });

            var propertiesComparer = new ValueComparer<Dictionary<string, string>>(
                (left, right) => ReferenceEquals(left, right) ||
                                 (left != null && right != null && left.SequenceEqual(right)),
                properties => properties == null
                    ? 0
                    : properties.Aggregate(
                        0,
                        (hash, property) => HashCode.Combine(hash, property.Key.GetHashCode(), property.Value.GetHashCode())),
                properties => new Dictionary<string, string>(properties, StringComparer.Ordinal));

            entity.Property(rule => rule.Properties)
                .HasConversion(
                    properties => JsonSerializer.Serialize(properties, JsonSerializerOptions.Default),
                    json => JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonSerializerOptions.Default)
                            ?? new Dictionary<string, string>())
                .Metadata.SetValueComparer(propertiesComparer);
        });
    }
}
