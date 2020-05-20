using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourcePropertyGetByKeyDto
    {
        [Required]
        public Guid IdentityResourceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }
    }
}
