using LINGYUN.Abp.WeChat.Authorization;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LINGYUN.Abp.IdentityServer
{
    public class WeChatResourceDataSeeder : IWeChatResourceDataSeeder, ITransientDependency
    {
        protected IIdentityClaimTypeRepository ClaimTypeRepository { get; }
        protected IIdentityResourceRepository IdentityResourceRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }

        public WeChatResourceDataSeeder(
            IIdentityResourceRepository identityResourceRepository,
            IGuidGenerator guidGenerator,
            IIdentityClaimTypeRepository claimTypeRepository)
        {
            IdentityResourceRepository = identityResourceRepository;
            GuidGenerator = guidGenerator;
            ClaimTypeRepository = claimTypeRepository;
        }

        public virtual async Task CreateStandardResourcesAsync()
        {
            var wechatClaimTypes = new string[]
            {
                AbpWeChatClaimTypes.AvatarUrl,
                AbpWeChatClaimTypes.City,
                AbpWeChatClaimTypes.Country,
                AbpWeChatClaimTypes.NickName,
                AbpWeChatClaimTypes.OpenId,
                AbpWeChatClaimTypes.Privilege,
                AbpWeChatClaimTypes.Province,
                AbpWeChatClaimTypes.Sex,
                AbpWeChatClaimTypes.UnionId
            };

            var wechatResource = new IdentityServer4.Models.IdentityResource(
                AbpWeChatAuthorizationConsts.ProfileKey, 
                AbpWeChatAuthorizationConsts.DisplayName, 
                wechatClaimTypes);

            foreach (var claimType in wechatClaimTypes)
            {
                await AddClaimTypeIfNotExistsAsync(claimType);
            }

            await AddIdentityResourceIfNotExistsAsync(wechatResource);
        }

        protected virtual async Task AddIdentityResourceIfNotExistsAsync(IdentityServer4.Models.IdentityResource resource)
        {
            if (await IdentityResourceRepository.CheckNameExistAsync(resource.Name))
            {
                return;
            }

            await IdentityResourceRepository.InsertAsync(
                new IdentityResource(
                    GuidGenerator.Create(),
                    resource
                )
            );
        }

        protected virtual async Task AddClaimTypeIfNotExistsAsync(string claimType)
        {
            if (await ClaimTypeRepository.AnyAsync(claimType))
            {
                return;
            }

            await ClaimTypeRepository.InsertAsync(
                new IdentityClaimType(
                    GuidGenerator.Create(),
                    claimType,
                    isStatic: true
                )
            );
        }
    }
}
