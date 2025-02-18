using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.Gdpr;

public class GdprRequestDto : EntityDto<Guid>, IHasCreationTime
{
    public DateTime CreationTime { get; set; }
    public DateTime ReadyTime { get; set; }
}
