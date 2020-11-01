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

        string[] Roles { get; }

        object this[object key] { get; set; }

        IDictionary<object, object> Properties { get; }
    }
}
