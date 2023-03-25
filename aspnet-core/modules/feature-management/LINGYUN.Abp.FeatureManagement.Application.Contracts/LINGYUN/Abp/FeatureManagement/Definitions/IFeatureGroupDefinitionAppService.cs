using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public interface IFeatureGroupDefinitionAppService : IApplicationService
{
    Task<FeatureGroupDefinitionDto> GetAsync(string name);

    Task DeleteAsync(string name);

    Task<FeatureGroupDefinitionDto> CreateAsync(FeatureGroupDefinitionCreateDto input);

    Task<FeatureGroupDefinitionDto> UpdateAsync(string name, FeatureGroupDefinitionUpdateDto input);

    Task<PagedResultDto<FeatureGroupDefinitionDto>> GetListAsync(FeatureGroupDefinitionGetListInput input);
}
