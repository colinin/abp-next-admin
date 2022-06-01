using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Theme.VueVbenAdmin;

public interface IThemeSettingAppService : IApplicationService
{
    Task<ThemeSettingDto> GetAsync();

    Task ChangeAsync(ThemeSettingDto input);
}
