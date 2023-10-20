using LINGYUN.Abp.WxPusher.Security.Claims;
using LINGYUN.Abp.WxPusher.User;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Identity.WxPusher.User;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IWxPusherUserStore))]
public class IdentityWxPusherUserStore : IWxPusherUserStore
{
    protected IdentityUserManager UserManager { get; }

    public IdentityWxPusherUserStore(IdentityUserManager userManager)
    {
        UserManager = userManager;
    }

    public async virtual Task<List<int>> GetSubscribeTopicsAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var topics = new List<int>();

        foreach (var userId in userIds)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            var userTopicClaim = user?.Claims
                .Where(c => c.ClaimType.Equals(AbpWxPusherClaimTypes.Topic))
                .FirstOrDefault();

            if (userTopicClaim != null &&
                int.TryParse(userTopicClaim.ClaimValue, out var topic))
            {
                topics.Add(topic);
            }
        }

        return topics.Distinct().ToList();
    }

    public async virtual Task<List<string>> GetBindUidsAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var uids = new List<string>();

        foreach (var userId in userIds)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());

            var userUidClaim = user?.Claims
                .Where(c => c.ClaimType.Equals(AbpWxPusherClaimTypes.Uid))
                .FirstOrDefault();

            if (userUidClaim != null)
            {
                uids.Add(userUidClaim.ClaimValue);
            }
        }

        return uids.Distinct().ToList();
    }
}
