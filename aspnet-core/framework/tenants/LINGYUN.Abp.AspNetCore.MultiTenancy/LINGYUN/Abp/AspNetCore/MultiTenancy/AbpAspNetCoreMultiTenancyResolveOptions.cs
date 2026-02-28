namespace LINGYUN.Abp.AspNetCore.MultiTenancy;

public class AbpAspNetCoreMultiTenancyResolveOptions
{
    /// <summary>
    /// 仅解析域名中的租户, 默认: true
    /// </summary>
    public bool OnlyResolveDomain {  get; set; }
    public AbpAspNetCoreMultiTenancyResolveOptions()
    {
        OnlyResolveDomain = true;
    }
}
