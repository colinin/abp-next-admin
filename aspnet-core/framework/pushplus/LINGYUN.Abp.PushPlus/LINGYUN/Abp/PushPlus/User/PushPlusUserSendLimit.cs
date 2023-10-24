namespace LINGYUN.Abp.PushPlus.User;

public enum PushPlusUserSendLimit
{
    /// <summary>
    /// 未限制
    /// </summary>
    None = 1,
    /// <summary>
    /// 短期限制
    /// </summary>
    Short = 2,
    /// <summary>
    /// 永久限制
    /// </summary>
    Permanent = 3,
}
