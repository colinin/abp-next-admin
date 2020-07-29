using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Identity
{
    public class OrganizationUnitGetUserDto : IEntityDto<Guid>
    {
        [Required]
        public Guid Id { get; set; }

        public string Filter { get; set; }
    }
}
