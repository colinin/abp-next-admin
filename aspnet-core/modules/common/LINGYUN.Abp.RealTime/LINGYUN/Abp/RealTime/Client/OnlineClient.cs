using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.RealTime.Client
{
    [Serializable]
    public class OnlineClient : IOnlineClient
    {
        public object this[object key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }

        public string ConnectionId { get; set; }

        public string IpAddress { get; set; }

        public Guid? TenantId { get; set; }

        public Guid? UserId { get; }
        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string[] Roles { get; set; }

        public DateTime ConnectTime { get; set; }

        private IDictionary<object, object> _properties;
        public IDictionary<object, object> Properties
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
            ConnectTime = DateTime.Now;
        }

        public OnlineClient(string connectionId, string ipAddress, Guid? tenantId, Guid? userId)
            : this()
        {
            ConnectionId = connectionId;
            IpAddress = ipAddress;
            TenantId = tenantId;
            UserId = userId;

            Roles = new string[0];
            Properties = new Dictionary<object, object>();
        }

        public override string ToString()
        {
            return string.Concat(
                "-- ConnectionId:", ConnectionId,
                "-- Connection Time:", ConnectTime,
                "-- IpAddress:", IpAddress ?? "::1",
                "-- UserName:", UserName,
                "-- TenantId:", TenantId.HasValue ? TenantId.Value.ToString() : "None");
        }
    }
}
