using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Tenants;

public class TenantDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public Guid? EditionId { get; set; }

    public string EditionName { get; set; }

    public bool IsActive { get; set; }

    public DateTime? EnableTime { get; set; }

    public DateTime? DisableTime { get; set; }
}