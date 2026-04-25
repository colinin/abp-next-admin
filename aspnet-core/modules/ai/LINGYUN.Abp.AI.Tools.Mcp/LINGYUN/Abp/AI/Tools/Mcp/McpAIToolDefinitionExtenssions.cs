using ModelContextProtocol.Authentication;
using ModelContextProtocol.Client;
using System;
using System.Collections.Generic;
using Volo.Abp;

namespace LINGYUN.Abp.AI.Tools.Mcp;
public static class McpAIToolDefinitionExtenssions
{
    public const string Endpoint = "McpEndpoint";
    public const string Headers = "McpHeaders";
    public const string TransportMode = "McpTransportMode";
    public const string ConnectionTimeout = "McpConnectionTimeout";
    public const string MaxReconnectionAttempts = "McpMaxReconnectionAttempts";
    public const string CurrentAccessToken = "UseMcpCurrentAccessToken";

    public static AIToolDefinition UseMcpCurrentAccessToken(this AIToolDefinition definition, bool useCurrentAccessToken = true)
    {
        definition.WithProperty(CurrentAccessToken, useCurrentAccessToken);

        return definition;
    }

    public static AIToolDefinition WithMcpEndpoint(this AIToolDefinition definition, string endPoint)
    {
        Check.NotNullOrWhiteSpace(endPoint, nameof(endPoint));

        definition.WithProperty(Endpoint, endPoint);

        return definition;
    }

    public static AIToolDefinition WithMcpTransportMode(this AIToolDefinition definition, HttpTransportMode transportMode = HttpTransportMode.AutoDetect)
    {
        definition.WithProperty(TransportMode, ((int)transportMode).ToString());

        return definition;
    }

    public static AIToolDefinition WithMcpConnectionTimeout(this AIToolDefinition definition, TimeSpan connectionTimeout)
    {
        definition.WithProperty(ConnectionTimeout, connectionTimeout.ToString());

        return definition;
    }

    public static AIToolDefinition WithMcpMaxReconnectionAttempts(this AIToolDefinition definition, int maxReconnectionAttempts = 5)
    {
        definition.WithProperty(MaxReconnectionAttempts, maxReconnectionAttempts);

        return definition;
    }

    public static AIToolDefinition WithMcpHeaders(this AIToolDefinition definition, IDictionary<string, string> headers)
    {
        Check.NotNullOrEmpty(headers, nameof(headers));

        var currentHeaders = definition.GetMcpHeaders();

        currentHeaders.AddIfNotContains(headers);

        definition.WithProperty(Headers, currentHeaders);

        return definition;
    }

    public static AIToolDefinition WithMcpHeaders(this AIToolDefinition definition, string key, string value)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNullOrEmpty(value, nameof(value));

        var currentHeaders = definition.GetMcpHeaders();

        currentHeaders.TryAdd(key, value);

        definition.WithProperty(Headers, currentHeaders);

        return definition;
    }

    public static bool IsUseMcpCurrentAccessToken(this AIToolDefinition definition)
    {
        if (definition.Properties.TryGetValue(CurrentAccessToken, out var useCurrentAccessTokenObj) && useCurrentAccessTokenObj != null
            && bool.TryParse(useCurrentAccessTokenObj.ToString(), out var useCurrentAccessToken))
        {
            return useCurrentAccessToken;
        }

        return false;
    }

    public static string GetMcpEndpoint(this AIToolDefinition definition)
    {
        definition.Properties.TryGetValue(Endpoint, out var endpointObj);

        Check.NotNull(endpointObj, nameof(Endpoint));

        return endpointObj.ToString()!;
    }

    public static TimeSpan GetMcpConnectionTimeout(this AIToolDefinition definition, double defaultConnectionTimeoutSeconds = 30)
    {
        definition.Properties.TryGetValue(ConnectionTimeout, out var connectionTimeoutObj);

        if (connectionTimeoutObj != null &&
            TimeSpan.TryParse(connectionTimeoutObj.ToString(), out var connectionTimeout))
        {
            return connectionTimeout;
        }

        return TimeSpan.FromSeconds(defaultConnectionTimeoutSeconds);
    }

    public static HttpTransportMode GetMcpTransportMode(this AIToolDefinition definition, HttpTransportMode defaultTransportMode = HttpTransportMode.AutoDetect)
    {
        definition.Properties.TryGetValue(TransportMode, out var defaultTransportModeObj);

        if (defaultTransportModeObj != null &&
            Enum.TryParse<HttpTransportMode>(defaultTransportModeObj.ToString(), out var transportMode))
        {
            return transportMode;
        }

        return defaultTransportMode;
    }
    public static int GetMcpMaxReconnectionAttempts(this AIToolDefinition definition, int defaultMaxReconnectionAttempts = 5)
    {
        definition.Properties.TryGetValue(MaxReconnectionAttempts, out var maxReconnectionAttemptsObj);

        if (maxReconnectionAttemptsObj != null &&
            int.TryParse(maxReconnectionAttemptsObj.ToString(), out var maxReconnectionAttempts))
        {
            return maxReconnectionAttempts;
        }

        return defaultMaxReconnectionAttempts;
    }

    public static IDictionary<string, string> GetMcpHeaders(this AIToolDefinition definition)
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

    #region OAuth

    public const string UseOAuth = "McpUseOAuth";
    public const string RedirectUri = "McpRedirectUri";
    public const string ClientId = "McpClientId";
    public const string ClientSecret = "McpClientSecret";
    public const string ClientMetadataDocumentUri = "McpClientMetadataDocumentUri";
    public const string Scopes = "McpScopes";
    public const string AuthorizationParameters = "McpAuthorizationParameters";

    public static AIToolDefinition WithMcpOAuth(this AIToolDefinition definition, ClientOAuthOptions oAuthOptions)
    {
        Check.NotNull(oAuthOptions, nameof(oAuthOptions));

        definition.WithProperty(UseOAuth, true);
        definition.WithProperty(RedirectUri, oAuthOptions.RedirectUri.ToString());
        if (!oAuthOptions.ClientId.IsNullOrWhiteSpace())
        {
            definition.WithProperty(ClientId, oAuthOptions.ClientId);
        }
        if (!oAuthOptions.ClientSecret.IsNullOrWhiteSpace())
        {
            definition.WithProperty(ClientSecret, oAuthOptions.ClientSecret);
        }
        if (oAuthOptions.ClientMetadataDocumentUri != null)
        {
            definition.WithProperty(ClientMetadataDocumentUri, oAuthOptions.ClientMetadataDocumentUri.ToString());
        }
        if (oAuthOptions.Scopes != null)
        {
            definition.WithProperty(Scopes, oAuthOptions.Scopes.JoinAsString(" "));
        }
        if (oAuthOptions.AdditionalAuthorizationParameters != null)
        {
            definition.WithProperty(AuthorizationParameters, oAuthOptions.AdditionalAuthorizationParameters);
        }

        return definition;
    }

    public static ClientOAuthOptions? GetMcpOAuth(this AIToolDefinition definition)
    {
        if (definition.Properties.TryGetValue(UseOAuth, out var useOAuthObj) && useOAuthObj is true &&
            definition.Properties.TryGetValue(RedirectUri, out var redirectUriObj) && redirectUriObj != null)
        {
            var oAuthOptions = new ClientOAuthOptions
            {
                RedirectUri = new Uri(redirectUriObj.ToString()!)
            };
            if (definition.Properties.TryGetValue(ClientId, out var clientIdObj) && clientIdObj != null)
            {
                oAuthOptions.ClientId = clientIdObj.ToString();
            }
            if (definition.Properties.TryGetValue(ClientSecret, out var clientSecretObj) && clientSecretObj != null)
            {
                oAuthOptions.ClientSecret = clientSecretObj.ToString();
            }
            if (definition.Properties.TryGetValue(ClientMetadataDocumentUri, out var clientMetadataDocumentUriObj) && clientMetadataDocumentUriObj != null)
            {
                var clientMetadataDocumentUri = clientMetadataDocumentUriObj.ToString();
                if (!clientMetadataDocumentUri.IsNullOrWhiteSpace())
                {
                    oAuthOptions.ClientMetadataDocumentUri = new Uri(clientMetadataDocumentUri);
                }
            }
            if (definition.Properties.TryGetValue(Scopes, out var scopesObj) && scopesObj != null)
            {
                oAuthOptions.Scopes = scopesObj.ToString()?.Split(" ");
            }
            if (definition.Properties.TryGetValue(AuthorizationParameters, out var parametersObj) && parametersObj != null)
            {
                if (parametersObj is IDictionary<string, string> parameters)
                {
                    oAuthOptions.AdditionalAuthorizationParameters = parameters;
                }
            }
            return oAuthOptions;
        }

        return null;
    }

    #endregion
}
