using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientUpdateInputDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public ClientUpdateDto Client { get; set; }

        public ClientUpdateInputDto()
        {
            Client = new ClientUpdateDto();
        }
    }
}
