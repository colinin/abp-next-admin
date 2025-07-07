using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Elsa.Designer.Areas.Elsa;

[ApiController]
[ApiVersion("1")]
[Route("v{apiVersion:apiVersion}/ElsaAuthentication/UserInfo")]
[Produces("application/json")]
public class ElsaUserInfoController : AbpControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual IActionResult Handle()
    {
        return Ok(
            new { 
                IsAuthenticated = CurrentUser.IsAuthenticated,
                name = CurrentUser.Name ?? CurrentUser.UserName,
                tenantId = CurrentUser.TenantId
            });
    }
}
