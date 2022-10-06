using Volo.Abp;
using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Scopes;

namespace LINGYUN.Abp.OpenIddict.Scopes;

internal static class OpenIddictScopeExtensions
{
    public static OpenIddictScope ToEntity(this OpenIddictScopeCreateOrUpdateDto dto, OpenIddictScope entity, IJsonSerializer jsonSerializer)
    {
        Check.NotNull(dto, nameof(dto));
        Check.NotNull(entity, nameof(entity));

        entity.Description = dto.Description;
        entity.Descriptions = jsonSerializer.Serialize(dto.Descriptions);
        entity.DisplayName = dto.DisplayName;
        entity.DisplayNames = jsonSerializer.Serialize(dto.DisplayNames);
        entity.Name = dto.Name;
        entity.Properties = jsonSerializer.Serialize(dto.Properties);
        entity.Resources = jsonSerializer.Serialize(dto.Resources);

        foreach (var extraProperty in dto.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictScopeDto ToDto(this OpenIddictScope entity, IJsonSerializer jsonSerializer)
    {
        if (entity == null)
        {
            return null;
        }

        var dto = new OpenIddictScopeDto
        {
            Id = entity.Id,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
            Description = entity.Description,
            Descriptions = jsonSerializer.DeserializeToDictionary<string, string>(entity.Descriptions),
            DisplayName = entity.DisplayName,
            DisplayNames = jsonSerializer.DeserializeToDictionary<string, string>(entity.DisplayNames),
            Name = entity.Name,
            Properties = jsonSerializer.DeserializeToDictionary<string, string>(entity.Properties),
            Resources = jsonSerializer.DeserializeToList<string>(entity.Resources)
        };


        foreach (var extraProperty in entity.ExtraProperties)
        {
            dto.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return dto;
    }
}
