using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateWay.Admin
{
    public class HostAndPort : Entity<Guid>
    {
        public virtual string Host { get; protected set; }
        public virtual int? Port { get; protected set; }
        protected HostAndPort()
        {

        }

        public HostAndPort(Guid id, [NotNull] string host, int? port = null)
        {
            Id = id;
            BindHost(host, port);
        }

        public void BindHost([NotNull] string host, int? port = null)
        {
            Check.NotNullOrWhiteSpace(host, nameof(host), HostAndPortConsts.MaxHostLength);
            Host = host;
            Port = port;
        }

        public override int GetHashCode()
        {
            if (!Host.IsNullOrWhiteSpace())
            {
                if (Port.HasValue)
                {
                    return Host.GetHashCode() & Port.Value;
                }
                return Host.GetHashCode();
            }
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null ||
               Host.IsNullOrWhiteSpace())
            {
                return false;
            }
            if (obj is HostAndPort hostAndPort)
            {
                if (Port.HasValue)
                {
                    return hostAndPort.Host.Equals(Host) &&
                        Port.Equals(hostAndPort.Port);
                }
                return hostAndPort.Host.Equals(Host);
            }
            return false;
        }
    }
}
