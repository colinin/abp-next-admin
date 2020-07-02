using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiSecretGetByTypeInputDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [DynamicStringLength(typeof(SecretConsts), nameof(SecretConsts.TypeMaxLength))]
        public string Type { get; set; }

        [Required]
        [DynamicStringLength(typeof(SecretConsts), nameof(SecretConsts.ValueMaxLength))]
        public string Value { get; set; }
    }
}
