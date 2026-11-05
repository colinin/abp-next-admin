using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.OperationRateLimiting.Dtos;

public class OperationRateLimitingPolicyDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;

    public string? DisplayName { get; set; }

    public bool IsEnabled { get; set; }

    public string? ErrorCode { get; set; }

    public List<OperationRateLimitingRuleDto> Rules { get; set; } = new();

    public DateTime CreationTime { get; set; }

    public DateTime? LastModificationTime { get; set; }
}
