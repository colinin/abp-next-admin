namespace LINGYUN.Abp.AspNetCore.Session;

public class AbpAspNetCoreSessionOptions
{
    /// <summary>
    /// 是否解析IP地理信息
    /// </summary>
    public bool IsParseIpLocation { get; set; }

    public AbpAspNetCoreSessionOptions()
    {
        IsParseIpLocation = false;
    }
}
