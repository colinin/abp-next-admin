using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.Middleware.Multiplexer
{
    public class AbpApiDefinitionAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            var isAbpResponse = responses.Any(response => response.DownstreamResponse.Headers.Any(h => h.Key.Equals("_abperrorformat")));
            return await MapAbpApiDefinitionAggregateContentAsync(responses);
            //if (isAbpResponse)
            //{
            //    return await MapAbpApiDefinitionAggregateContentAsync(responses);
            //}
            //else
            //{
            //    return await MapSimpleJsonAggregateContentAsync(responses);
            //}
        }

        protected virtual async Task<DownstreamResponse> MapAbpApiDefinitionAggregateContentAsync(List<DownstreamContext> downstreamContexts)
        {
            var responseKeys = downstreamContexts.Select(s => s.DownstreamReRoute.Key).Distinct().ToList();
            JObject responseObject = null;
            for (var k = 0; k < responseKeys.Count; k++)
            {
                var contexts = downstreamContexts.Where(w => w.DownstreamReRoute.Key == responseKeys[k]).ToList();
                if (contexts.Count == 1)
                {
                    if (contexts[0].IsError)
                    {
                        return contexts[0].DownstreamResponse;
                    }

                    var content = await contexts[0].DownstreamResponse.Content.ReadAsStringAsync();
                    var contentObject = JsonConvert.DeserializeObject(content);
                    if (responseObject == null)
                    {
                        responseObject = JObject.FromObject(contentObject);
                    }
                    else
                    {
                        responseObject.Merge(contentObject);
                    }
                }
                else
                {
                    for (var i = 0; i < contexts.Count; i++)
                    {
                        if (contexts[i].IsError)
                        {
                            return contexts[i].DownstreamResponse;
                        }

                        var content = await contexts[i].DownstreamResponse.Content.ReadAsStringAsync();
                        var contentObject = JsonConvert.DeserializeObject(content);
                        if (responseObject == null)
                        {
                            responseObject = JObject.FromObject(contentObject);
                        }
                        else
                        {
                            responseObject.Merge(contentObject);
                        }
                    }
                }
            }

            var stringContent = new StringContent(responseObject.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };
            stringContent.Headers.Add("_abperrorformat", "true");
            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "cannot return from aggregate..which reason phrase would you use?");
        }

        protected virtual async Task<DownstreamResponse> MapSimpleJsonAggregateContentAsync(List<DownstreamContext> downstreamContexts)
        {
            var contentBuilder = new StringBuilder();

            contentBuilder.Append("{");

            var responseKeys = downstreamContexts.Select(s => s.DownstreamReRoute.Key).Distinct().ToList();

            for (var k = 0; k < responseKeys.Count; k++)
            {
                var contexts = downstreamContexts.Where(w => w.DownstreamReRoute.Key == responseKeys[k]).ToList();
                if (contexts.Count == 1)
                {
                    if (contexts[0].IsError)
                    {
                        return contexts[0].DownstreamResponse;
                    }

                    var content = await contexts[0].DownstreamResponse.Content.ReadAsStringAsync();
                    contentBuilder.Append($"\"{responseKeys[k]}\":{content}");
                }
                else
                {
                    contentBuilder.Append($"\"{responseKeys[k]}\":");
                    contentBuilder.Append("[");

                    for (var i = 0; i < contexts.Count; i++)
                    {
                        if (contexts[i].IsError)
                        {
                            return contexts[i].DownstreamResponse;
                        }

                        var content = await contexts[i].DownstreamResponse.Content.ReadAsStringAsync();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            continue;
                        }

                        contentBuilder.Append($"{content}");

                        if (i + 1 < contexts.Count)
                        {
                            contentBuilder.Append(",");
                        }
                    }

                    contentBuilder.Append("]");
                }

                if (k + 1 < responseKeys.Count)
                {
                    contentBuilder.Append(",");
                }
            }

            contentBuilder.Append("}");

            var stringContent = new StringContent(contentBuilder.ToString())
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "cannot return from aggregate..which reason phrase would you use?");
        }

    }
}
