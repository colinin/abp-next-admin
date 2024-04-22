using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement;

public interface ISettingDefinitionAppService : IApplicationService
{
    Task<SettingDefinitionDto> GetAsync(string name);

    Task<ListResultDto<SettingDefinitionDto>> GetListAsync(SettingDefinitionGetListInput input);

    Task<SettingDefinitionDto> CreateAsync(SettingDefinitionCreateDto input);

    Task<SettingDefinitionDto> UpdateAsync(string name, SettingDefinitionUpdateDto input);

    Task DeleteOrRestoreAsync(string name);
}
