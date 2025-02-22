namespace LINGYUN.Abp.Gdpr;

public static class GdprErrorCodes
{
    public const string Namespace = "Gdpr";
    /// <summary>
    /// 你已经提交了一个个人数据下载请求, 请在 {RequestTimeInterval} 后再次尝试.
    /// </summary>
    public const string PersonalDataRequestAlreadyDays = Namespace + ":010001";
    /// <summary>
    /// 你的个人数据正在准备中, 请在 {ReadyTime} 后再次尝试下载.
    /// </summary>
    public const string DataNotPreparedYet = Namespace + ":010002";
}
