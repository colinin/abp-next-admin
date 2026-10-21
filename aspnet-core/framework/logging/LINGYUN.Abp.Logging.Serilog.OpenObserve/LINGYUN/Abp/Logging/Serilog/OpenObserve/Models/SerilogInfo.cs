using Microsoft.Extensions.Logging;
using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging.Serilog.OpenObserve.Models;

[Serializable]
public class SerilogInfo
{
    [JsonPropertyName("_timestamp")]
    public long TimeStamp { get; set; }

    [JsonPropertyName("severity")]
    [JsonConverter(typeof(EnumToStringConverter<LogLevel>))]
    public LogLevel Level { get; set; }

    [JsonPropertyName("body")]
    public string? Message { get; set; }

    [JsonPropertyName("uniqueid")]
    public long? UniqueId { get; set; }

    [JsonPropertyName("machinename")]
    public string? MachineName { get; set; }

    [JsonPropertyName("environmentname")]
    public string? Environment { get; set; }

    [JsonPropertyName("applicationname")]
    public string? Application { get; set; }

    [JsonPropertyName("instrumentation_library_name")]
    public string? Context { get; set; }

    [JsonPropertyName("actionid")]
    public string? ActionId { get; set; }

    [JsonPropertyName("actionname")]
    public string? ActionName { get; set; }

    [JsonPropertyName("requestid")]
    public string? RequestId { get; set; }

    [JsonPropertyName("requestpath")]
    public string? RequestPath { get; set; }

    [JsonPropertyName("connectionid")]
    public string? ConnectionId { get; set; }

    [JsonPropertyName("correlationid")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("clientid")]
    public string? ClientId { get; set; }

    [JsonPropertyName("userid")]
    public string? UserId { get; set; }

    [JsonPropertyName("tenantId")]
    public Guid? TenantId { get; set; }

    [JsonPropertyName("processid")]
    public int? ProcessId { get; set; }

    [JsonPropertyName("threadid")]
    public int? ThreadId { get; set; }

    [JsonPropertyName("exception_message")]
    public string? ExceptionMessage { get; set; }

    [JsonPropertyName("exception_stacktrace")]
    public string? ExceptionStacktrace { get; set; }

    [JsonPropertyName("exception_type")]
    public string? ExceptionType { get; set; }

    [JsonPropertyName("span_id")]
    public string? SpanId { get; set; }

    [JsonPropertyName("trace_id")]
    public string? TraceId { get; set; }

    [JsonPropertyName("telemetry_sdk_language")]
    public string? TelemetrySdkLanguage { get; set; }

    [JsonPropertyName("telemetry_sdk_name")]
    public string? TelemetrySdkName { get; set; }

    [JsonPropertyName("telemetry_sdk_version")]
    public string? TelemetrySdkVersion { get; set; }
}
