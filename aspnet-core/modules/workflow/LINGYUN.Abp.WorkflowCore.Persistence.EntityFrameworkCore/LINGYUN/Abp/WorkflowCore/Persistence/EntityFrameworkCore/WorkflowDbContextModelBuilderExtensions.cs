using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore
{
    public static class WorkflowDbContextModelBuilderExtensions
    {
        public static void ConfigureWorkflow([NotNull] this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<PersistedWorkflow>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "Workflow");

                b.Property(p => p.WorkflowDefinitionId).HasMaxLength(200);
                b.Property(p => p.Description).HasMaxLength(500);
                b.Property(p => p.Reference).HasMaxLength(200);

                b.ConfigureByConvention();

                b.HasIndex(p => p.NextExecution);
            });

            builder.Entity<PersistedExecutionPointer>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "ExecutionPointer");

                b.Property(p => p.Id).HasMaxLength(50);
                b.Property(p => p.EventName).HasMaxLength(200);
                b.Property(p => p.EventKey).HasMaxLength(200);
                b.Property(p => p.StepName).HasMaxLength(100);
                b.Property(p => p.PredecessorId).HasMaxLength(100);

                b.ConfigureByConvention();
            });

            builder.Entity<PersistedExtensionAttribute>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "ExtensionAttribute");

                b.Property(p => p.Key).HasMaxLength(100);
                b.Property(p => p.ExecutionPointerId).HasMaxLength(50);

                b.ConfigureByConvention();
            });

            builder.Entity<PersistedEvent>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "Event");

                b.Property(p => p.EventName).HasMaxLength(200);
                b.Property(p => p.EventKey).HasMaxLength(200);

                b.ConfigureByConvention();

                b.HasIndex(x => new { x.EventName, x.EventKey });
                b.HasIndex(x => x.CreationTime);
                b.HasIndex(x => x.IsProcessed);
            });

            builder.Entity<PersistedSubscription>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "Subscription");

                b.Property(p => p.ExecutionPointerId).HasMaxLength(50);
                b.Property(p => p.EventName).HasMaxLength(200);
                b.Property(p => p.EventKey).HasMaxLength(200);
                b.Property(p => p.ExternalToken).HasMaxLength(200);
                b.Property(p => p.ExternalWorkerId).HasMaxLength(200);

                b.ConfigureByConvention();

                b.HasIndex(x => x.EventName);
                b.HasIndex(x => x.EventKey);
            });

            builder.Entity<PersistedExecutionError>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "ExecutionError");

                b.Property(p => p.ExecutionPointerId).HasMaxLength(50);

                b.ConfigureByConvention();
            });

            builder.Entity<PersistedScheduledCommand>(b =>
            {
                b.ToTable(WorkflowDbProperties.TablePrefix + "ScheduledCommand");

                b.Property(p => p.CommandName).HasMaxLength(200);
                b.Property(p => p.Data).HasMaxLength(500);

                b.ConfigureByConvention();

                b.HasIndex(x => x.ExecuteTime);
                b.HasIndex(x => new { x.CommandName, x.Data }).IsUnique();
            });
        }
    }
}
