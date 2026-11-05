using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.OperationRateLimiting.Dtos;

public class UpdateOperationRateLimitingPolicyDto
{
    [StringLength(OperationRateLimitingManagementConsts.MaxDisplayNameLength)]
    public string? DisplayName { get; set; }

    public bool IsEnabled { get; set; } = true;

    [StringLength(OperationRateLimitingManagementConsts.MaxErrorCodeLength)]
    public string? ErrorCode { get; set; }

    [Required]
    [MinLength(1)]
    public List<OperationRateLimitingRuleDto> Rules { get; set; } = new();
}
