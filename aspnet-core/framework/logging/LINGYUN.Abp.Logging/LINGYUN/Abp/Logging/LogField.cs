using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.Logging;

public class LogField
{
    [JsonPropertyName("UniqueId")]
    public string? Id { get; set; }

    [JsonPropertyName(AbpLoggingEnricherPropertyNames.MachineName)]
    public string? MachineName { get; set; }

    [JsonPropertyName(AbpLoggingEnricherPropertyNames.EnvironmentName)]
    public string? Environment { get; set; }

    [JsonPropertyName("ApplicationName")]
    public string? Application { get; set; }

    [JsonPropertyName("SourceContext")]
    public string? Context { get; set; }

    [JsonPropertyName("ActionId")]
    public string? ActionId { get; set; }

    [JsonPropertyName("ActionName")]
    public string? ActionName { get; set; }

    [JsonPropertyName("RequestId")]
    public string? RequestId { get; set; }

    [JsonPropertyName("RequestPath")]
    public string? RequestPath { get; set; }

    [JsonPropertyName("ConnectionId")]
    public string? ConnectionId { get; set; }

    [JsonPropertyName("CorrelationId")]
    public string? CorrelationId { get; set; }

    [JsonPropertyName("ClientId")]
    public string? ClientId { get; set; }

    [JsonPropertyName("UserId")]
    public string? UserId { get; set; }

    [JsonPropertyName("TenantId")]
    public Guid? TenantId { get; set; }

    [JsonPropertyName("ProcessId")]
    public int? ProcessId { get; set; }

    [JsonPropertyName("ThreadId")]
    public int? ThreadId { get; set; }
}
