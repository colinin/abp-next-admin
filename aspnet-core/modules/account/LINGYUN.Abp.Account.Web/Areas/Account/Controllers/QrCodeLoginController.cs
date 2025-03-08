using Asp.Versioning;
using LINGYUN.Abp.Account.Web.Areas.Account.Controllers.Models;
using LINGYUN.Abp.Identity.QrCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace LINGYUN.Abp.Account.Web.Areas.Account.Controllers;

[Controller]
[ControllerName("QrCodeLogin")]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}/qrcode")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class QrCodeLoginController : AbpControllerBase
{
    private readonly IdentityUserManager _userManager;
    private readonly IQrCodeLoginProvider _qrCodeLoginProvider;

    public QrCodeLoginController(
        IdentityUserManager userManager, 
        IQrCodeLoginProvider qrCodeLoginProvider)
    {
        _userManager = userManager;
        _qrCodeLoginProvider = qrCodeLoginProvider;
    }

    [HttpPost]
    [Route("generate")]
    [AllowAnonymous]
    public async Task<GenerateQrCodeResult> GenerateAsync()
    {
        var qrCodeInfo = await _qrCodeLoginProvider.GenerateAsync();

        return new GenerateQrCodeResult
        {
            Key = qrCodeInfo.Key,
        };
    }

    [HttpGet]
    [Route("{key}/check")]
    [AllowAnonymous]
    public async Task<QrCodeUserInfoResult> CheckCodeAsync(string key)
    {
        var qrCodeInfo = await _qrCodeLoginProvider.GetCodeAsync(key);

        return new QrCodeUserInfoResult
        {
            Key = qrCodeInfo.Key,
            Status = qrCodeInfo.Status,
            Picture = qrCodeInfo.Picture,
            UserId = qrCodeInfo.UserId,
            UserName = qrCodeInfo.UserName,
            TenantId = qrCodeInfo.TenantId
        };
    }

    [HttpPost]
    [Route("{key}/scan")]
    [Authorize]
    public async Task<QrCodeUserInfoResult> ScanCodeAsync(string key)
    {
        using (CurrentTenant.Change(CurrentUser.TenantId))
        {
            var currentUser = await _userManager.GetByIdAsync(CurrentUser.GetId());

            var userName = CurrentUser.FindClaim(AbpClaimTypes.Name)?.Value ?? currentUser.UserName;
            var userId = await _userManager.GetUserIdAsync(currentUser);

            var qrCodeInfo = await _qrCodeLoginProvider.ScanCodeAsync(key,
                new QrCodeScanParams(userId, userName, currentUser.TenantId));

            return new QrCodeUserInfoResult
            {
                Key = qrCodeInfo.Key,
                Status = qrCodeInfo.Status,
                Picture = qrCodeInfo.Picture,
                UserId = qrCodeInfo.UserId,
                UserName = qrCodeInfo.UserName,
                TenantId = qrCodeInfo.TenantId
            };
        }
    }

    [HttpPost]
    [Route("{key}/confirm")]
    [Authorize]
    public async Task<QrCodeUserInfoResult> ConfirmCodeAsync(string key)
    {
        using (CurrentTenant.Change(CurrentUser.TenantId))
        {
            var qrCodeInfo = await _qrCodeLoginProvider.ConfirmCodeAsync(key);

            return new QrCodeUserInfoResult
            {
                Key = qrCodeInfo.Key,
                Status = qrCodeInfo.Status,
                Picture = qrCodeInfo.Picture,
                UserId = qrCodeInfo.UserId,
                UserName = qrCodeInfo.UserName,
                TenantId = qrCodeInfo.TenantId
            };
        }
    }
}
