using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.LocalizationManagement;

public interface IResourceAppService : IApplicationService
{
    Task<ResourceDto> GetByNameAsync(string name);

    Task<ResourceDto> CreateAsync(ResourceCreateDto input);

    Task<ResourceDto> UpdateAsync(string name, ResourceUpdateDto input);

    Task<PagedResultDto<ResourceDto>> GetListAsync(ResourceGetPagedListInput input);

    Task DeleteAsync(string name);
}
