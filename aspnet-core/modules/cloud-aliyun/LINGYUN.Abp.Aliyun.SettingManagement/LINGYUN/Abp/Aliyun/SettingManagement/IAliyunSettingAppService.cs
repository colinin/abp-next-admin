using LINGYUN.Abp.SettingManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Aliyun.SettingManagement
{
    public interface IAliyunSettingAppService
    {
        Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync();

        Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync();
    }
}
