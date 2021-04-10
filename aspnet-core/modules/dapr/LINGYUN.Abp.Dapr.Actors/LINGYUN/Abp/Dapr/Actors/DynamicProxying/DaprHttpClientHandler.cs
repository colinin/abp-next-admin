using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dapr.Actors.DynamicProxying
{
    public class DaprHttpClientHandler : HttpClientHandler
    {
        private Action<HttpRequestMessage> _preConfigureInvoke;
        protected Action<HttpRequestMessage> PreConfigureInvoke => _preConfigureInvoke;

        public virtual void PreConfigure(Action<HttpRequestMessage> config)
        {
            if (_preConfigureInvoke == null)
            {
                _preConfigureInvoke = config;
            }
            else
            {
                _preConfigureInvoke += config;
            }
        }

        public void AddHeader(string key, string value)
        {
            PreConfigure(request =>
            {
                request.Headers.Add(key, value);
            });
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            PreConfigureInvoke?.Invoke(request);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
