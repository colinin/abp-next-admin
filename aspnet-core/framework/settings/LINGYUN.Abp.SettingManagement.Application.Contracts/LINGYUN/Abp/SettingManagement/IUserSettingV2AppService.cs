using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement;

public interface IUserSettingV2AppService : IApplicationService
{
    Task SetAsync(UpdateSettingsDto input);

    Task<SettingGroupResult> GetAsync();
}
