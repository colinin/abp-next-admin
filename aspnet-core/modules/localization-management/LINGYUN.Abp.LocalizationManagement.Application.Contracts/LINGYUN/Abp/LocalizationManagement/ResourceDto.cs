using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.LocalizationManagement;

public class ResourceDto : AuditedEntityDto<Guid>
{
    public bool Enable { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string DefaultCultureName { get; set; }
}
