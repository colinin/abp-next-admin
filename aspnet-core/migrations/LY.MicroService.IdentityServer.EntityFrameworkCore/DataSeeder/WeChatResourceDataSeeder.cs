﻿using LINGYUN.Abp.WeChat;
using LINGYUN.Abp.WeChat.Common.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.IdentityResources;

namespace LY.MicroService.IdentityServer.EntityFrameworkCore.DataSeeder;

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

    public async virtual Task CreateStandardResourcesAsync()
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
            AbpWeChatGlobalConsts.ProfileKey,
            AbpWeChatGlobalConsts.DisplayName,
            wechatClaimTypes);

        foreach (var claimType in wechatClaimTypes)
        {
            await AddClaimTypeIfNotExistsAsync(claimType);
        }

        await AddIdentityResourceIfNotExistsAsync(wechatResource);
    }

    protected async virtual Task AddIdentityResourceIfNotExistsAsync(IdentityServer4.Models.IdentityResource resource)
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

    protected async virtual Task AddClaimTypeIfNotExistsAsync(string claimType)
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
