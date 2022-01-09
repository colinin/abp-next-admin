using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueComparers;
using Volo.Abp.EntityFrameworkCore.ValueConverters;

namespace LINGYUN.Abp.TaskManagement.EntityFrameworkCore;

public static class TaskManagementDbContextModelCreatingExtensions
{
    public static void ConfigureTaskManagement(
        this ModelBuilder builder,
        Action<TaskManagementModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new TaskManagementModelBuilderConfigurationOptions(
            TaskManagementDbProperties.DbTablePrefix,
            TaskManagementDbProperties.DbSchema
        );
        optionsAction?.Invoke(options);

        builder.Entity<BackgroundJobInfo>(b =>
        {
            b.ToTable(options.TablePrefix + "BackgroundJobs", options.Schema);

            b.Property(p => p.Name)
                .HasColumnName(nameof(BackgroundJobInfo.Name))
                .HasMaxLength(BackgroundJobInfoConsts.MaxNameLength)
                .IsRequired();
            b.Property(p => p.Group)
                .HasColumnName(nameof(BackgroundJobInfo.Group))
                .HasMaxLength(BackgroundJobInfoConsts.MaxGroupLength)
                .IsRequired();
            b.Property(p => p.Type)
                .HasColumnName(nameof(BackgroundJobInfo.Type))
                .HasMaxLength(BackgroundJobInfoConsts.MaxTypeLength)
                .IsRequired();
            b.Property(p => p.Cron)
                .HasColumnName(nameof(BackgroundJobInfo.Cron))
                .HasMaxLength(BackgroundJobInfoConsts.MaxCronLength);
            b.Property(p => p.Description)
                .HasColumnName(nameof(BackgroundJobInfo.Description))
                .HasMaxLength(BackgroundJobInfoConsts.MaxDescriptionLength);
            b.Property(p => p.Result)
                .HasColumnName(nameof(BackgroundJobInfo.Result))
                .HasMaxLength(BackgroundJobInfoConsts.MaxResultLength);
            b.Property(p => p.Args)
                .HasColumnName(nameof(BackgroundJobInfo.Args))
                .HasConversion(new ExtraPropertiesValueConverter(b.Metadata.ClrType))
                .Metadata.SetValueComparer(new ExtraPropertyDictionaryValueComparer());

            b.ConfigureByConvention();

            b.HasIndex(p => new { p.Name, p.Group });
        });

        builder.Entity<BackgroundJobLog>(b =>
        {
            b.ToTable(options.TablePrefix + "BackgroundJobLogs", options.Schema);

            b.Property(p => p.JobName)
                .HasColumnName(nameof(BackgroundJobLog.JobName))
                .HasMaxLength(BackgroundJobInfoConsts.MaxNameLength);
            b.Property(p => p.JobGroup)
                .HasColumnName(nameof(BackgroundJobLog.JobGroup))
                .HasMaxLength(BackgroundJobInfoConsts.MaxGroupLength);
            b.Property(p => p.JobType)
                .HasColumnName(nameof(BackgroundJobLog.JobType))
                .HasMaxLength(BackgroundJobInfoConsts.MaxTypeLength);
            b.Property(p => p.Message)
                .HasColumnName(nameof(BackgroundJobLog.Message))
                .HasMaxLength(BackgroundJobLogConsts.MaxMessageLength);
            b.Property(p => p.Exception)
                .HasColumnName(nameof(BackgroundJobLog.Exception))
                .HasMaxLength(BackgroundJobLogConsts.MaxExceptionLength);

            b.ConfigureByConvention();

            b.HasIndex(p => new { p.JobGroup, p.JobName });
        });
    }
}
