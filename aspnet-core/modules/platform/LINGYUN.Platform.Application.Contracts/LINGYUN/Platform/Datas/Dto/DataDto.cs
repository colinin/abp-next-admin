using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Datas;

public class DataDto : EntityDto<Guid>
{
    public string Name { get; set; } = default!;

    public string Code { get; set; } = default!;

    public string DisplayName { get; set; } = default!;

    public string? Description { get; set; }

    public Guid? ParentId { get; set; }

    public List<DataItemDto> Items { get; set; } = new List<DataItemDto>();
}
