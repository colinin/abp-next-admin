using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Account;

[Authorize]
public class MyClaimAppService : AccountApplicationServiceBase, IMyClaimAppService
{
    public MyClaimAppService()
    {

    }

    public async virtual Task ChangeAvatarAsync(ChangeAvatarInput input)
    {
        var user = await GetCurrentUserAsync();

        // TODO: Use AbpClaimTypes.Picture
        user.Claims.RemoveAll(x => x.ClaimType.Equals(IdentityConsts.ClaimType.Avatar.Name));
        user.AddClaim(GuidGenerator, new Claim(IdentityConsts.ClaimType.Avatar.Name, input.AvatarUrl));

        var avatarClaims = user.Claims.Where(x => x.ClaimType.StartsWith(AbpClaimTypes.Picture))
            .Select(x => x.ToClaim())
            .Skip(0)
            .Take(3)
            .ToList();
        if (avatarClaims.Any())
        {
            // 保留最多3个头像
            if (avatarClaims.Count >= 3)
            {
                user.RemoveClaim(avatarClaims.First());
                avatarClaims.RemoveAt(0);
            }

            // 历史头像加数字标识
            for (var index = 1; index <= avatarClaims.Count; index++)
            {
                var avatarClaim = avatarClaims[index - 1];
                var findClaim = user.FindClaim(avatarClaim);
                if (findClaim != null)
                {
                    findClaim.SetClaim(new Claim(
                        AbpClaimTypes.Picture + index.ToString(),
                        findClaim.ClaimValue));
                }
            }
        }

        user.AddClaim(GuidGenerator, new Claim(AbpClaimTypes.Picture, input.AvatarUrl));

        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }
}
