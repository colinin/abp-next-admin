using LINGYUN.Abp.DataProtection;

namespace LINGYUN.Abp.DataProtectionManagement;

public class SubjectStrategyDto
{
    public bool IsEnabled { get; set; }
    public string SubjectName { get; set; } = default!;
    public string SubjectId { get; set; } = default!;
    public DataAccessStrategy Strategy { get; set; }
}
