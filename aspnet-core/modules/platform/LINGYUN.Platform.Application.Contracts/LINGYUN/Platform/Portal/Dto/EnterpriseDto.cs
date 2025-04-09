using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Platform.Portal;

public class EnterpriseDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }
    public string EnglishName { get; set; }
    public string Logo { get; set; }
    public string Address { get; set; }
    public string LegalMan { get; set; }
    public string TaxCode { get; set; }
    public string OrganizationCode { get; set; }
    public string RegistrationCode { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string ConcurrencyStamp { get; set; }
}
