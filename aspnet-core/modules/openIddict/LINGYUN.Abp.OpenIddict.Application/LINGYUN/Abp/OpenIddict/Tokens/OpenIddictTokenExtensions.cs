using Volo.Abp;
using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Tokens;

namespace LINGYUN.Abp.OpenIddict.Tokens;

internal static class OpenIddictTokenExtensions
{
    public static OpenIddictToken ToEntity(this OpenIddictTokenDto dto, OpenIddictToken entity, IJsonSerializer jsonSerializer)
    {
        Check.NotNull(dto, nameof(dto));
        Check.NotNull(entity, nameof(entity));

        entity.ApplicationId = dto.ApplicationId;
        entity.AuthorizationId = dto.AuthorizationId;
        entity.CreationDate = dto.CreationDate;
        entity.ExpirationDate = dto.ExpirationDate;
        entity.Payload = dto.Payload;
        entity.Properties = jsonSerializer.Serialize(dto.Properties);
        entity.RedemptionDate = dto.RedemptionDate;
        entity.ReferenceId = dto.ReferenceId;
        entity.Status = dto.Status;
        entity.Subject = dto.Subject;
        entity.Type = dto.Type;

        foreach (var extraProperty in dto.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictTokenDto? ToDto(this OpenIddictToken entity, IJsonSerializer jsonSerializer)
    {
        if (entity == null)
        {
            return null;
        }

        var model = new OpenIddictTokenDto
        {
            Id = entity.Id,
            ApplicationId = entity.ApplicationId,
            AuthorizationId = entity.AuthorizationId,
            CreationDate = entity.CreationDate,
            ExpirationDate = entity.ExpirationDate,
            Payload = entity.Payload,
            RedemptionDate = entity.RedemptionDate,
            ReferenceId = entity.ReferenceId,
            Status = entity.Status,
            Subject = entity.Subject,
            Type = entity.Type,
            Properties = jsonSerializer.DeserializeToDictionary<string, string>(entity.Properties),
        };

        foreach (var extraProperty in entity.ExtraProperties)
        {
            model.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return model;
    }
}
