using Elsa.AI.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.ElsaNext.EntityFrameworkCore.AI;

/// <summary>
/// AIDbContext 的显式工厂。
/// </summary>
/// <remarks>
/// AIDbContext 同时提供 <c>(DbContextOptions&lt;AIDbContext&gt;)</c> 与
/// <c>(DbContextOptions&lt;AIDbContext&gt;, IServiceProvider)</c> 两个公开构造函数，
/// 而 EF Core 的 <c>AddDbContextFactory&lt;TDbContext&gt;</c> 经 ActivatorUtilities 解析构造函数时
/// 会抛出 <c>Multiple constructors accepting all given argument types have been found</c>，
/// 因此注册时固定使用本工厂：<c>AddDbContextFactory&lt;AIDbContext, AbpAIDbContextFactory&gt;</c>。
/// 传入的 <see cref="IServiceProvider"/> 必须来自当前作用域，AIDbContext 会用它解析多租户信息与实体处理器。
/// </remarks>
public sealed class AbpAIDbContextFactory(DbContextOptions<AIDbContext> options, IServiceProvider serviceProvider)
    : IDbContextFactory<AIDbContext>
{
    /// <inheritdoc />
    public AIDbContext CreateDbContext()
    {
        return new(options, serviceProvider);
    }

    /// <inheritdoc />
    public Task<AIDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CreateDbContext());
    }
}
