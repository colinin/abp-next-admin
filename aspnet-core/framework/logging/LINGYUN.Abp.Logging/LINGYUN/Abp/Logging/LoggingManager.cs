using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.Logging;

public class LoggingManager : ILoggingManager, ITransientDependency
{
    protected AbpLoggingOptions Options { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public LoggingManager(
        IOptionsMonitor<AbpLoggingOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        Options = options.CurrentValue;
        ServiceScopeFactory = serviceScopeFactory;
    }

    public async virtual Task<LogInfo?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredKeyedService<ILoggingProvider>(Options.Provider);

        return await provider.GetAsync(id, cancellationToken);
    }

    public async virtual Task<long> GetCountAsync(
        DateTime? startTime = null, 
        DateTime? endTime = null,
        LogLevel? level = null,
        string? machineName = null,
        string? environment = null,
        string? application = null,
        string? context = null, 
        string? requestId = null,
        string? requestPath = null, 
        string? correlationId = null, 
        int? processId = null, 
        int? threadId = null,
        bool? hasException = null,
        CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredKeyedService<ILoggingProvider>(Options.Provider);

        return await provider.GetCountAsync(
            startTime,
            endTime,
            level,
            machineName,
            environment,
            application,
            context,
            requestId,
            requestPath,
            correlationId,
            processId,
            threadId,
            hasException,
            cancellationToken);
    }

    public async virtual Task<List<LogInfo>> GetListAsync(
        string? sorting = null,
        int maxResultCount = 50, 
        int skipCount = 0, 
        DateTime? startTime = null,
        DateTime? endTime = null,
        LogLevel? level = null,
        string? machineName = null,
        string? environment = null,
        string? application = null,
        string? context = null,
        string? requestId = null, 
        string? requestPath = null,
        string? correlationId = null, 
        int? processId = null, 
        int? threadId = null,
        bool? hasException = null,
        bool includeDetails = false, 
        CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredKeyedService<ILoggingProvider>(Options.Provider);

        return await provider.GetListAsync(
            sorting,
            maxResultCount,
            skipCount,
            startTime,
            endTime,
            level,
            machineName,
            environment,
            application,
            context,
            requestId,
            requestPath,
            correlationId,
            processId,
            threadId,
            hasException,
            includeDetails,
            cancellationToken);
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<LogInfo> specification,
        CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredKeyedService<ILoggingProvider>(Options.Provider);

        return await provider.GetCountAsync(
            specification,
            cancellationToken);
    }

    public async virtual Task<List<LogInfo>> GetListAsync(
        ISpecification<LogInfo> specification,
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredKeyedService<ILoggingProvider>(Options.Provider);

        return await provider.GetListAsync(
            specification,
            sorting,
            maxResultCount,
            skipCount,
            includeDetails,
            cancellationToken);
    }
}
