using System;

namespace LINGYUN.Abp.IdentityServer
{
    public class SecretBaseDto
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public DateTime? Expiration { get; set; }
    }
}
