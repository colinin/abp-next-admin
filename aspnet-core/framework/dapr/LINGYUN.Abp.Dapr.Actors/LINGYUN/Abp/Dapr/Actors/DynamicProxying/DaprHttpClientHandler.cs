using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Dapr.Actors.DynamicProxying
{
    public class DaprHttpClientHandler : HttpClientHandler
    {
        private Func<HttpRequestMessage, Task> _preConfigureInvoke;
        protected Func<HttpRequestMessage, Task> PreConfigureInvoke => _preConfigureInvoke;

        public virtual void PreConfigure(Func<HttpRequestMessage, Task> config)
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

                return Task.CompletedTask;
            });
        }

        public void AcceptLanguage(string value)
        {
            PreConfigure(request =>
            {
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(value));

                return Task.CompletedTask;
            });
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (PreConfigureInvoke != null)
            {
                await PreConfigureInvoke(request);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
