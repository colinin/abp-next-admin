using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OpenIddict.Tokens;

[Serializable]
public class OpenIddictTokenGetListInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? AuthorizationId { get; set; }
    public string Subject { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string ReferenceId { get; set; }
    public DateTime? BeginExpirationDate { get; set; }
    public DateTime? EndExpirationDate { get; set; }
    public DateTime? BeginCreationTime { get; set; }
    public DateTime? EndCreationTime { get; set; }
}
