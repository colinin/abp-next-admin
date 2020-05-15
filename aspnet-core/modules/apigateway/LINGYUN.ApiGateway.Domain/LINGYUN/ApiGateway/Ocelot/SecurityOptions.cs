using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class SecurityOptions : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }

        public virtual string IPAllowedList { get; private set; }

        public virtual string IPBlockedList { get; private set; }
        public virtual ReRoute ReRoute { get; private set; }
        protected SecurityOptions()
        {

        }
        public SecurityOptions(long rerouteId)
        {
            ReRouteId = rerouteId;
        }
        public void SetAllowIpList(List<string> allowIpList)
        {
            IPAllowedList = allowIpList.JoinAsString(",");
        }

        public void SetBlockIpList(List<string> blockIpList)
        {
            IPBlockedList = blockIpList.JoinAsString(",");
        }
    }
}
