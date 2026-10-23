using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzCalendarEntityTypeConfiguration : IEntityTypeConfiguration<QuartzCalendar>
    {
        private readonly string? prefix;

        public QuartzCalendarEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzCalendar> builder)
        {
            builder.ToTable($"{prefix}CALENDARS");

            builder.HasKey(x => new { x.SchedulerName, x.CalendarName });

            builder.Property(x => x.SchedulerName)
              .HasColumnName("SCHED_NAME")
              .HasColumnType("varchar(120)")
              .IsRequired();

            builder.Property(x => x.CalendarName)
              .HasColumnName("CALENDAR_NAME")
              .HasColumnType("varchar(200)")
              .IsRequired();

            builder.Property(x => x.Calendar)
              .HasColumnName("CALENDAR")
              .HasColumnType("blob")
              .IsRequired();
        }
    }
}