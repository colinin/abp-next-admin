using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.IdentityServer.Clients
{
    public class ClientGetByIdInputDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
