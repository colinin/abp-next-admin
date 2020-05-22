using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiSecretCreateDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [StringLength(SecretConsts.TypeMaxLength)]
        public string Type { get; set; }

        public HashType HashType { get; set; }

        [Required]
        [StringLength(SecretConsts.ValueMaxLength)]
        public string Value { get; set; }

        [StringLength(SecretConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime? Expiration { get; set; }

        public ApiSecretCreateDto()
        {
            HashType = 0;
        }
    }
}
