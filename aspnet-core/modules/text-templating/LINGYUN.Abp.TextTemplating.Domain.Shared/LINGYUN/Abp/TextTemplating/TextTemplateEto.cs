using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.TextTemplating;

public class TextTemplateEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Content { get; set; }
    public string Culture { get; set; }
}
