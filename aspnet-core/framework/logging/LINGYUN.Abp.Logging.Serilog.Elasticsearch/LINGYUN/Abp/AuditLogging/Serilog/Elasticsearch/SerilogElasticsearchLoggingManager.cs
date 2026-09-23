using Elastic.Clients.Elasticsearch;
using Elastic.Transport.Diagnostics.Auditing;
using LINGYUN.Abp.Elasticsearch;
using LINGYUN.Linq.Dynamic.Queryable;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Logging.Serilog.Elasticsearch;

[Dependency(ReplaceServices = true)]
public class SerilogElasticsearchLoggingManager : ILoggingManager, ISingletonDependency
{
    private readonly static Regex _indexFormatRegex = new Regex(@"^(.*)(?:\{0\:.+\})(.*)$");
    private readonly static Dictionary<Type, Type> _defaultTypeMap = new Dictionary<Type, Type>
    {
        [typeof(LogInfo)] = typeof(SerilogInfo),
        [typeof(LogLevel)] = typeof(string),
        [typeof(LogField)] = typeof(SerilogField),
        [typeof(LogException)] = typeof(SerilogException),
    };

    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly AbpLoggingSerilogElasticsearchOptions _options;
    private readonly IElasticsearchClientFactory _clientFactory;
    private readonly IExpressionQueryService _expressionQueryService;
    private readonly IObjectMapper<AbpLoggingSerilogElasticsearchModule> _objectMapper;

    public ILogger<SerilogElasticsearchLoggingManager> Logger { protected get; set; }

    public SerilogElasticsearchLoggingManager(
        IClock clock,
        ICurrentTenant currentTenant,
        IOptions<AbpLoggingSerilogElasticsearchOptions> options,
        IElasticsearchClientFactory clientFactory,
        IExpressionQueryService expressionQueryService,
        IObjectMapper<AbpLoggingSerilogElasticsearchModule> objectMapper)
    {
        _clock = clock;
        _objectMapper = objectMapper;
        _currentTenant = currentTenant;
        _clientFactory = clientFactory;
        _expressionQueryService = expressionQueryService;
        _options = options.Value;

        Logger = NullLogger<SerilogElasticsearchLoggingManager>.Instance;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<LogInfo> specification,
        CancellationToken cancellationToken = default)
    {
        var converter = new ExpressionQueryConverter<LogInfo, SerilogInfo>(_defaultTypeMap);
        var expression = converter.Convert(specification.ToExpression());

        return await _expressionQueryService.GetCountAsync(
            CreateIndex(), 
            expression,
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
        var converter = new ExpressionQueryConverter<LogInfo, SerilogInfo>(_defaultTypeMap);
        var expression = converter.Convert(specification.ToExpression());
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = ElasticsearchJsonFormatter.TimestampPropertyName;
        }

        var serilogLogs = await _expressionQueryService.GetListAsync(
            CreateIndex(),
            expression,
            sorting,
            maxResultCount,
            skipCount,
            sourceExcludes: includeDetails == false
                ? Fields.FromFields(
                [
                    new Field("exceptions"),
                ])
                : null,
            cancellationToken: cancellationToken);

        return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(serilogLogs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">时间类型或者转换为timestamp都可以查询</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async virtual Task<LogInfo?> GetAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.Create();
        Expression<Func<SerilogInfo, bool>> expression = x => x.Fields.UniqueId == long.Parse(id);
        expression = expression.AndIf(_currentTenant.IsAvailable, x => x.Fields.TenantId == _currentTenant.Id);

        var serilogs = await _expressionQueryService.GetListAsync<SerilogInfo>(
            CreateIndex(),
            x => x.Fields.UniqueId == long.Parse(id),
            sorting: $"{ElasticsearchJsonFormatter.TimestampPropertyName} DESC",
            maxResultCount: 1,
            cancellationToken: cancellationToken);

        return _objectMapper.Map<SerilogInfo?, LogInfo?>(serilogs.FirstOrDefault());
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
        var client = _clientFactory.Create();

        Expression<Func<SerilogInfo, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.TimeStamp >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.TimeStamp <= _clock.Normalize(endTime!.Value))
            .AndIf(level.HasValue, x => x.Level == GetLogEventLevel(level!.Value))
            .AndIf(!machineName.IsNullOrWhiteSpace(), x => x.Fields.MachineName!.Contains(machineName!))
            .AndIf(!environment.IsNullOrWhiteSpace(), x => x.Fields.Environment!.Contains(environment!))
            .AndIf(!application.IsNullOrWhiteSpace(), x => x.Fields.Application!.Contains(application!))
            .AndIf(!context.IsNullOrWhiteSpace(), x => x.Fields.Context == context)
            .AndIf(!requestId.IsNullOrWhiteSpace(), x => x.Fields.RequestId == requestId)
            .AndIf(!requestPath.IsNullOrWhiteSpace(), x => x.Fields.RequestPath!.StartsWith(requestPath!))
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.Fields.CorrelationId!.Contains(correlationId!))
            .AndIf(processId.HasValue, x => x.Fields.ProcessId == processId)
            .AndIf(threadId.HasValue, x => x.Fields.ThreadId == threadId)
            .AndIf(hasException == true, x => x.Exceptions != null)
            .AndIf(hasException == false, x => x.Exceptions == null);

        return await _expressionQueryService.GetCountAsync(
            CreateIndex(),
            expression,
            cancellationToken);
    }

    /// <summary>
    /// 获取日志列表
    /// </summary>
    /// <param name="sorting">排序字段</param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="level"></param>
    /// <param name="machineName"></param>
    /// <param name="environment"></param>
    /// <param name="application"></param>
    /// <param name="context"></param>
    /// <param name="requestId"></param>
    /// <param name="requestPath"></param>
    /// <param name="correlationId"></param>
    /// <param name="processId"></param>
    /// <param name="threadId"></param>
    /// <param name="hasException"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
        var client = _clientFactory.Create();
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = $"{ElasticsearchJsonFormatter.TimestampPropertyName} DESC";
        }
        // 额外处理一下timestamp字段
        else if (sorting.Contains("timestamp", StringComparison.CurrentCultureIgnoreCase))
        {
            sorting = sorting
                .Replace("timestamp", ElasticsearchJsonFormatter.TimestampPropertyName, StringComparison.CurrentCultureIgnoreCase)
                .Replace("@@", "@");
        }

        Expression<Func<SerilogInfo, bool>> expression = _ => true;

        expression = expression
            .AndIf(startTime.HasValue, x => x.TimeStamp >= _clock.Normalize(startTime!.Value))
            .AndIf(endTime.HasValue, x => x.TimeStamp <= _clock.Normalize(endTime!.Value))
            .AndIf(level.HasValue, x => x.Level == GetLogEventLevel(level!.Value))
            .AndIf(!machineName.IsNullOrWhiteSpace(), x => x.Fields.MachineName!.Contains(machineName!))
            .AndIf(!environment.IsNullOrWhiteSpace(), x => x.Fields.Environment!.Contains(environment!))
            .AndIf(!application.IsNullOrWhiteSpace(), x => x.Fields.Application!.Contains(application!))
            .AndIf(!context.IsNullOrWhiteSpace(), x => x.Fields.Context == context)
            .AndIf(!requestId.IsNullOrWhiteSpace(), x => x.Fields.RequestId == requestId)
            .AndIf(!requestPath.IsNullOrWhiteSpace(), x => x.Fields.RequestPath!.StartsWith(requestPath!))
            .AndIf(!correlationId.IsNullOrWhiteSpace(), x => x.Fields.CorrelationId!.Contains(correlationId!))
            .AndIf(processId.HasValue, x => x.Fields.ProcessId == processId)
            .AndIf(threadId.HasValue, x => x.Fields.ThreadId == threadId)
            .AndIf(hasException == true, x => x.Exceptions != null)
            .AndIf(hasException == false, x => x.Exceptions == null);

        var serilogLogs = await _expressionQueryService.GetListAsync(
            CreateIndex(),
            expression,
            sorting: sorting,
            maxResultCount: maxResultCount,
            skipCount: skipCount,
            sourceExcludes: includeDetails == false
                ? Fields.FromFields(
                [
                    new Field("exceptions"),
                ])
                : null,
            cancellationToken: cancellationToken);

        return _objectMapper.Map<List<SerilogInfo>, List<LogInfo>>(serilogLogs);
    }

    protected virtual string CreateIndex(DateTimeOffset? offset = null)
    {
        if (!offset.HasValue)
        {
            return _indexFormatRegex.Replace(_options.IndexFormat, @"$1*$2");
        }
        return string.Format(_options.IndexFormat, offset.Value).ToLowerInvariant();
    }

    protected virtual LogEventLevel GetLogEventLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.None or LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Verbose,
        };
    }
}
