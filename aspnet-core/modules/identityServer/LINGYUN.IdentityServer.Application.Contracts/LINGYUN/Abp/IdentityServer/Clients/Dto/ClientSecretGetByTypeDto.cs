using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientSecretGetByTypeDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(SecretConsts.TypeMaxLength)]
        public string Type { get; set; }
    }
}
