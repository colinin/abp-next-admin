using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public Guid? EditionId { get; set; }

    public string EditionName { get; set; }

    public bool IsActive { get; set; }

    public DateTime? EnableTime { get; set; }

    public DateTime? DisableTime { get; set; }
    public string ConcurrencyStamp { get; set; }
}