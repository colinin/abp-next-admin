using System.Threading.Tasks;

namespace LINGYUN.Abp.SettingManagement;
public interface IUserTimeZoneSettingsAppService
{
    Task<string> GetMyTimezoneAsync();

    Task SetMyTimezoneAsync(string timezone);
}
