using System.Collections.Generic;
using System.Net.Http;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Tools.Http;
public static class HttpAIToolDefinitionExtenssions
{
    private const string RemoteService = "RemoteService";
    private const string RemoteController = "RemoteController";
    private const string RemoteMethod = "RemoteMethod";

    private const string Endpoint = "HttpEndpoint";
    private const string Method = "HttpMethod";
    private const string Headers = "HttpHeaders";

    private const string CurrentAccessToken = "UseHttpCurrentAccessToken";

    public static AIToolDefinition UseHttpCurrentAccessToken(this AIToolDefinition definition, bool useCurrentAccessToken = true)
    {
        definition.WithProperty(CurrentAccessToken, useCurrentAccessToken);

        return definition;
    }

    public static AIToolDefinition WithRemoteService(this AIToolDefinition definition, string service, string controllerName, string uniqueMethodName)
    {
        Check.NotNullOrWhiteSpace(service, nameof(service));
        Check.NotNullOrWhiteSpace(controllerName, nameof(controllerName));
        Check.NotNullOrWhiteSpace(uniqueMethodName, nameof(uniqueMethodName));

        definition.WithProperty(RemoteService, service);
        definition.WithProperty(RemoteController, controllerName);
        definition.WithProperty(RemoteMethod, uniqueMethodName);

        return definition;
    }

    public static AIToolDefinition WithHttpEndpoint(this AIToolDefinition definition, string endPoint)
    {
        Check.NotNullOrWhiteSpace(endPoint, nameof(endPoint));

        definition.WithProperty(Endpoint, endPoint);

        return definition;
    }

    public static AIToolDefinition WithHttpMethod(this AIToolDefinition definition, HttpMethod method)
    {
        Check.NotNull(method, nameof(method));

        definition.WithProperty(Method, method.Method);

        return definition;
    }

    public static AIToolDefinition WithHttpHeaders(this AIToolDefinition definition, IDictionary<string, string> headers)
    {
        Check.NotNullOrEmpty(headers, nameof(headers));

        var currentHeaders = definition.GetHttpHeaders();

        currentHeaders.AddIfNotContains(headers);

        definition.WithProperty(Headers, currentHeaders);

        return definition;
    }

    public static AIToolDefinition WithHttpHeaders(this AIToolDefinition definition, string key, string value)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNullOrEmpty(value, nameof(value));

        var currentHeaders = definition.GetHttpHeaders();

        currentHeaders.TryAdd(key, value);

        definition.WithProperty(Headers, currentHeaders);

        return definition;
    }

    public static bool IsUseHttpCurrentAccessToken(this AIToolDefinition definition)
    {
        if (definition.Properties.TryGetValue(CurrentAccessToken, out var useCurrentAccessTokenObj) && useCurrentAccessTokenObj != null
            && bool.TryParse(useCurrentAccessTokenObj.ToString(), out var useCurrentAccessToken))
        {
            return useCurrentAccessToken;
        }

        return false;
    }

    public static string? GetRemoteServiceOrNull(this AIToolDefinition definition)
    {
        definition.Properties.TryGetValue(RemoteService, out var remoteServiceObj);

        return remoteServiceObj?.ToString();
    }

    public static string? GetRemoteControllerOrNull(this AIToolDefinition definition)
    {
        definition.Properties.TryGetValue(RemoteController, out var remoteControllerObj);

        return remoteControllerObj?.ToString();
    }

    public static string? GetRemoteMethodOrNull(this AIToolDefinition definition)
    {
        definition.Properties.TryGetValue(RemoteMethod, out var remoteMethodObj);

        return remoteMethodObj?.ToString();
    }

    public static string GetHttpEndpoint(this AIToolDefinition definition)
    {
        definition.Properties.TryGetValue(Endpoint, out var endpointObj);

        Check.NotNull(endpointObj, nameof(Endpoint));

        return endpointObj.ToString()!;
    }

    public static HttpMethod GetHttpMethod(this AIToolDefinition definition)
    {
        if (definition.Properties.TryGetValue(Method, out var methodObj) && methodObj != null)
        {
            try
            {
                return HttpMethod.Parse(methodObj.ToString());
            }
            catch { }
        }

        return HttpMethod.Get;
    }

    public static IDictionary<string, string> GetHttpHeaders(this AIToolDefinition definition)
    {
        if (definition.Properties.TryGetValue(Headers, out var headersObj) && headersObj != null)
        {
            if (headersObj is IDictionary<string, string> h)
            {
                return h;
            }
        }

        return new Dictionary<string, string>();
    }
}
