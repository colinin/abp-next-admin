using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzFiredTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzFiredTrigger>
    {
        private readonly string? prefix;

        public QuartzFiredTriggerEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzFiredTrigger> builder)
        {
            builder.ToTable($"{prefix}FIRED_TRIGGERS");

            builder.HasKey(x => new { x.SchedulerName, x.EntryId });

            builder.Property(x => x.SchedulerName)
              .HasColumnName("SCHED_NAME")
              .HasColumnType("varchar(120)")
              .IsRequired();

            builder.Property(x => x.EntryId)
              .HasColumnName("ENTRY_ID")
              .HasColumnType("varchar(140)")
              .IsRequired();

            builder.Property(x => x.TriggerName)
              .HasColumnName("TRIGGER_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.TriggerGroup)
              .HasColumnName("TRIGGER_GROUP")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.InstanceName)
              .HasColumnName("INSTANCE_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.FiredTime)
              .HasColumnName("FIRED_TIME")
              .HasColumnType("bigint(19)")
              .IsRequired();

            builder.Property(x => x.ScheduledTime)
              .HasColumnName("SCHED_TIME")
              .HasColumnType("bigint(19)")
              .IsRequired();

            builder.Property(x => x.Priority)
              .HasColumnName("PRIORITY")
              .HasColumnType("integer")
              .IsRequired();

            builder.Property(x => x.State)
              .HasColumnName("STATE")
              .HasColumnType("varchar(16)")
              .IsRequired();

            builder.Property(x => x.JobName)
              .HasColumnName("JOB_NAME")
              .HasColumnType("varchar(200)");

            builder.Property(x => x.JobGroup)
              .HasColumnName("JOB_GROUP")
              .HasColumnType("varchar(200)");

            builder.Property(x => x.IsNonConcurrent)
              .HasColumnName("IS_NONCONCURRENT")
              .HasColumnType("tinyint(1)")
              .IsRequired();

            builder.Property(x => x.RequestsRecovery)
              .HasColumnName("REQUESTS_RECOVERY")
              .HasColumnType("tinyint(1)");

            builder.HasIndex(x => x.TriggerName)
              .HasDatabaseName($"IDX_{prefix}FT_TRIG_NAME");

            builder.HasIndex(x => x.TriggerGroup)
              .HasDatabaseName($"IDX_{prefix}FT_TRIG_GROUP");

            builder.HasIndex(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
              .HasDatabaseName($"IDX_{prefix}FT_TRIG_NM_GP");

            builder.HasIndex(x => x.InstanceName)
              .HasDatabaseName($"IDX_{prefix}FT_TRIG_INST_NAME");

            builder.HasIndex(x => x.JobName)
              .HasDatabaseName($"IDX_{prefix}FT_JOB_NAME");

            builder.HasIndex(x => x.JobGroup)
              .HasDatabaseName($"IDX_{prefix}FT_JOB_GROUP");

            builder.HasIndex(x => x.RequestsRecovery)
              .HasDatabaseName($"IDX_{prefix}FT_JOB_REQ_RECOVERY");
        }
    }
}