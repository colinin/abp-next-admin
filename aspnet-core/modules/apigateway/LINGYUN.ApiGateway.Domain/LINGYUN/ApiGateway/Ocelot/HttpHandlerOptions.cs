using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class HttpHandlerOptions : Entity<int>
    {
        public virtual long? ItemId { get; private set; }

        public virtual long? ReRouteId { get; private set; }

        public virtual int? MaxConnectionsPerServer { get; private set; }

        public virtual bool AllowAutoRedirect { get; private set; }

        public virtual bool UseCookieContainer { get; private set; }

        public virtual bool UseTracing { get; private set; }

        public virtual bool UseProxy { get; private set; }
        public virtual ReRoute ReRoute { get; private set; }
        public virtual GlobalConfiguration GlobalConfiguration { get; private set; }
        protected HttpHandlerOptions()
        {

        }

        public static HttpHandlerOptions Default()
        {
            var options = new HttpHandlerOptions();
            options.ApplyAllowAutoRedirect(false);
            options.ApplyCookieContainer(false);
            options.ApplyHttpTracing(false);
            options.ApplyHttpProxy(false);
            return options;
        }
        public HttpHandlerOptions SetReRouteId(long rerouteId)
        {
            ReRouteId = rerouteId;
            return this;
        }

        public HttpHandlerOptions SetItemId(long itemId)
        {
            ItemId = itemId;
            return this;
        }

        public void ApplyAllowAutoRedirect(bool allowAutoRedirect)
        {
            AllowAutoRedirect = allowAutoRedirect;
        }
        public void ApplyCookieContainer(bool useCookieContainer)
        {
            UseCookieContainer = useCookieContainer;
        }

        public void ApplyHttpTracing(bool httpTracing)
        {
            UseTracing = httpTracing;
        }

        public void ApplyHttpProxy(bool httpProxy)
        {
            UseProxy = httpProxy;
        }

        public void SetMaxConnections(int? maxConnections)
        {
            MaxConnectionsPerServer = maxConnections;
        }
    }
}
