using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzSimpleTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzSimpleTrigger>
    {
        private readonly string? prefix;

        public QuartzSimpleTriggerEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzSimpleTrigger> builder)
        {
            builder.ToTable($"{prefix}SIMPLE_TRIGGERS");

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

            builder.Property(x => x.RepeatCount)
              .HasColumnName("REPEAT_COUNT")
              .HasColumnType("bigint(7)")
              .IsRequired();

            builder.Property(x => x.RepeatInterval)
              .HasColumnName("REPEAT_INTERVAL")
              .HasColumnType("bigint(12)")
              .IsRequired();

            builder.Property(x => x.TimesTriggered)
              .HasColumnName("TIMES_TRIGGERED")
              .HasColumnType("bigint(10)")
              .IsRequired();

            builder.HasOne(x => x.Trigger)
              .WithMany(x => x.SimpleTriggers)
              .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}