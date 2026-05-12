using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity;

public class OrganizationUnitGetChildrenDto : EntityDto<Guid>
{
    public bool Recursive { get; set; }
}
