using LINGYUN.Abp.Identity;
using OpenIddict.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using VoloUserInfoController = Volo.Abp.OpenIddict.Controllers.UserInfoController;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Controllers;

[ExposeServices(
    typeof(VoloUserInfoController),
    typeof(UserInfoController))]
public class UserInfoController : VoloUserInfoController
{
    protected async override Task<Dictionary<string, object>> GetUserInfoClaims()
    {
        var user = await UserManager.GetUserAsync(User);
        if (user == null)
        {
            return null;
        }

        var claims = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
            [OpenIddictConstants.Claims.Subject] = await UserManager.GetUserIdAsync(user)
        };

        if (User.HasScope(OpenIddictConstants.Scopes.Profile))
        {
            claims[AbpClaimTypes.TenantId] = user.TenantId;
            claims[OpenIddictConstants.Claims.PreferredUsername] = user.UserName;
            claims[OpenIddictConstants.Claims.FamilyName] = user.Surname;
            claims[OpenIddictConstants.Claims.GivenName] = user.Name;
            // 重写添加用户头像
            var picture = user.Claims.FirstOrDefault(x => x.ClaimType == IdentityConsts.ClaimType.Avatar.Name);
            if (picture != null)
            {
                claims[OpenIddictConstants.Claims.Picture] = picture.ClaimValue;
            }
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Email))
        {
            claims[OpenIddictConstants.Claims.Email] = await UserManager.GetEmailAsync(user);
            claims[OpenIddictConstants.Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Phone))
        {
            claims[OpenIddictConstants.Claims.PhoneNumber] = await UserManager.GetPhoneNumberAsync(user);
            claims[OpenIddictConstants.Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
        }

        if (User.HasScope(OpenIddictConstants.Scopes.Roles))
        {
            claims[OpenIddictConstants.Claims.Role] = await UserManager.GetRolesAsync(user);
        }

        // Note: the complete list of standard claims supported by the OpenID Connect specification
        // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

        return claims;
    }
}
