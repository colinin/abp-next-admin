using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
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

        entity.ClientSecret = dto.ClientSecret;
        entity.ConsentType = dto.ConsentType;
        entity.DisplayName = dto.DisplayName;
        entity.DisplayNames = jsonSerializer.Serialize(dto.DisplayNames);
        entity.PostLogoutRedirectUris = jsonSerializer.Serialize(dto.PostLogoutRedirectUris);
        entity.Properties = jsonSerializer.Serialize(dto.Properties);
        entity.RedirectUris = jsonSerializer.Serialize(dto.RedirectUris);
        entity.Type = dto.Type;
        entity.ClientUri = dto.ClientUri;
        entity.LogoUri = dto.LogoUri;

        var requirements = new List<string>();
        requirements.AddRange(
            dto.Requirements.Select(requirement =>
            {
                if (!requirement.StartsWith(OpenIddictConstants.Requirements.Prefixes.Feature))
                {
                    return OpenIddictConstants.Requirements.Prefixes.Feature + requirement;
                }
                return requirement;
            }));
        entity.Requirements = jsonSerializer.Serialize(requirements);

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
            ClientSecret = entity.ClientSecret,
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
            Type = entity.Type,
            ClientUri = entity.ClientUri,
            LogoUri = entity.LogoUri
        };

        var requirements = jsonSerializer.DeserializeToList<string>(entity.Requirements);
        dto.Requirements = requirements
            .Where(HasPrefixKey(OpenIddictConstants.Requirements.Prefixes.Feature))
            .Select(GetPrefixKey(OpenIddictConstants.Requirements.Prefixes.Feature))
            .ToList();

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
}
