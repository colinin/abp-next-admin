using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientClaimGetByKeyInputDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(ClientClaimConsts.TypeMaxLength)]
        public string Type { get; set; }
    }
}
