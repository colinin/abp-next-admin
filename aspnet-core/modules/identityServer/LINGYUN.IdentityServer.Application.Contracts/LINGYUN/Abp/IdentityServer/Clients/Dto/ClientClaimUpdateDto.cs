using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientClaimUpdateDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(ClientClaimConsts.TypeMaxLength)]
        public string Type { get; set; }

        [Required]
        [StringLength(ClientClaimConsts.ValueMaxLength)]
        public string Value { get; set; }
    }
}
