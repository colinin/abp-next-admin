using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

[Serializable]
public class OpenIddictAuthorizationDto : ExtensibleAuditedEntityDto<Guid>
{
    public Guid? ApplicationId { get; set; }
    public DateTime? CreationDate { get; set; }
    public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    public List<string> Scopes { get; set; } = new List<string>();
    public string Status { get; set; }
    public string Subject { get; set; }
    public string Type { get; set; }
}
