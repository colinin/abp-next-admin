using LINGYUN.Abp.Data.DbMigrator;
using LINGYUN.Abp.WorkflowCore.Persistence.EntityFrameworkCore;
using LINGYUN.Abp.WorkflowManagement;
using LINGYUN.Abp.WorkflowManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using WorkflowCore.Interface;

namespace LY.MicroService.WorkflowManagement;

public class WorkflowEngineManager : IWorkflowEngineManager, ISingletonDependency
{
    private readonly IWorkflowHost _workflowHost;
    private readonly IDbSchemaMigrator _dbSchemaMigrator;

    private readonly ILogger<WorkflowEngineManager> _logger;
    public WorkflowEngineManager(
        IWorkflowHost workflowHost,
        IDbSchemaMigrator dbSchemaMigrator,
        ILogger<WorkflowEngineManager> logger)
    {
        _logger = logger;
        _workflowHost = workflowHost;
        _dbSchemaMigrator = dbSchemaMigrator;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Migrating workflow core context...");
        await _dbSchemaMigrator.MigrateAsync<WorkflowDbContext>(
            (connectionString, builder) =>
            {
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                return new WorkflowDbContext(builder.Options);
            });
        _logger.LogInformation("Migrated workflow core context.");

        _logger.LogInformation("Migrating workflow management context...");
        await _dbSchemaMigrator.MigrateAsync<WorkflowManagementDbContext>(
            (connectionString, builder) =>
            {
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                return new WorkflowManagementDbContext(builder.Options);
            });
        _logger.LogInformation("Migrated workflow management context.");
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await _workflowHost.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await _workflowHost.StopAsync(cancellationToken);
    }
}
