using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientCreateDto
    {
        [Required]
        [StringLength(ClientConsts.ClientIdMaxLength)]
        public string ClientId { get; set; }

        [Required]
        [StringLength(ClientConsts.ClientNameMaxLength)]
        public string ClientName { get; set; }

        [StringLength(ClientConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; }
        public ClientCreateDto()
        {
            AllowedGrantTypes = new List<ClientGrantTypeDto>();
        }
    }
}
