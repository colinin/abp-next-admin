using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Elsa.Designer.Areas.Elsa;

[ApiController]
[ApiVersion("1")]
[Route("v{apiVersion:apiVersion}/ElsaAuthentication/options")]
[Produces("application/json")]
public class ElsaAuthenticationContextController : AbpControllerBase
{
    public static string CurrentTenantAccessorName { get; internal set; } = nameof(AbpTenantAccessor);
    public static string TenantAccessorKeyName { get; internal set; } = "__tenant";
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Handle()
    {
        return Ok(new
        {
            AuthenticationStyles = new List<string> 
            {
                "ServerManagedCookie" // Cookie
            },
            CurrentTenantAccessorName,
            TenantAccessorKeyName
        });
    }
}
