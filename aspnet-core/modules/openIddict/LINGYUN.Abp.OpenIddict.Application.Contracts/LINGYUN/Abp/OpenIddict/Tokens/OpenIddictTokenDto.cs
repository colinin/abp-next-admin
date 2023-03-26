using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Serializable]
public class OpenIddictTokenDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }

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
