using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.ApiGateway.Ocelot
{
    public class AuthenticationOptions : Entity<int>
    {
        public virtual long ReRouteId { get; private set; }
        public virtual string AuthenticationProviderKey { get; private set; }
        public virtual string AllowedScopes { get; set; }
        public virtual ReRoute ReRoute { get; private set; }

        protected AuthenticationOptions()
        {

        }
        public AuthenticationOptions(long rerouteId)
        {
            ReRouteId = rerouteId;
        }

        public void ApplyAuthOptions(string key, List<string> allowScopes)
        {
            AuthenticationProviderKey = key;
            SetAllowScopes(allowScopes);
        }

        public void SetAllowScopes(List<string> allowScopes)
        {
            if(allowScopes != null && allowScopes.Count > 0)
            {
                AllowedScopes = allowScopes.JoinAsString(",");
            }
            else
            {
                AllowedScopes = "";
            }
        }
    }
}
