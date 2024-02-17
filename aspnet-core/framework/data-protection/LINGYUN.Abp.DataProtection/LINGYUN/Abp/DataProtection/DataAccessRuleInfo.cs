using System.Collections.Generic;

namespace LINGYUN.Abp.DataProtection;
public class DataAccessRuleInfo
{
    public List<DataAccessRule> Rules { get; }
    public DataAccessRuleInfo(List<DataAccessRule> rules)
    {
        Rules = rules ?? new List<DataAccessRule>();
    }
}
