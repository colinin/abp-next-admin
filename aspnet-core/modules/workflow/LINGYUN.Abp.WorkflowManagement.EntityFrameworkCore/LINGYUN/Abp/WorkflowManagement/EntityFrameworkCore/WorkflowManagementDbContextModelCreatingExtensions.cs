using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore
{
    public static class WorkflowManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureWorkflowManagement(
            this ModelBuilder builder,
            Action<WorkflowManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new WorkflowManagementModelBuilderConfigurationOptions(
                WorkflowManagementDbProperties.DbTablePrefix,
                WorkflowManagementDbProperties.DbSchema
            );
            optionsAction?.Invoke(options);

            builder.Entity<Workflow>(b =>
            {
                b.ToTable(options.TablePrefix + "Definition", options.Schema);

                b.Property(p => p.DisplayName).HasMaxLength(WorkflowConsts.MaxDisplayNameLength);
                b.Property(p => p.Name).HasMaxLength(WorkflowConsts.MaxNameLength).IsRequired();
                b.Property(p => p.Description).HasMaxLength(WorkflowConsts.MaxDescriptionLength);

                b.HasMany(u => u.Datas).WithOne().HasForeignKey(uc => uc.WorkflowId).IsRequired();

                b.ConfigureByConvention();
            });

            builder.Entity<WorkflowData>(b =>
            {
                b.ToTable(options.TablePrefix + "DefinitionData", options.Schema);

                b.Property(p => p.DisplayName).HasMaxLength(WorkflowDataConsts.MaxDisplayNameLength).IsRequired();
                b.Property(p => p.Name).HasMaxLength(WorkflowDataConsts.MaxNameLength).IsRequired();

                b.HasIndex(p => p.WorkflowId);

                b.ConfigureByConvention();
            });

            builder.Entity<StepNode>(b =>
            {
                b.ToTable(options.TablePrefix + "Step", options.Schema);

                b.ConfigureStep();
                b.ConfigureByConvention();
            });

            builder.Entity<CompensateNode>(b =>
            {
                b.ToTable(options.TablePrefix + "Compensate", options.Schema);

                b.ConfigureStep();
                b.ConfigureByConvention();
            });
        }

        public static void ConfigureStep<TStep>(
            this EntityTypeBuilder<TStep> builder)
            where TStep : Step
        {
            builder.Property(p => p.CancelCondition).HasMaxLength(WorkflowConsts.MaxCancelConditionLength);
            builder.Property(p => p.Name).HasMaxLength(WorkflowConsts.MaxNameLength);
            builder.Property(p => p.StepType).HasMaxLength(WorkflowConsts.MaxStepTypeLength);

            builder.Property(p => p.Inputs)
                .HasConversion(new ExtraPropertiesValueConverter(builder.Metadata.ClrType))
                .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());
            builder.Property(p => p.Outputs)
                .HasConversion(new ExtraPropertiesValueConverter(builder.Metadata.ClrType))
                .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());
            builder.Property(p => p.SelectNextStep)
                .HasConversion(new ExtraPropertiesValueConverter(builder.Metadata.ClrType))
                .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());
        }
    }
}
