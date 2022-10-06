using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Authorizations;

[Serializable]
public class OpenIddictAuthorizationGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public string Subject { get; set; }
    public Guid? ClientId { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public DateTime? BeginCreationTime { get; set; }
    public DateTime? EndCreationTime { get; set; }
}
