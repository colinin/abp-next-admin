namespace LINGYUN.Abp.Identity.Session.AspNetCore;
public class AbpIdentitySessionAspNetCoreOptions
{
    /// <summary>
    /// 是否解析IP地理信息
    /// </summary>
    public bool IsParseIpLocation { get; set; }

    public AbpIdentitySessionAspNetCoreOptions()
    {
        IsParseIpLocation = false;
    }
}
