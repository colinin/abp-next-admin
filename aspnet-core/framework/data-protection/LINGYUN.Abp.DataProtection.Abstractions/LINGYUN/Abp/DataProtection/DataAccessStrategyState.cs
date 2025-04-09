using System;

namespace LINGYUN.Abp.DataProtection;

[Serializable]
public class DataAccessStrategyState
{
    /// <summary>
    /// 权限主体
    /// </summary>
    public string SubjectName { get; set; }

    /// <summary>
    /// 权限主体标识
    /// </summary>
    public string[] SubjectKeys { get; set; }

    /// <summary>
    /// 权限策略
    /// </summary>
    public DataAccessStrategy Strategy { get; set; }
    public DataAccessStrategyState()
    {

    }
    public DataAccessStrategyState(
        string subjectName,
        string[] subjectKeys,
        DataAccessStrategy strategy)
    {
        SubjectName = subjectName;
        SubjectKeys = subjectKeys;
        Strategy = strategy;
    }
}
