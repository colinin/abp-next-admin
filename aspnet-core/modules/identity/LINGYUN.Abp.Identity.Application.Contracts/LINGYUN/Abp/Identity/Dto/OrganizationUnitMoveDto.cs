using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitMoveDto : IEntityDto<Guid>
    {
        [Required]
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
    }
}
