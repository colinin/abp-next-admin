using LINGYUN.Abp.SettingManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.OssManagement.SettingManagement
{
    public interface IOssManagementSettingAppService
    {
        Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync();

        Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync();
    }
}
