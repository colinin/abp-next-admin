using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Serializable]
public class OpenIddictTokenDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;

    public Guid? ApplicationId { get; set; }

    public Guid? AuthorizationId { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public string? Payload { get; set; }

    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

    public DateTime? RedemptionDate { get; set; }

    public string? ReferenceId { get; set; }

    public string? Status { get; set; }

    public string? Subject { get; set; }

    public string? Type { get; set; }
}
