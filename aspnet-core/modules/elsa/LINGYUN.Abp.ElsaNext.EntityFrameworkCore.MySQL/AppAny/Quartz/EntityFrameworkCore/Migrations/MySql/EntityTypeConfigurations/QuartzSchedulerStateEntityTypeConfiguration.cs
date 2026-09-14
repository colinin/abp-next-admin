using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzSchedulerStateEntityTypeConfiguration : IEntityTypeConfiguration<QuartzSchedulerState>
    {
        private readonly string? prefix;

        public QuartzSchedulerStateEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzSchedulerState> builder)
        {
            builder.ToTable($"{prefix}SCHEDULER_STATE");

            builder.HasKey(x => new { x.SchedulerName, x.InstanceName });

            builder.Property(x => x.SchedulerName)
              .HasColumnName("SCHED_NAME")
              .HasColumnType("varchar(120)")
              .IsRequired();

            builder.Property(x => x.InstanceName)
              .HasColumnName("INSTANCE_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.LastCheckInTime)
              .HasColumnName("LAST_CHECKIN_TIME")
              .HasColumnType("bigint(19)")
              .IsRequired();

            builder.Property(x => x.CheckInInterval)
              .HasColumnName("CHECKIN_INTERVAL")
              .HasColumnType("bigint(19)")
              .IsRequired();
        }
    }
}