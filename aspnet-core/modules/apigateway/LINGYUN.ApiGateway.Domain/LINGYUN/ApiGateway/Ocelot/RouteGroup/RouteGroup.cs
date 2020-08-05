using LINGYUN.ApiGateway.Data.Filter;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class RouteGroup : FullAuditedAggregateRoot<Guid>, IActivation
    {
        public virtual string Name { get; set; }
        public virtual string AppId { get; protected set; }
        public virtual string AppName { get; protected set; }
        public virtual string AppIpAddress { get; protected set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        protected RouteGroup()
        {

        }

        public RouteGroup(string appId, string appName, string appIp)
        {
            AppId = appId;
            AppName = appName;
            AppIpAddress = appIp;
        }

        public void SwitchApp(string appName, string appIp)
        {
            AppName = appName ?? AppName;
            AppIpAddress = appIp ?? AppIpAddress;
        }
    }
}
