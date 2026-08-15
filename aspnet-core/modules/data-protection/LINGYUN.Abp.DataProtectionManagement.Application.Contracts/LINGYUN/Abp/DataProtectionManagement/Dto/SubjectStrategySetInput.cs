using LINGYUN.Abp.DataProtection;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.DataProtectionManagement;

public class SubjectStrategySetInput
{
    public bool IsEnabled { get; set; }

    [Required]
    [DynamicStringLength(typeof(SubjectStrategyConsts), nameof(SubjectStrategyConsts.MaxSubjectNameLength))]
    public string SubjectName { get; set; } = default!;

    [Required]
    [DynamicStringLength(typeof(SubjectStrategyConsts), nameof(SubjectStrategyConsts.MaxSubjectNameLength))]
    public string SubjectId { get; set; } = default!;

    public DataAccessStrategy Strategy { get; set; }
}
