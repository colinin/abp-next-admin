using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppAny.Quartz.EntityFrameworkCore.Migrations.MySql
{
    public class QuartzJobDetailEntityTypeConfiguration : IEntityTypeConfiguration<QuartzJobDetail>
    {
        private readonly string? prefix;

        public QuartzJobDetailEntityTypeConfiguration(string? prefix)
        {
            this.prefix = prefix;
        }

        public void Configure(EntityTypeBuilder<QuartzJobDetail> builder)
        {
            builder.ToTable($"{prefix}JOB_DETAILS");

            builder.HasKey(x => new { x.SchedulerName, x.JobName, x.JobGroup });

            builder.Property(x => x.SchedulerName)
              .HasColumnName("SCHED_NAME")
              .HasColumnType("varchar(120)")
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

            builder.Property(x => x.JobClassName)
              .HasColumnName("JOB_CLASS_NAME")
              .HasColumnType("varchar(250)")
              .IsRequired();

            builder.Property(x => x.IsDurable)
              .HasColumnName("IS_DURABLE")
              .HasColumnType("tinyint(1)")
              .IsRequired();

            builder.Property(x => x.IsNonConcurrent)
              .HasColumnName("IS_NONCONCURRENT")
              .HasColumnType("tinyint(1)")
              .IsRequired();

            builder.Property(x => x.IsUpdateData)
              .HasColumnName("IS_UPDATE_DATA")
              .HasColumnType("tinyint(1)")
              .IsRequired();

            builder.Property(x => x.RequestsRecovery)
              .HasColumnName("REQUESTS_RECOVERY")
              .HasColumnType("tinyint(1)")
              .IsRequired();

            builder.Property(x => x.JobData)
              .HasColumnName("JOB_DATA")
              .HasColumnType("blob");

            builder.HasIndex(x => x.RequestsRecovery)
              .HasDatabaseName($"IDX_{prefix}J_REQ_RECOVERY");
        }
    }
}