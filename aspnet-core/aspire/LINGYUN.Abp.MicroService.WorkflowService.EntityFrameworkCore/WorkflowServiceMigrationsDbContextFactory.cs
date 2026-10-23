using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LINGYUN.Abp.MicroService.WorkflowService;

/// <summary>
/// EF 设计时上下文工厂。
/// </summary>
/// <remarks>
/// 连接串优先取环境变量 <c>ConnectionStrings__Default</c>，以便 CI / 本地执行 <c>dotnet ef</c> 时覆盖；
/// 未设置时使用与宿主开发环境一致的默认连接串。
/// </remarks>
public class WorkflowServiceMigrationsDbContextFactory : IDesignTimeDbContextFactory<WorkflowServiceMigrationsDbContext>
{
    private const string DefaultConnectionString = "Host=127.0.0.1;Database=abp;Username=postgres;Password=123456";

    public WorkflowServiceMigrationsDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? DefaultConnectionString;

        var builder = new DbContextOptionsBuilder<WorkflowServiceMigrationsDbContext>()
            .UseNpgsql(connectionString);

        return new WorkflowServiceMigrationsDbContext(builder.Options);
    }
}
