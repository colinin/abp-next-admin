using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Saas.Editions;

public class EditionDto : ExtensibleFullAuditedEntityDto<Guid>
{
    public string DisplayName { get; set; }
}
