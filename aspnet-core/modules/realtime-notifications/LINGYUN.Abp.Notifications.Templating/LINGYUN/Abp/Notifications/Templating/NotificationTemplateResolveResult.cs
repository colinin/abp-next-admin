using System.Collections.Generic;

namespace LINGYUN.Abp.Notifications.Templating;
public class NotificationTemplateResolveResult
{
    /// <summary>
    /// 模板数据
    /// </summary>
    public object Model { get; set; }

    public List<string> AppliedResolvers { get; }
    public NotificationTemplateResolveResult()
    {
        AppliedResolvers = new List<string>();
    }
}
