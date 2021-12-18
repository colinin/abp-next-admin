using Microsoft.Net.Http.Headers;
using Shouldly;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.TestBase;

namespace LINGYUN.Abp.AspNetCore
{
    public class AbpAspNetCoreTestBase : AbpAspNetCoreTestBase<Startup>
    {

    }

    public abstract class AbpAspNetCoreTestBase<TStartup> : AbpAspNetCoreIntegratedTestBase<TStartup>
        where TStartup : class
    {
        protected IDictionary<string, string> RequestHeaders { get; }

        protected AbpAspNetCoreTestBase()
        {
            RequestHeaders = new Dictionary<string, string>();
        }

        protected virtual async Task<T> GetResponseAsObjectAsync<T>(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK, bool xmlHttpRequest = false, HttpMethod method = null)
        {
            var strResponse = await GetResponseAsStringAsync(url, expectedStatusCode, xmlHttpRequest, method);
            return JsonSerializer.Deserialize<T>(strResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK, bool xmlHttpRequest = false, HttpMethod method = null)
        {
            using (var response = await GetResponseAsync(url, expectedStatusCode, xmlHttpRequest, method))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK, bool xmlHttpRequest = false, HttpMethod method = null)
        {
            using (var requestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("Accept-Language", CultureInfo.CurrentUICulture.Name);
                if (xmlHttpRequest)
                {
                    requestMessage.Headers.Add(HeaderNames.XRequestedWith, "XMLHttpRequest");
                }
                foreach (var header in RequestHeaders)
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                var response = await Client.SendAsync(requestMessage);
                response.StatusCode.ShouldBe(expectedStatusCode);
                return response;
            }
        }
    }
}
