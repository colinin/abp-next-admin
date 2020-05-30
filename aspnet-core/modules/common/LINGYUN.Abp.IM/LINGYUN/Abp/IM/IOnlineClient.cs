using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.IM
{
    public interface IOnlineClient
    {
        string ConnectionId { get; }

        string IpAddress { get; }

        Guid? TenantId { get; }

        Guid? UserId { get; }

        string[] Roles { get; }

        DateTime ConnectTime { get; }

        object this[string key] { get; set; }

        Dictionary<string, object> Properties { get; }
    }
}
