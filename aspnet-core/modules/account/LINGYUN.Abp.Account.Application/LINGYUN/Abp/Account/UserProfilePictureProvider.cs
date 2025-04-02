using LINGYUN.Abp.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.Account;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IUserPictureProvider))]
public class UserProfileUserPictureProvider : IUserPictureProvider
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IdentityUserManager UserManager { get; }
    protected IBlobContainer<AccountContainer> AccountBlobContainer { get; }
    public UserProfileUserPictureProvider(
        IGuidGenerator guidGenerator,
        IdentityUserManager userManager,
        IBlobContainer<AccountContainer> accountBlobContainer)
    {
        UserManager = userManager;
        GuidGenerator = guidGenerator;
        AccountBlobContainer = accountBlobContainer;
    }

    public async virtual Task SetPictureAsync(IdentityUser user, Stream stream, string fileName = null)
    {
        var userId = user.Id.ToString("N");
        var pictureBlobId = fileName ?? $"{GuidGenerator.Create():n}.jpg";

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

        user.AddClaim(GuidGenerator, new Claim(AbpClaimTypes.Picture, pictureBlobId));

        (await UserManager.UpdateAsync(user)).CheckErrors();

        var pictureName = $"{userId}/avatar/{pictureBlobId}";

        await AccountBlobContainer.SaveAsync(pictureName, stream, true);
    }

    public async virtual Task<Stream> GetPictureAsync(string userId)
    {
        var user = await UserManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Stream.Null;
        }

        var picture = user.Claims
            .FirstOrDefault(x => x.ClaimType == AbpClaimTypes.Picture)
            ?.ClaimValue;

        if (picture.IsNullOrWhiteSpace())
        {
            return Stream.Null;
        }

        var pictureName = $"{user.Id:N}/avatar/{picture}";

        return await AccountBlobContainer.ExistsAsync(pictureName)
            ? await AccountBlobContainer.GetAsync(pictureName)
            : Stream.Null;
    }
}
