using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement;

public interface IReadonlySettingV2AppService : IApplicationService
{
    Task<SettingGroupResult> GetAsync();
}
