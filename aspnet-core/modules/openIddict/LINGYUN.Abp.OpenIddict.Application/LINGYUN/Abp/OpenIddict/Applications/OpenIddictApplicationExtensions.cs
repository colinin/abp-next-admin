using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Applications;

namespace LINGYUN.Abp.OpenIddict.Applications;

internal static class OpenIddictApplicationExtensions
{
    public static OpenIddictApplication ToEntity(this OpenIddictApplicationCreateOrUpdateDto dto, OpenIddictApplication entity, IJsonSerializer jsonSerializer)
    {
        Check.NotNull(dto, nameof(dto));
        Check.NotNull(entity, nameof(entity));

        entity.ConsentType = dto.ConsentType;
        entity.DisplayName = dto.DisplayName;
        entity.DisplayNames = jsonSerializer.Serialize(dto.DisplayNames);
        entity.PostLogoutRedirectUris = jsonSerializer.Serialize(dto.PostLogoutRedirectUris);
        entity.Properties = jsonSerializer.Serialize(dto.Properties);
        entity.RedirectUris = jsonSerializer.Serialize(dto.RedirectUris);
        entity.ApplicationType = dto.ApplicationType;
        entity.ClientUri = dto.ClientUri;
        entity.ClientType = dto.ClientType;
        entity.LogoUri = dto.LogoUri;

        TrySetSettings(jsonSerializer, dto, entity);
        TrySetRequirements(jsonSerializer, dto, entity);

        var permissions = new List<string>();
        permissions.AddRange(
            dto.Endpoints.Select(endpoint =>
            {
                if (!endpoint.StartsWith(OpenIddictConstants.Permissions.Prefixes.Endpoint))
                {
                    return OpenIddictConstants.Permissions.Prefixes.Endpoint + endpoint;
                }
                return endpoint;
            }));
        permissions.AddRange(
            dto.GrantTypes.Select(grantType =>
            {
                if (!grantType.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType))
                {
                    return OpenIddictConstants.Permissions.Prefixes.GrantType + grantType;
                }
                return grantType;
            }));
        permissions.AddRange(
            dto.ResponseTypes.Select(responseType =>
            {
                if (!responseType.StartsWith(OpenIddictConstants.Permissions.Prefixes.ResponseType))
                {
                    return OpenIddictConstants.Permissions.Prefixes.ResponseType + responseType;
                }
                return responseType;
            }));
        permissions.AddRange(
            dto.Scopes.Select(scope =>
            {
                if (!scope.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope))
                {
                    return OpenIddictConstants.Permissions.Prefixes.Scope + scope;
                }
                return scope;
            }));
        entity.Permissions = jsonSerializer.Serialize(permissions);

        foreach (var extraProperty in dto.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictApplicationDto ToDto(this OpenIddictApplication entity, IJsonSerializer jsonSerializer)
    {
        if (entity == null)
        {
            return null;
        }

        var dto = new OpenIddictApplicationDto
        {
            Id = entity.Id,
            ClientId = entity.ClientId,
            ClientType = entity.ClientType,
            ConsentType = entity.ConsentType,
            DisplayName = entity.DisplayName,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
            DisplayNames = jsonSerializer.DeserializeToDictionary<string, string>(entity.DisplayNames),
            PostLogoutRedirectUris = jsonSerializer.DeserializeToList<string>(entity.PostLogoutRedirectUris),
            Properties = jsonSerializer.DeserializeToDictionary<string, string>(entity.Properties),
            RedirectUris = jsonSerializer.DeserializeToList<string>(entity.RedirectUris),
            ApplicationType = entity.ApplicationType,
            ClientUri = entity.ClientUri,
            LogoUri = entity.LogoUri,
            JsonWebKeySet = entity.JsonWebKeySet,
            ConcurrencyStamp = entity.ConcurrencyStamp,
        };

        var settings = jsonSerializer.DeserializeToDictionary<string, string>(entity.Settings);
        TryGetSettings(settings, dto);

        var requirements = jsonSerializer.DeserializeToList<string>(entity.Requirements);
        TryGetRequirements(requirements, dto);

        var permissions = jsonSerializer.DeserializeToList<string>(entity.Permissions);

        dto.Endpoints = permissions
            .Where(HasPrefixKey(OpenIddictConstants.Permissions.Prefixes.Endpoint))
            .Select(GetPrefixKey(OpenIddictConstants.Permissions.Prefixes.Endpoint))
            .ToList();

        dto.GrantTypes = permissions
            .Where(HasPrefixKey(OpenIddictConstants.Permissions.Prefixes.GrantType))
            .Select(GetPrefixKey(OpenIddictConstants.Permissions.Prefixes.GrantType))
            .ToList();

        dto.ResponseTypes = permissions
            .Where(HasPrefixKey(OpenIddictConstants.Permissions.Prefixes.ResponseType))
            .Select(GetPrefixKey(OpenIddictConstants.Permissions.Prefixes.ResponseType))
            .ToList();

        dto.Scopes = permissions
            .Where(HasPrefixKey(OpenIddictConstants.Permissions.Prefixes.Scope))
            .Select(GetPrefixKey(OpenIddictConstants.Permissions.Prefixes.Scope))
            .ToList();

        foreach (var extraProperty in entity.ExtraProperties)
        {
            dto.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return dto;
    }

    private static Func<string, bool> HasPrefixKey(string prefix)
    {
        return p => p.StartsWith(prefix);
    }

    private static Func<string, string> GetPrefixKey(string prefix)
    {
        return p => p.RemovePreFix(prefix);
    }
    /// <summary>
    /// 尝试获取应用要求
    /// </summary>
    /// <param name="requirements"></param>
    /// <param name="dto"></param>
    private static void TrySetRequirements(IJsonSerializer jsonSerializer, OpenIddictApplicationCreateOrUpdateDto dto, OpenIddictApplication entity)
    {
        var requirements = jsonSerializer.DeserializeToList<string>(entity.Requirements);
        if (dto.Requirements?.Features?.RequirePkce == true)
        {
            requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);
        }
        else
        {
            requirements.RemoveAll(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange.Equals);
        }
        entity.Requirements = jsonSerializer.Serialize(requirements);
    }
    /// <summary>
    /// 尝试获取应用要求
    /// </summary>
    /// <param name="requirements"></param>
    /// <param name="dto"></param>
    private static void TryGetRequirements(List<string> requirements, OpenIddictApplicationDto dto)
    {
        if (requirements.Contains(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange))
        {
            dto.Requirements.Features.RequirePkce = true;
        }
    }
    private static void TrySetSettings(IJsonSerializer jsonSerializer, OpenIddictApplicationCreateOrUpdateDto dto, OpenIddictApplication entity)
    {
        var settings = entity.Settings .IsNullOrWhiteSpace() ? new Dictionary<string, string>()
            : jsonSerializer.DeserializeToDictionary<string, string>(entity.Settings);

        if (dto.Settings != null)
        {
            if (dto.Settings.TokenLifetime != null)
            {
                void TryUpdateTokenLifetime(string key, long? value)
                {
                    if (value.HasValue)
                    {
                        settings[key] = TimeSpan.FromSeconds(value.Value).ToString("c", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        settings.Remove(key);
                    }
                }

                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.AccessToken, dto.Settings.TokenLifetime.AccessToken);
                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode, dto.Settings.TokenLifetime.AuthorizationCode);
                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.DeviceCode, dto.Settings.TokenLifetime.DeviceCode);
                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.IdentityToken, dto.Settings.TokenLifetime.IdentityToken);
                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.RefreshToken, dto.Settings.TokenLifetime.RefreshToken);
                TryUpdateTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.UserCode, dto.Settings.TokenLifetime.UserCode);
            }

            entity.Settings = jsonSerializer.Serialize(settings);
        }
    }
    /// <summary>
    /// 尝试获取应用设置
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="dto"></param>
    private static void TryGetSettings(Dictionary<string, string> settings, OpenIddictApplicationDto dto)
    {
        long? GetTokenLifetime(string key)
        {
            if (settings.TryGetValue(key, out var tokenLifetime) &&
                TimeSpan.TryParse(tokenLifetime, CultureInfo.InvariantCulture, out var tokenLifetimeValue))
            {
                return (long)tokenLifetimeValue.TotalSeconds;
            }
            return null;
        }

        dto.Settings.TokenLifetime.AccessToken = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.AccessToken);
        dto.Settings.TokenLifetime.AuthorizationCode = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.AuthorizationCode);
        dto.Settings.TokenLifetime.DeviceCode = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.DeviceCode);
        dto.Settings.TokenLifetime.IdentityToken = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.IdentityToken);
        dto.Settings.TokenLifetime.RefreshToken = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.RefreshToken);
        dto.Settings.TokenLifetime.UserCode = GetTokenLifetime(OpenIddictConstants.Settings.TokenLifetimes.UserCode);
    }
}
