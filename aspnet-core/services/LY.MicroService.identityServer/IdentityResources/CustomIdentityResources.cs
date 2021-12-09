using IdentityServer4.Models;

namespace LY.MicroService.IdentityServer.IdentityResources;

public class CustomIdentityResources
{
    public class AvatarUrl : IdentityResource
    {
        public static string ClaimType { get; set; } = "avatarUrl";
        public AvatarUrl()
        {
            Name = ClaimType;
            DisplayName = "Your avatar url";
            Emphasize = true;
            UserClaims = new string[] { ClaimType };
        }
    }
}
