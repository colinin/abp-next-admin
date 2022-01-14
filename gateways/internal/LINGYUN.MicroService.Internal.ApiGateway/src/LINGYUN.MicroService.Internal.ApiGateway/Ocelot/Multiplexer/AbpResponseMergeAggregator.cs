using LINGYUN.MicroService.Internal.ApiGateway;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.Http;

namespace Ocelot.Multiplexer
{
    public class AbpResponseMergeAggregator : IDefinedAggregator
    {
        public ILogger<AbpResponseMergeAggregator> Logger { protected get; set; }
        protected InternalApiGatewayOptions Options { get; }

        public AbpResponseMergeAggregator(
            IOptions<InternalApiGatewayOptions> options)
        {
            Options = options.Value;

            Logger = NullLogger<AbpResponseMergeAggregator>.Instance;
        }

        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            return await MapAbpApiDefinitionAggregateContentAsync(responses);
        }

        protected virtual async Task<DownstreamResponse> MapAbpApiDefinitionAggregateContentAsync(List<HttpContext> responses)
        {
            JObject responseObject = null;
            JsonMergeSettings mergeSetting = new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                PropertyNameComparison = System.StringComparison.CurrentCultureIgnoreCase
            };
            foreach (var httpResponse in responses)
            {
                var errorItems = httpResponse.Items.Errors();
                var response = httpResponse.Items.DownstreamResponse();

                if (errorItems.Any())
                {
                    if (!Options.AggregationIgnoreError)
                    {
                        var errorItem = errorItems.First();
                        string error = errorItems.Select(x => x.Message).JoinAsString(";");
                        if (httpResponse.Response.Headers.ContainsKey(AbpHttpConsts.AbpErrorFormat))
                        {
                            // header 存在abp错误标头, 可以序列化错误信息
                            error = await response.Content.ReadAsStringAsync();
                        }
                        var errorContent = new StringContent(error)
                        {
                            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                        };
                        if (httpResponse.Response.Headers.ContainsKey(AbpHttpConsts.AbpErrorFormat))
                        {
                            errorContent.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
                        }

                        return new DownstreamResponse(
                            errorContent,
                            (HttpStatusCode)errorItem.HttpStatusCode,
                            httpResponse.Response.Headers.Select(x => new Header(x.Key, x.Value)).ToList(),
                            errorItem.Message);
                    }
                    var route = httpResponse.Items.DownstreamRoute();
                    var service = route.ServiceName ?? route.LoadBalancerKey ?? route.DownstreamPathTemplate.Value;
                    Logger.LogWarning($"The downstream service {service} returned an error, which is configured to be ignored");
                    continue;
                }
                var content = await response.Content.ReadAsStringAsync();
                var contentObject = JsonConvert.DeserializeObject(content);
                if (responseObject == null)
                {
                    responseObject = JObject.FromObject(contentObject);
                }
                else
                {
                    responseObject.Merge(contentObject, mergeSetting);
                }
            }
            var stringContent = new StringContent(responseObject.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            //if (responses.Any(response => response.Response.Headers.ContainsKey(AbpHttpConsts.AbpErrorFormat)))
            //{
            //    stringContent.Headers.Add("_AbpErrorFormat", "true");
            //}

            return new DownstreamResponse(
                stringContent,
                HttpStatusCode.OK,
                new List<KeyValuePair<string, IEnumerable<string>>>(),
                "OK");
        }

    }
}
