using System.Threading.Tasks;

namespace LINGYUN.Abp.SettingManagement
{
    public interface ISettingAppService : IReadonlySettingAppService
    {
        Task SetGlobalAsync(UpdateSettingsDto input);

        Task SetCurrentTenantAsync(UpdateSettingsDto input);
    }
}
