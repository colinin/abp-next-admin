using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.RulesManagement.EntityFrameworkCore
{
    public static class RulesManagementDbContextModelBuilderExtensions
    {
        public static void ConfigureRulesManagement(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<RulesManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new RulesManagementModelBuilderConfigurationOptions(
                RulesManagementDbProperties.DbTablePrefix,
                RulesManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<EntityRuleParam>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityRuleParams", options.Schema);

                b.ConfigureMultiTenant();
                b.ConfigureExtraProperties();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(rp => rp.Name).HasMaxLength(EntityRuleParamConsts.MaxNameLength).IsRequired();
                b.Property(rp => rp.Expression).HasMaxLength(EntityRuleParamConsts.MaxExpressionLength).IsRequired();

                b.HasIndex(rp => rp.Name);
            });

            builder.Entity<EntityRule>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityRules", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(r => r.Name).HasMaxLength(EntityRuleConsts.MaxNameLength).IsRequired();
                b.Property(r => r.Expression).HasMaxLength(EntityRuleConsts.MaxExpressionLength).IsRequired();

                b.Property(r => r.Operator).HasMaxLength(EntityRuleConsts.MaxOperatorLength);
                b.Property(r => r.DisplayName).HasMaxLength(EntityRuleConsts.MaxDisplayNameLength);
                b.Property(r => r.ErrorMessage).HasMaxLength(EntityRuleConsts.MaxErrorMessageLength);

                b.HasMany(r => r.SubRules).WithOne().HasForeignKey(r => r.RuleId).IsRequired();
                b.HasMany(r => r.InjectRules).WithOne().HasForeignKey(r => r.InjectId).IsRequired();
                b.HasMany(r => r.Params).WithOne().HasForeignKey(r => r.RuleId).IsRequired();

                b.HasIndex(r => r.Name);
            });

            builder.Entity<EntityRuleGroup>(b =>
            {
                b.ToTable(options.TablePrefix + "EntityRuleGroups", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Id).ValueGeneratedNever();

                b.Property(rg => rg.Name).HasMaxLength(EntityRuleGroupConsts.MaxNameLength).IsRequired();
                b.Property(rg => rg.EntityFullTypeName).HasMaxLength(EntityRuleGroupConsts.MaxEntiyFullTypeNameLength).IsRequired();

                b.Property(rg => rg.DisplayName).HasMaxLength(EntityRuleGroupConsts.MaxDisplayNameLength);

                b.HasMany(rg => rg.Rules).WithOne().HasForeignKey(rg => rg.GroupId).IsRequired();
                b.HasMany(rg => rg.InjectRules).WithOne().HasForeignKey(rg => rg.InjectId).IsRequired();

                b.HasIndex(uc => uc.Name);
            });
        }
    }
}
