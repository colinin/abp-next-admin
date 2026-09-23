using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes;

public abstract class RouteEto : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public Guid Id { get; set; }
    public string Path { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Description { get; set; }
    public string? Redirect { get; set; }
}
