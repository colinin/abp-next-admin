using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class HostAndPort : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }
        public virtual string Host { get; private set; }
        public virtual int? Port { get; private set; }

        protected HostAndPort()
        {

        }

        public HostAndPort(long rerouteId)
        {
            ReRouteId = rerouteId;
        }

        public void SetHostAndPort(string host, int? port)
        {
            Host = host;
            Port = port;
        }
    }
}
