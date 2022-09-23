using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Menus;

public class UserFavoriteMenuDto : AuditedEntityDto<Guid>
{
    public Guid MenuId { get; set; }

    public Guid UserId { get; set; }

    public string Framework { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Path { get; set; }

    public string Icon { get; set; }
}
