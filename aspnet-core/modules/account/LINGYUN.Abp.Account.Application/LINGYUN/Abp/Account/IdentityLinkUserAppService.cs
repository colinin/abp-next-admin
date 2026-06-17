using LINGYUN.Abp.Account.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account;

[Authorize]
public class IdentityLinkUserAppService : AccountApplicationServiceBase, IIdentityLinkUserAppService
{
    protected IdentityLinkUserManager IdentityLinkUserManager { get; }

    protected ITenantStore TenantStore { get; }

    public IdentityLinkUserAppService(
        IdentityLinkUserManager identityLinkUserManager,
        ITenantStore tenantStore)
    {
        IdentityLinkUserManager = identityLinkUserManager;
        TenantStore = tenantStore;
    }

    public async virtual Task<string> GenerateLinkLoginTokenAsync()
    {
        return await IdentityLinkUserManager.GenerateLinkTokenAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            LinkUserTokenProviderConsts.LinkUserLoginTokenPurpose);
    }

    public async virtual Task<string> GenerateLinkTokenAsync()
    {
        return await IdentityLinkUserManager.GenerateLinkTokenAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);
    }

    public async virtual Task<ListResultDto<LinkUserDto>> GetListAsync()
    {
        var identityLinkUserDtos = new List<LinkUserDto>();
        var identityLinkUserInfo = new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id);
        var identityLinkUsers = await IdentityLinkUserManager.GetListAsync(identityLinkUserInfo, includeIndirect: true);

        var currentUserId = CurrentUser.GetId();
        var currentTenantId = CurrentTenant.Id;

        var allActiveTenants = (await TenantStore.GetListAsync()).Where(x => x.IsActive);
        var allIdentityLinkUserDtos = identityLinkUsers
            .SelectMany(x => new[]
            {
                new LinkUserDto
                {
                    LinkTenantId = x.TargetTenantId,
                    LinkUserId = x.TargetUserId,
                    DirectlyLinked = (x.SourceTenantId == currentTenantId && x.SourceUserId == currentUserId)
                                  || (x.TargetTenantId == currentTenantId && x.TargetUserId == currentUserId)
                },
                new LinkUserDto
                {
                    LinkTenantId = x.SourceTenantId,
                    LinkUserId = x.SourceUserId,
                    DirectlyLinked = (x.SourceTenantId == currentTenantId && x.SourceUserId == currentUserId)
                                  || (x.TargetTenantId == currentTenantId && x.TargetUserId == currentUserId)
                }
            })
            .Where(x => x.LinkTenantId != currentTenantId || x.LinkUserId != currentUserId)
            .GroupBy(x => new { x.LinkTenantId, x.LinkUserId })
            .Select(g => g.FirstOrDefault(x => x.DirectlyLinked) ?? g.First())
            .ToList();

        foreach (var identityLinkUserDto in allIdentityLinkUserDtos)
        {
            var linkTenant = allActiveTenants.FirstOrDefault(x => x.Id == identityLinkUserDto.LinkTenantId);
            using (CurrentTenant.Change(linkTenant?.Id, linkTenant?.Name))
            {
                var linkUser = await UserManager.FindByIdAsync(identityLinkUserDto.LinkUserId.ToString());
                if (linkUser != null)
                {
                    identityLinkUserDto.LinkUserName = linkUser.UserName;
                    identityLinkUserDtos.Add(
                        new LinkUserDto
                        {
                            LinkTenantId = linkTenant?.Id,
                            LinkTenantName = linkTenant?.Name,
                            LinkUserId = linkUser.Id,
                            LinkUserName = linkUser.UserName,
                            DirectlyLinked = identityLinkUserDto.DirectlyLinked,
                        });
                }
            }
        }

        return new ListResultDto<LinkUserDto>(identityLinkUserDtos);
    }

    public async virtual Task LinkAsync(LinkUserInput input)
    {
        var identityLinkUserInfo = new IdentityLinkUserInfo(input.UserId, input.TenantId);
        if (await IdentityLinkUserManager.VerifyLinkTokenAsync(identityLinkUserInfo, input.Token, LinkUserTokenProviderConsts.LinkUserTokenPurpose))
        {
            await IdentityLinkUserManager.LinkAsync(
                new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
                identityLinkUserInfo);
            return;
        }

        throw new BusinessException(
            LINGYUN.Abp.Identity.IdentityErrorCodes.LinkUserTokenInValid,
            "Link user token is invalid!");
    }

    public async virtual Task UnlinkAsync(UnLinkUserInput input)
    {
        await IdentityLinkUserManager.UnlinkAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            new IdentityLinkUserInfo(input.UserId, input.TenantId));
    }

    public async virtual Task<VerifyLinkUserDto> VerifyLinkUserAsync(VerifyLinkUserInput input)
    {
        if (await IdentityLinkUserManager.IsLinkedAsync(
            new IdentityLinkUserInfo(CurrentUser.GetId(), CurrentTenant.Id),
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            includeIndirect: true))
        {
            using (CurrentTenant.Change(input.TenantId))
            {
                TenantConfiguration tenant = null;
                if (input.TenantId.HasValue)
                {
                    tenant = await TenantStore.FindAsync(input.TenantId.Value);
                }
                var user = await UserManager.FindByIdAsync(input.UserId.ToString());

                return new VerifyLinkUserDto
                {
                    LinkTenantId = tenant?.Id,
                    LinkTenantName = tenant?.Name,
                    LinkUserId = user?.Id,
                    LinkUserName = user?.UserName,
                    IsLinked = user != null
                };
            }
        }
        return new VerifyLinkUserDto { IsLinked = false };
    }

    [AllowAnonymous]
    public async virtual Task<bool> VerifyLinkLoginTokenAsync(VerifyLinkTokenInput input)
    {
        return await IdentityLinkUserManager.VerifyLinkTokenAsync(
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            input.Token,
            LinkUserTokenProviderConsts.LinkUserLoginTokenPurpose);
    }

    [AllowAnonymous]
    public async virtual Task<bool> VerifyLinkTokenAsync(VerifyLinkTokenInput input)
    {
        return await IdentityLinkUserManager.VerifyLinkTokenAsync(
            new IdentityLinkUserInfo(input.UserId, input.TenantId),
            input.Token,
            LinkUserTokenProviderConsts.LinkUserTokenPurpose);
    }
}
