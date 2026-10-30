using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzCronTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzCronTrigger>
    {
        private readonly string? prefix;

        public QuartzCronTriggerEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzCronTrigger> builder)
        {
            builder.ToTable($"{prefix}CRON_TRIGGERS");

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

            builder.Property(x => x.CronExpression)
              .HasColumnName("CRON_EXPRESSION")
              .HasColumnType("varchar(120)")
              .IsRequired();

            builder.Property(x => x.TimeZoneId)
              .HasColumnName("TIME_ZONE_ID")
              .HasColumnType("varchar(80)");

            builder.HasOne(x => x.Trigger)
              .WithMany(x => x.CronTriggers)
              .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}