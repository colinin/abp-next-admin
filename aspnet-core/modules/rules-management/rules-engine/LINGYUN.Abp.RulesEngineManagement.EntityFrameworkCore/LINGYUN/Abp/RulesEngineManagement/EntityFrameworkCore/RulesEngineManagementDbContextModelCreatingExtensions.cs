using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.RulesEngineManagement.EntityFrameworkCore;

public static class RulesEngineManagementDbContextModelCreatingExtensions
{
    public static void ConfigureRulesEngineManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<ActionRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "Actions", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasMaxLength(ActionRecordConsts.MaxNameLength)
                .IsRequired();

            b.HasIndex(u => new { u.TenantId, u.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<ParamRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "Params", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasMaxLength(ParamRecordConsts.MaxNameLength)
                .IsRequired();
            b.Property(t => t.Expression)
                .HasMaxLength(ParamRecordConsts.MaxExpressionLength)
                .IsRequired();

            b.HasIndex(u => new { u.TenantId, u.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<RuleRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "Rules", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasMaxLength(RuleRecordConsts.MaxNameLength)
                .IsRequired();
            b.Property(t => t.Expression)
                .HasMaxLength(RuleRecordConsts.MaxExpressionLength)
                .IsRequired();
            b.Property(t => t.ErrorMessage)
                .HasMaxLength(RuleRecordConsts.MaxErrorMessageLength)
                .IsRequired();

            b.Property(t => t.Operator)
                .HasMaxLength(RuleRecordConsts.MaxOperatorLength);
            b.Property(t => t.InjectWorkflows)
                .HasMaxLength(RuleRecordConsts.MaxInjectWorkflowsLength);
            b.Property(t => t.SuccessEvent)
                .HasMaxLength(RuleRecordConsts.MaxSuccessEventLength);

            b.HasIndex(u => new { u.TenantId, u.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<WorkflowRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "Workflows", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.Property(t => t.Name)
                .HasMaxLength(WorkflowRecordConsts.MaxNameLength)
                .IsRequired();
            b.Property(t => t.TypeFullName)
                .HasMaxLength(WorkflowRecordConsts.MaxTypeFullNameLength)
                .IsRequired();

            b.Property(t => t.InjectWorkflows)
                .HasMaxLength(WorkflowRecordConsts.MaxInjectWorkflowsLength);

            b.HasIndex(u => u.TypeFullName);
            b.HasIndex(u => new { u.TenantId, u.Name }).IsUnique();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<RuleActionRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "RuleActions", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<RuleParamRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "RuleParams", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<WorkflowParamRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "WorkflowParams", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.ApplyObjectExtensionMappings();
        });
        builder.Entity<WorkflowRuleRecord>(b =>
        {
            b.ToTable(RulesEngineManagementDbPropertites.DbTablePrefix + "WorkflowRules", RulesEngineManagementDbPropertites.DbSchema);

            b.ConfigureByConvention();

            b.ApplyObjectExtensionMappings();
        });

        builder.TryConfigureObjectExtensions<RulesEngineManagementDbContext>();
    }
}
