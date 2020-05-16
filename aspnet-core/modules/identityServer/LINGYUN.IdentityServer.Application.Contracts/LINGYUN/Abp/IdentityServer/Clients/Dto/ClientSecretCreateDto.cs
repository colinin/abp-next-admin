using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientSecretCreateDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(SecretConsts.TypeMaxLength)]
        public string Type { get; set; }

        [Required]
        [StringLength(SecretConsts.ValueMaxLength)]
        public string Value { get; set; }

        [StringLength(SecretConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
