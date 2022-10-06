using Volo.Abp.Json;
using Volo.Abp.OpenIddict.Authorizations;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

internal static class OpenIddictAuthorizationExtensions
{
    public static OpenIddictAuthorizationDto ToDto(this OpenIddictAuthorization entity, IJsonSerializer jsonSerializer)
    {
        if (entity == null)
        {
            return null;
        }

        var dto = new OpenIddictAuthorizationDto
        {
            Id = entity.Id,
            ApplicationId = entity.ApplicationId,
            CreationDate = entity.CreationDate,
            CreationTime = entity.CreationTime,
            CreatorId = entity.CreatorId,
            LastModificationTime = entity.LastModificationTime,
            LastModifierId = entity.LastModifierId,
            Properties = jsonSerializer.DeserializeToDictionary<string, string>(entity.Properties),
            Scopes = jsonSerializer.DeserializeToList<string>(entity.Scopes),
            Status = entity.Status,
            Subject = entity.Subject,
            Type = entity.Type
        };

        foreach (var extraProperty in entity.ExtraProperties)
        {
            dto.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return dto;
    }
}
