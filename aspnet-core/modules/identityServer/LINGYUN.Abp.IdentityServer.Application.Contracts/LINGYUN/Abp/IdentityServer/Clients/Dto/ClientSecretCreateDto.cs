using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientSecretCreateDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [DynamicStringLength(typeof(SecretConsts), nameof(SecretConsts.TypeMaxLength))]
        public string Type { get; set; }

        public HashType HashType { get; set; }

        [Required]
        [DynamicStringLength(typeof(SecretConsts), nameof(SecretConsts.ValueMaxLength))]
        public string Value { get; set; }

        [DynamicStringLength(typeof(SecretConsts), nameof(SecretConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public DateTime? Expiration { get; set; }

        public ClientSecretCreateDto()
        {
            HashType = 0;
        }
    }
}
