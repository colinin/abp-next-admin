using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Platform.Theme.VueVbenAdmin;

[Controller]
[RemoteService(Name = PlatformRemoteServiceConsts.RemoteServiceName)]
[Route("api/platform/theme/vue-vben-admin")]
public class ThemeSettingController : AbpControllerBase, IThemeSettingAppService
{
    protected IThemeSettingAppService ThemeSettingAppService { get; }

    public ThemeSettingController(
        IThemeSettingAppService themeSettingAppService)
    {
        ThemeSettingAppService = themeSettingAppService;
    }

    [HttpGet]
    public Task<ThemeSettingDto> GetAsync()
    {
        return ThemeSettingAppService.GetAsync();
    }

    [HttpPut]
    [Authorize]
    [Route("change")]
    public Task ChangeAsync(ThemeSettingDto input)
    {
        return ThemeSettingAppService.ChangeAsync(input);
    }
}
