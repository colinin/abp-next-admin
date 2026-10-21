using LINGYUN.Abp.Captcha;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Account.Web.Areas.LazyCaptcha.Account.Controllers;

[Controller]
[Area(AccountRemoteServiceConsts.ModuleName)]
[Route($"api/{AccountRemoteServiceConsts.ModuleName}/captcha/lazy")]
[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
public class LazyCaptchaController : AbpControllerBase
{
    private readonly ICodeCaptchaProvider _codeCaptchaProvider;

    public LazyCaptchaController(ICodeCaptchaProvider codeCaptchaProvider)
    {
        _codeCaptchaProvider = codeCaptchaProvider;
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshAsync()
    {
        var captchaId = EnsureCaptchaIdCookie();
        var captchaData = await _codeCaptchaProvider.GenerateAsync(captchaId);
        var base64 = Convert.ToBase64String(captchaData.Data);

        return new JsonResult(new { captchaImage = $"data:image/png;base64,{base64}" });
    }

    protected virtual string EnsureCaptchaIdCookie()
    {
        if (!Request.Cookies.TryGetValue(CaptchaKeywords.CaptchaIdCookieName, out var captchaId))
        {
            captchaId = GuidGenerator.Create().ToString("N");
        }

        Response.Cookies.Append(
            CaptchaKeywords.CaptchaIdCookieName,
            captchaId,
            new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                IsEssential = true,
                Expires = DateTimeOffset.Now.AddHours(1)
            });

        return captchaId;
    }
}
