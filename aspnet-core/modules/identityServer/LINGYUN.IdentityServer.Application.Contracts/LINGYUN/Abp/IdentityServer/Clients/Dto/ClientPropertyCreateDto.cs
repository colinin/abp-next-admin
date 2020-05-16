using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientPropertyCreateDto
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [StringLength(ClientPropertyConsts.KeyMaxLength)]
        public string Key { get; set; }

        [Required]
        [StringLength(ClientPropertyConsts.ValueMaxLength)]
        public string Value { get; set; }
    }
}
