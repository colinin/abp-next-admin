using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitDtoAddOrRemoveRoleDto : IEntityDto<Guid>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
