using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzTrigger>
    {
        private readonly string? prefix;

        public QuartzTriggerEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzTrigger> builder)
        {
            builder.ToTable($"{prefix}TRIGGERS");

            builder.HasKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup });

            builder.Property(x => x.SchedulerName)
              .HasColumnName("SCHED_NAME")
              .HasColumnType("varchar(120)")
              .IsRequired();

            builder.Property(x => x.TriggerName)
              .HasColumnName("TRIGGER_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.TriggerGroup)
              .HasColumnName("TRIGGER_GROUP")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.JobName)
              .HasColumnName("JOB_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.JobGroup)
              .HasColumnName("JOB_GROUP")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.Description)
              .HasColumnName("DESCRIPTION")
              .HasColumnType("varchar(250)");

            builder.Property(x => x.NextFireTime)
              .HasColumnName("NEXT_FIRE_TIME")
              .HasColumnType("bigint(19)");

            builder.Property(x => x.PreviousFireTime)
              .HasColumnName("PREV_FIRE_TIME")
              .HasColumnType("bigint(19)");

            builder.Property(x => x.Priority)
              .HasColumnName("PRIORITY")
              .HasColumnType("integer");

            builder.Property(x => x.TriggerState)
              .HasColumnName("TRIGGER_STATE")
              .HasColumnType("varchar(16)")
              .IsRequired();

            builder.Property(x => x.TriggerType)
              .HasColumnName("TRIGGER_TYPE")
              .HasColumnType("varchar(8)")
              .IsRequired();

            builder.Property(x => x.StartTime)
              .HasColumnName("START_TIME")
              .HasColumnType("bigint(19)")
              .IsRequired();

            builder.Property(x => x.EndTime)
              .HasColumnName("END_TIME")
              .HasColumnType("bigint(19)");

            //builder.Property(x => x.MisfireOriginalFireTime)
            //  .HasColumnName("MISFIRE_ORIG_FIRE_TIME")
            //  .HasColumnType("bigint(19)");

            builder.Property(x => x.CalendarName)
              .HasColumnName("CALENDAR_NAME")
              .HasColumnType("varchar(200)");

            builder.Property(x => x.MisfireInstruction)
              .HasColumnName("MISFIRE_INSTR")
              .HasColumnType("smallint(2)");

            builder.Property(x => x.JobData)
              .HasColumnName("JOB_DATA")
              .HasColumnType("blob");

            builder.HasOne(x => x.JobDetail)
              .WithMany(x => x.Triggers)
              .HasForeignKey(x => new { x.SchedulerName, x.JobName, x.JobGroup })
              .IsRequired();

            builder.HasIndex(x => x.NextFireTime)
              .HasDatabaseName($"IDX_{prefix}T_NEXT_FIRE_TIME");

            builder.HasIndex(x => x.TriggerState)
              .HasDatabaseName($"IDX_{prefix}T_STATE");

            builder.HasIndex(x => new { x.NextFireTime, x.TriggerState })
              .HasDatabaseName($"IDX_{prefix}T_NFT_ST");
        }
    }
}