namespace LINGYUN.Abp.Tencent.QQ;

public class AbpTencentQQOptions
{
    /// <summary>
    /// 在QQ互联上申请的AppId
    /// </summary>
    /// <remarks>
    /// see: https://wiki.connect.qq.com/%e5%87%86%e5%a4%87%e5%b7%a5%e4%bd%9c_oauth2-0
    /// </remarks>
    public string AppId { get; set; }
    /// <summary>
    /// 在QQ互联上申请的AppKey
    /// </summary>
    /// <remarks>
    /// see: https://wiki.connect.qq.com/%e5%87%86%e5%a4%87%e5%b7%a5%e4%bd%9c_oauth2-0
    /// </remarks>
    public string AppKey { get; set; }
    /// <summary>
    /// 是否移动端样式
    /// </summary>
    public bool IsMobile { get; set; }
}
