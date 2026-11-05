using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OperationRateLimiting.Dtos;

public class OperationRateLimitingRuleDto
{
    [StringLength(OperationRateLimitingManagementConsts.MaxRuleNameLength)]
    public string? Name { get; set; }

    [Required]
    [StringLength(OperationRateLimitingManagementConsts.MaxAlgorithmNameLength)]
    public string Algorithm { get; set; } = OperationRateLimitingAlgorithms.FixedWindow;

    [Range(1, int.MaxValue)]
    public int DurationSeconds { get; set; } = 60;

    [Range(0, int.MaxValue)]
    public int MaxCount { get; set; } = 1;

    [Range(2, 1000)]
    public int Segments { get; set; } = 10;

    [Range(1, int.MaxValue)]
    public int TokensPerPeriod { get; set; } = 1;

    public OperationRateLimitingPartitionType PartitionType { get; set; } = OperationRateLimitingPartitionType.Parameter;

    [StringLength(OperationRateLimitingManagementConsts.MaxPartitionResolverNameLength)]
    public string? PartitionResolverName { get; set; }

    public bool IsMultiTenancyEnabled { get; set; }

    [StringLength(OperationRateLimitingManagementConsts.MaxErrorCodeLength)]
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Additional algorithm specific values. The standard values (duration, max count,
    /// segments, tokens per period) are mapped from the typed properties by the application service.
    /// </summary>
    public Dictionary<string, string> Properties { get; set; } = new();
}
