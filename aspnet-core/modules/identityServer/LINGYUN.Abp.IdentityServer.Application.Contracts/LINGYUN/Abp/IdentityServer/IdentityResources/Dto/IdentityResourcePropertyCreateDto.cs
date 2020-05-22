using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourcePropertyCreateDto
    {
        [Required]
        public Guid IdentityResourceId { get; set; }

        [Required]
        [StringLength(2000)]
        public string ConcurrencyStamp { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        [Required]
        [StringLength(2000)]
        public string Value { get; set; }
    }
}
