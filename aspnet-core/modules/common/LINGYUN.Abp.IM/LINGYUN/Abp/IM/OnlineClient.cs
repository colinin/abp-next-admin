using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.IM
{
    [Serializable]
    public class OnlineClient : IOnlineClient
    {
        public object this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }

        public string ConnectionId { get; set; }

        public string IpAddress { get; set; }

        public Guid? TenantId { get; set; }

        public Guid? UserId { get; set; }

        public string[] Roles { get; set; }

        public DateTime ConnectTime { get; set; }

        private Dictionary<string, object> _properties;
        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _properties = value;
            }
        }

        public OnlineClient()
        {
            Roles = new string[0];
            ConnectTime = DateTime.Now;
        }

        public OnlineClient(string connectionId, string ipAddress, Guid? tenantId, Guid? userId, params string[] roles)
            : this()
        {
            ConnectionId = connectionId;
            IpAddress = ipAddress;
            TenantId = tenantId;
            UserId = userId;
            Roles = roles;

            Properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return string.Concat(
                "-- ConnectionId:", ConnectionId,
                "-- Connection Time:", ConnectTime,
                "-- IpAddress:", IpAddress ?? "::1",
                "-- UserId:", UserId.HasValue ? UserId.Value.ToString() : "None",
                "-- TenantId:", TenantId.HasValue ? TenantId.Value.ToString() : "None");
        }
    }
}
