using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiSecretGetByTypeInputDto
    {
        [Required]
        public Guid ApiResourceId { get; set; }

        [Required]
        [StringLength(SecretConsts.TypeMaxLength)]
        public string Type { get; set; }

        [Required]
        [StringLength(SecretConsts.ValueMaxLength)]
        public string Value { get; set; }
    }
}
