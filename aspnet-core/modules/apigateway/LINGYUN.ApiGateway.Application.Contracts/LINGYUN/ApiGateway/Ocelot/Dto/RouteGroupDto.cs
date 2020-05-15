using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroupDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string AppId { get; set; }
        public string AppName { get; set; }
        public string AppIpAddress { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
