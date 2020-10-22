using System.Collections.Generic;

namespace LINGYUN.Abp.IdentityServer.ApiResources
{
    public class ApiScopeDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }

        public ApiScopeDto()
        {
            UserClaims = new List<string>();
        }
    }
}
