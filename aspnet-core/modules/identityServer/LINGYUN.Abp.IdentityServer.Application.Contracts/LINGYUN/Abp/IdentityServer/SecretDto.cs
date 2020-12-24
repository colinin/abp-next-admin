using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.IdentityServer
{
    public class SecretDto
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
