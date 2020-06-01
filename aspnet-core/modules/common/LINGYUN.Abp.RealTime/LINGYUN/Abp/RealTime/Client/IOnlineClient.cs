using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.RealTime.Client
{
    public interface IOnlineClient
    {
        string ConnectionId { get; }

        string IpAddress { get; }

        Guid? TenantId { get; }

        Guid? UserId { get; }

        string UserAccount { get; }

        string UserName { get; }

        DateTime ConnectTime { get; }

        object this[string key] { get; set; }

        Dictionary<string, object> Properties { get; }
    }
}
