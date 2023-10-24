using LINGYUN.Abp.Dapr.Client.ClientProxying;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Http.Client.ClientProxying;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DynamicDaprProxyInterceptorClientProxy<TService> : DaprClientProxyBase<TService>
    {
        public async virtual Task<T> CallRequestAsync<T>(ClientProxyRequestContext requestContext)
        {
            return await RequestAsync<T>(requestContext);
        }

        public async virtual Task<HttpContent> CallRequestAsync(ClientProxyRequestContext requestContext)
        {
            return await RequestAsync(requestContext);
        }
    }
}
