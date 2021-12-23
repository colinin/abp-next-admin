using Dapr.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;
using Volo.Abp.Tracing;

namespace LINGYUN.Abp.Dapr.Client.DynamicProxying
{
    public class DaprApiDescriptionFinder : IDaprApiDescriptionFinder, ITransientDependency
    {
        public static JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ICancellationTokenProvider CancellationTokenProvider { get; set; }
        protected IApiDescriptionCache Cache { get; }
        protected AbpCorrelationIdOptions AbpCorrelationIdOptions { get; }
        protected ICorrelationIdProvider CorrelationIdProvider { get; }
        protected ICurrentTenant CurrentTenant { get; }

        protected IDaprClientFactory DaprClientFactory { get; }
        public DaprApiDescriptionFinder(
            IDaprClientFactory daprClientFactory,
            IApiDescriptionCache cache, 
            IOptions<AbpCorrelationIdOptions> abpCorrelationIdOptions, 
            ICorrelationIdProvider correlationIdProvider, 
            ICurrentTenant currentTenant) 
        {
            DaprClientFactory = daprClientFactory;

            Cache = cache;
            AbpCorrelationIdOptions = abpCorrelationIdOptions.Value;
            CorrelationIdProvider = correlationIdProvider;
            CurrentTenant = currentTenant;
            CancellationTokenProvider = NullCancellationTokenProvider.Instance;
        }

        public virtual async Task<ActionApiDescriptionModel> FindActionAsync(string service, string appId, Type serviceType, MethodInfo method)
        {
            var apiDescription = await GetApiDescriptionAsync(service, appId);

            //TODO: Cache finding?

            var methodParameters = method.GetParameters().ToArray();

            foreach (var module in apiDescription.Modules.Values)
            {
                foreach (var controller in module.Controllers.Values)
                {
                    if (!controller.Implements(serviceType))
                    {
                        continue;
                    }

                    foreach (var action in controller.Actions.Values)
                    {
                        if (action.Name == method.Name && action.ParametersOnMethod.Count == methodParameters.Length)
                        {
                            var found = true;

                            for (int i = 0; i < methodParameters.Length; i++)
                            {
                                if (!TypeMatches(action.ParametersOnMethod[i], methodParameters[i]))
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                return action;
                            }
                        }
                    }
                }
            }

            throw new AbpException($"Could not found remote action for method: {method} on the appId: {appId}");
        }

        public virtual async Task<ApplicationApiDescriptionModel> GetApiDescriptionAsync(string service, string appId)
        {
            return await Cache.GetAsync(appId, () => GetApiDescriptionFromServerAsync(service, appId));
        }

        protected virtual async Task<ApplicationApiDescriptionModel> GetApiDescriptionFromServerAsync(string service, string appId)
        {
            var client = DaprClientFactory.CreateClient(service);
            var requestMessage = client.CreateInvokeMethodRequest(HttpMethod.Get, appId, "api/abp/api-definition");

            AddHeaders(requestMessage);

            var response = await client.InvokeMethodWithResponseAsync(
                requestMessage,
                CancellationTokenProvider.Token);

            if (!response.IsSuccessStatusCode)
            {
                throw new AbpException("Remote service returns error! StatusCode = " + response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(content, DeserializeOptions);

            return result;
        }

        protected virtual void AddHeaders(HttpRequestMessage requestMessage)
        {
            //CorrelationId
            requestMessage.Headers.Add(AbpCorrelationIdOptions.HttpHeaderName, CorrelationIdProvider.Get());

            //TenantId
            if (CurrentTenant.Id.HasValue)
            {
                //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
                requestMessage.Headers.Add(TenantResolverConsts.DefaultTenantKey, CurrentTenant.Id.Value.ToString());
            }

            //Culture
            //TODO: Is that the way we want? Couldn't send the culture (not ui culture)
            var currentCulture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if (!currentCulture.IsNullOrEmpty())
            {
                requestMessage.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCulture));
            }

            //X-Requested-With
            requestMessage.Headers.Add("X-Requested-With", "XMLHttpRequest");
        }

        protected virtual bool TypeMatches(MethodParameterApiDescriptionModel actionParameter, ParameterInfo methodParameter)
        {
            return actionParameter.Type.ToUpper() == TypeHelper.GetFullNameHandlingNullableAndGenerics(methodParameter.ParameterType).ToUpper();
        }
    }
}
