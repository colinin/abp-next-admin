using IdentityServer4.Models;
using LINGYUN.Abp.Identity;

namespace LY.MicroService.IdentityServer.IdentityResources;

public class CustomIdentityResources
{
    public class AvatarUrl : IdentityResource
    {
        public AvatarUrl()
        {
            Name = IdentityConsts.ClaimType.Avatar.Name;
            DisplayName = IdentityConsts.ClaimType.Avatar.DisplayName;
            Description = IdentityConsts.ClaimType.Avatar.Description;
            Emphasize = true;
            UserClaims = new string[] { IdentityConsts.ClaimType.Avatar.Name };
        }
    }
}
