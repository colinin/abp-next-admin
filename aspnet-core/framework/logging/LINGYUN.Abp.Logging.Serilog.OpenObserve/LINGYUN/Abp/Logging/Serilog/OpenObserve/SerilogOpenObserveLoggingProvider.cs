using LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;
using LINGYUN.Abp.Logging.Serilog.OpenObserve.Utils;
using LINGYUN.Abp.OpenObserve;
using LINGYUN.Abp.OpenObserve.Models.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Specifications;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve;

public class SerilogOpenObserveLoggingProvider : ILoggingProvider
{
    protected IClock Clock { get; }
    protected OpenObserveClient Client { get; }
    protected AbpLoggingSerilogOpenObserveOptions Options { get; }
    public SerilogOpenObserveLoggingProvider(
        IClock clock,
        OpenObserveClient client, 
        IOptions<AbpLoggingSerilogOpenObserveOptions> options)
    {
        Clock = clock;
        Client = client;
        Options = options.Value;
    }

    public async virtual Task<LogInfo?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        if (!long.TryParse(id, out var uniqueId))
        {
            return null;
        }

        var response = await Client.SearchAsync<SerilogInfo>(
            Options.Organization,
            new SearchQuery(
                $"select * from {Options.Stream} where uniqueid = {uniqueId}",
                GetOrDefaultStartTime(),
                Clock.Now),
            cancellationToken: cancellationToken);

        if (response.Hits.Length == 0)
        {
            return null;
        }

        return ConvertSerilogInfoToLogInfo(response.Hits[0]);
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
        var sqlBuilder = new StringBuilder(128);
        sqlBuilder.AppendFormat("select count(*) as total from {0} ", Options.Stream);
        sqlBuilder.AppendFormat(
            "where {0}", 
            BuildSqlCondition(
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
                hasException));

        var response = await Client.SearchAsync<SerilogTotal>(
            Options.Organization,
            new SearchQuery(
                sqlBuilder.ToString(),
                GetOrDefaultStartTime(startTime),
                endTime ?? Clock.Now),
            cancellationToken: cancellationToken);

        if (response.Hits.Length == 0)
        {
            return 0;
        }

        return response.Hits[0].Total;
    }

    public async virtual Task<long> GetCountAsync(
        ISpecification<LogInfo> specification, 
        CancellationToken cancellationToken = default)
    {
        var conditions = ExpressionParser.ExtractFields(specification.ToExpression());

        var sqlBuilder = new StringBuilder(128);
        sqlBuilder.AppendFormat("select count(*) as total from {0} ", Options.Stream);
        sqlBuilder.AppendFormat("where {0}", BuildSqlCondition(conditions));

        var startTime = conditions
            .FirstOrDefault(x => x.Field == "TimeStamp"
                && (x.Op == ComparisonOp.GreaterThanOrEqual || x.Op == ComparisonOp.GreaterThan))
            ?.Value as DateTime?;

        var endTime = conditions
            .FirstOrDefault(x => x.Field == "TimeStamp"
                && (x.Op == ComparisonOp.LessThanOrEqual || x.Op == ComparisonOp.LessThan))
            ?.Value as DateTime?;

        var response = await Client.SearchAsync<SerilogTotal>(
            Options.Organization,
            new SearchQuery(
                sqlBuilder.ToString(),
                GetOrDefaultStartTime(startTime),
                endTime ?? Clock.Now),
            cancellationToken: cancellationToken);

        if (response.Hits.Length == 0)
        {
            return 0;
        }

        return response.Hits[0].Total;
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
        var sqlBuilder = new StringBuilder(128);
        sqlBuilder.AppendFormat("select * from {0} ", Options.Stream);
        sqlBuilder.AppendFormat(
            "where {0}",
            BuildSqlCondition(
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
                hasException));
        if (!sorting.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" order by {0}", sorting);
        }

        var response = await Client.SearchAsync<SerilogInfo>(
            Options.Organization,
            new SearchQuery(
                sqlBuilder.ToString(),
                GetOrDefaultStartTime(startTime),
                endTime ?? Clock.Now,
                skipCount,
                maxResultCount),
            cancellationToken: cancellationToken);

        return response.Hits.Select(ConvertSerilogInfoToLogInfo).ToList();
    }

    public async virtual Task<List<LogInfo>> GetListAsync(
        ISpecification<LogInfo> specification, 
        string? sorting = null,
        int maxResultCount = 50,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var conditions = ExpressionParser.ExtractFields(specification.ToExpression());

        var sqlBuilder = new StringBuilder(128);
        sqlBuilder.AppendFormat("select * from {0} ", Options.Stream);
        sqlBuilder.AppendFormat("where {0}", BuildSqlCondition(conditions));
        if (!sorting.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" order by {0}", sorting);
        }

        var startTime = conditions
            .FirstOrDefault(x => x.Field == "TimeStamp"
                && (x.Op == ComparisonOp.GreaterThanOrEqual || x.Op == ComparisonOp.GreaterThan))
            ?.Value as DateTime?;

        var endTime = conditions
            .FirstOrDefault(x => x.Field == "TimeStamp" 
                && (x.Op == ComparisonOp.LessThanOrEqual || x.Op == ComparisonOp.LessThan))
            ?.Value as DateTime?;

        var response = await Client.SearchAsync<SerilogInfo>(
            Options.Organization,
            new SearchQuery(
                sqlBuilder.ToString(),
                GetOrDefaultStartTime(startTime),
                endTime ?? Clock.Now,
                skipCount,
                maxResultCount),
            cancellationToken: cancellationToken);

        return response.Hits.Select(ConvertSerilogInfoToLogInfo).ToList();
    }

    protected virtual DateTime GetOrDefaultStartTime(DateTime? startTime = null)
    {
        return startTime ?? new DateTime(Clock.Now.Year - 2, 1, 1, 0, 0, 0);
    }

    protected readonly static Dictionary<string, string> MapedFields = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
    {
        { "TimeStamp", "_timestamp" },
        { "Level", "severity" },
        { "Message", "body" },
        { "Fields.Id", "uniqueid" },
        { "Fields.MachineName", "machinename" },
        { "Fields.Environment", "environmentname" },
        { "Fields.Application", "applicationname" },
        { "Fields.Context", "instrumentation_library_name" },
        { "Fields.ActionId", "actionid" },
        { "Fields.ActionName", "actionname" },
        { "Fields.RequestId", "requestid" },
        { "Fields.RequestPath", "requestpath" },
        { "Fields.ConnectionId", "connectionid" },
        { "Fields.CorrelationId", "correlationid" },
        { "Fields.ClientId", "clientid" },
        { "Fields.UserId", "userid" },
        { "Fields.TenantId", "tenantid" },
        { "Fields.ProcessId", "processid" },
        { "Fields.ThreadId", "threadid" },
        { "Exceptions.Message", "exception_message" },
        { "Exceptions.StackTraceString", "exception_stacktrace" },
        { "Exceptions.Class", "exception_type" },
    };

    protected virtual string GetMapedField(string field)
    {
        return MapedFields.GetOrDefault(field) ?? field;
    }

    protected virtual string BuildSqlCondition(List<Condition> conditions)
    {
        string FormatValue(string field, object? value)
        {
            if (field == "Level" && value != null)
            {
                var level = (LogLevel)Enum.ToObject(typeof(LogLevel), value);
                return $"'{EscapeSql(level.ToString())}'";
            }

            return value == null
                ? "NULL"
                : value switch
            {
                string s => $"'{EscapeSql(s)}'",
                bool b => b ? "true" : "false",
                DateTime d => $"'{d:yyyy-MM-dd HH:mm:ss}'",
                Guid g => $"'{g}'",
                _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? "NULL"
            };
        }

        var parts = new List<string>();
        foreach (var c in conditions)
        {
            if (c.Field == "TimeStamp")
            {
                continue;
            }
            var field = GetMapedField(c.Field);
            var value = FormatValue(c.Field, c.Value);
            var sql = c.Op switch
            {
                ComparisonOp.Equal => $"{field} = {value}",
                ComparisonOp.NotEqual => $"{field} != {value}",
                ComparisonOp.GreaterThan => $"{field} > {value}",
                ComparisonOp.GreaterThanOrEqual => $"{field} >= {value}",
                ComparisonOp.LessThan => $"{field} < {value}",
                ComparisonOp.LessThanOrEqual => $"{field} <= {value}",
                ComparisonOp.Contains => $"{field} LIKE '%{EscapeSql(c.Value?.ToString() ?? "")}%'",
                ComparisonOp.StartsWith => $"{field} LIKE '{EscapeSql(c.Value?.ToString() ?? "")}%'",
                ComparisonOp.EndsWith => $"{field} LIKE '%{EscapeSql(c.Value?.ToString() ?? "")}'",
                ComparisonOp.IsNull => $"{field} IS NULL",
                ComparisonOp.IsNotNull => $"{field} IS NOT NULL",
                _ => throw new NotSupportedException()
            };
            parts.Add(sql);
        }
        return parts.Count == 0 ? "1 = 1" : string.Join(" AND ", parts);
    }

    protected virtual string BuildSqlCondition(
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
        bool? hasException = null)
    {
        var sqlBuilder = new StringBuilder(128);
        sqlBuilder.Append(" 1 = 1");
        if (level.HasValue)
        {
            sqlBuilder.AppendFormat(" and severity = '{0}'", level.ToString());
        }
        if (!machineName.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and machinename like '%{0}%'", EscapeSql(machineName));
        }
        if (!environment.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and environmentname like '%{0}%'", EscapeSql(environment));
        }
        if (!application.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and applicationname like '%{0}%'", EscapeSql(application));
        }
        if (!context.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and instrumentation_library_name = '{0}'", EscapeSql(context));
        }
        if (!requestId.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and requestid = '{0}'", EscapeSql(requestId));
        }
        if (!requestPath.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and requestpath like '{0}%'", EscapeSql(requestPath));
        }
        if (!correlationId.IsNullOrWhiteSpace())
        {
            sqlBuilder.AppendFormat(" and correlationid like '%{0}%'", EscapeSql(correlationId));
        }
        if (processId.HasValue)
        {
            sqlBuilder.AppendFormat(" and processid = {0}", processId.Value);
        }
        if (threadId.HasValue)
        {
            sqlBuilder.AppendFormat(" and threadid = {0}", threadId.Value);
        }
        if (hasException == true)
        {
            sqlBuilder.Append(" and exception_type is not null and exception_type != ''");
        }
        if (hasException == false)
        {
            sqlBuilder.Append(" and (exception_type is null or exception_type = '')");
        }
        return sqlBuilder.ToString();
    }

    protected virtual string EscapeSql(string? input)
    {
        return input?.Replace("'", "''") ?? string.Empty;
    }


    protected virtual LogInfo ConvertSerilogInfoToLogInfo(SerilogInfo serilogInfo)
    {
        var epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        return new LogInfo
        {
            TimeStamp = epoch.AddTicks(serilogInfo.TimeStamp * 10).UtcDateTime,
            Level = serilogInfo.Level,
            Message = serilogInfo.Message,
            Fields = new LogField
            {
                ActionId = serilogInfo.ActionId,
                ActionName = serilogInfo.ActionName,
                Application = serilogInfo.Application,
                ClientId = serilogInfo.ClientId,
                ConnectionId = serilogInfo.ConnectionId,
                Context = serilogInfo.Context,
                CorrelationId = serilogInfo.CorrelationId,
                Environment = serilogInfo.Environment,
                Id = serilogInfo.UniqueId?.ToString(),
                MachineName = serilogInfo.MachineName,
                ProcessId = serilogInfo.ProcessId,
                RequestId = serilogInfo.RequestId,
                RequestPath = serilogInfo.RequestPath,
                TenantId = serilogInfo.TenantId,
                ThreadId = serilogInfo.ThreadId,
                UserId = serilogInfo.UserId,
            },
            Exceptions = !serilogInfo.ExceptionType.IsNullOrWhiteSpace()
                ? new List<LogException>
                    {
                        new LogException
                        {
                            Class = serilogInfo.ExceptionType,
                            Message = serilogInfo.ExceptionMessage,
                            StackTrace = serilogInfo.ExceptionStacktrace,
                        }
                    }
                : null,
        };
    }
}
