using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public class BlobContainerDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
}
