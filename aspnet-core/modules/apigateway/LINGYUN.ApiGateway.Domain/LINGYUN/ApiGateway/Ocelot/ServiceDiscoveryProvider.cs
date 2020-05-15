using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class ServiceDiscoveryProvider : Entity<int>
    {
        public virtual long ItemId { get; protected set; }
        public virtual string Host { get; set; }
        public virtual int? Port { get; set; }
        public virtual string Type { get; set; }
        public virtual string Token { get; set; }
        public virtual string ConfigurationKey { get; set; }
        public virtual int? PollingInterval { get; set; }
        public virtual string Namespace { get; set; }
        public virtual string Scheme { get; set; }
        public virtual GlobalConfiguration GlobalConfiguration { get; private set; }
        protected ServiceDiscoveryProvider()
        {

        }
        public ServiceDiscoveryProvider(long itemId)
        {
            ItemId = itemId;
        }

        public void BindServiceRegister(string host, int? port)
        {
            Host = host;
            Port = port;
        }
    }
}
