using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.Auditing;
public class AbpAspNetCoreAuditingHeaderOptions
{
    /// <summary>
    /// 是否在审计日志中记录Http请求头,默认: true
    /// </summary>
    public bool IsEnabled {  get; set; }
    /// <summary>
    /// 要记录的Http请求头
    /// </summary>
    public IList<string> HttpHeaders { get; }
    public AbpAspNetCoreAuditingHeaderOptions()
    {
        IsEnabled = true;
        HttpHeaders = new List<string>();
    }
}
