using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.PermissionManagement.Definitions;

public interface IPermissionDefinitionAppService : IApplicationService
{
    Task<PermissionDefinitionDto> GetAsync(string name);

    Task DeleteAsync(string name);

    Task<PermissionDefinitionDto> CreateAsync(PermissionDefinitionCreateDto input);

    Task<PermissionDefinitionDto> UpdateAsync(string name, PermissionDefinitionUpdateDto input);

    Task<PagedResultDto<PermissionDefinitionDto>> GetListAsync(PermissionDefinitionGetListInput input);
}
