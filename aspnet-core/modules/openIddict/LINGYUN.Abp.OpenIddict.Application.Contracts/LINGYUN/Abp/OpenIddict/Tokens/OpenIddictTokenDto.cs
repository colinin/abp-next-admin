using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Serializable]
public class OpenIddictTokenDto : ExtensibleAuditedEntityDto<Guid>
{
    public Guid? ApplicationId { get; set; }

    public Guid? AuthorizationId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string Payload { get; set; }

    public string Properties { get; set; }

    public DateTime? RedemptionDate { get; set; }

    public string ReferenceId { get; set; }

    public string Status { get; set; }

    public string Subject { get; set; }

    public string Type { get; set; }
}
