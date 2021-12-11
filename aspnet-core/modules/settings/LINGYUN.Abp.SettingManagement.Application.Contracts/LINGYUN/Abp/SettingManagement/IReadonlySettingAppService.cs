using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement
{
    public interface IReadonlySettingAppService : IApplicationService
    {
        Task<SettingGroupResult> GetAllForGlobalAsync();

        Task<SettingGroupResult> GetAllForCurrentTenantAsync();
    }
}
