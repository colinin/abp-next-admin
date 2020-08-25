using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Identity
{
    public class IdentityUserOrganizationUnitUpdateDto
    {
        [Required]
        public Guid[] OrganizationUnitIds { get; set; }
    }
}
