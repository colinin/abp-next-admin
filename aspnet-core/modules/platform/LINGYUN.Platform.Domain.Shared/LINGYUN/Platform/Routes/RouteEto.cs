using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Routes
{
    public class RouteEto : IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string LinkUrl { get; set; }
        public string Icon { get; set; }
        public PlatformType PlatformType { get; set; }
    }
}
