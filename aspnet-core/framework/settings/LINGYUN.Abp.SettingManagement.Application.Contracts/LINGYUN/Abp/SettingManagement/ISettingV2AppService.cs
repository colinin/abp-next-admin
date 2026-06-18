using System.Threading.Tasks;

namespace LINGYUN.Abp.SettingManagement;

public interface ISettingV2AppService : IReadonlySettingV2AppService
{
    Task SetAsync(UpdateSettingsDto input);
}
