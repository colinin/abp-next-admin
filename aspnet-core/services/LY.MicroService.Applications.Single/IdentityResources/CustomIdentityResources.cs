namespace LY.MicroService.Applications.Single.IdentityResources;

public class CustomIdentityResources
{
    public class AvatarUrl : IdentityServer4.Models.IdentityResource
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
