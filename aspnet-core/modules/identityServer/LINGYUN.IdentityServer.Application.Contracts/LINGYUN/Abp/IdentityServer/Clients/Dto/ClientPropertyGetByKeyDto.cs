using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientPropertyGetByKeyDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(ClientPropertyConsts.KeyMaxLength)]
        public string Key { get; set; }
    }
}
