using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Identity
{
    public class IdentityRoleAddOrRemoveOrganizationUnitDto
    {
        [Required]
        public Guid[] OrganizationUnitIds { get; set; }
    }
}
