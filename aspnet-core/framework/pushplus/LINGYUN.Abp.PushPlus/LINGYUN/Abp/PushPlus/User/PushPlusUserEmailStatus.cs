namespace LINGYUN.Abp.PushPlus.User;

public enum PushPlusUserEmailStatus
{
    /// <summary>
    /// 未验证
    /// </summary>
    None = 0,
    /// <summary>
    /// 待验证
    /// </summary>
    UnConfirmed = 1,
    /// <summary>
    /// 已验证
    /// </summary>
    Confirmed = 2,
}
