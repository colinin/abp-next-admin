using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzSimplePropertyTriggerEntityTypeConfiguration : IEntityTypeConfiguration<QuartzSimplePropertyTrigger>
    {
        private readonly string? prefix;

        public QuartzSimplePropertyTriggerEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzSimplePropertyTrigger> builder)
        {
            builder.ToTable($"{prefix}SIMPROP_TRIGGERS");

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

            builder.Property(x => x.StringProperty1)
              .HasColumnName("STR_PROP_1")
              .HasColumnType("varchar(512)");

            builder.Property(x => x.StringProperty2)
              .HasColumnName("STR_PROP_2")
              .HasColumnType("varchar(512)");

            builder.Property(x => x.StringProperty3)
              .HasColumnName("STR_PROP_3")
              .HasColumnType("varchar(512)");

            builder.Property(x => x.IntegerProperty1)
              .HasColumnName("INT_PROP_1")
              .HasColumnType("int");

            builder.Property(x => x.IntegerProperty2)
              .HasColumnName("INT_PROP_2")
              .HasColumnType("int");

            builder.Property(x => x.LongProperty1)
              .HasColumnName("LONG_PROP_1")
              .HasColumnType("BIGINT");

            builder.Property(x => x.LongProperty2)
              .HasColumnName("LONG_PROP_2")
              .HasColumnType("BIGINT");

            builder.Property(x => x.DecimalProperty1)
              .HasColumnName("DEC_PROP_1")
              .HasColumnType("NUMERIC(13,4)");

            builder.Property(x => x.DecimalProperty2)
              .HasColumnName("DEC_PROP_2")
              .HasColumnType("NUMERIC(13,4)");

            builder.Property(x => x.BooleanProperty1)
              .HasColumnName("BOOL_PROP_1")
              .HasColumnType("tinyint(1)");

            builder.Property(x => x.BooleanProperty2)
              .HasColumnName("BOOL_PROP_2")
              .HasColumnType("tinyint(1)");

            builder.Property(x => x.TimeZoneId)
              .HasColumnName("TIME_ZONE_ID")
              .HasColumnType("varchar(80)");

            builder.HasOne(x => x.Trigger)
              .WithMany(x => x.SimplePropertyTriggers)
              .HasForeignKey(x => new { x.SchedulerName, x.TriggerName, x.TriggerGroup })
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}