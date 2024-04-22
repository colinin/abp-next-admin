using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.FeatureManagement.Definitions;

public interface IFeatureDefinitionAppService : IApplicationService
{
    Task<FeatureDefinitionDto> GetAsync(string name);

    Task DeleteAsync(string name);

    Task<FeatureDefinitionDto> CreateAsync(FeatureDefinitionCreateDto input);

    Task<FeatureDefinitionDto> UpdateAsync(string name, FeatureDefinitionUpdateDto input);

    Task<ListResultDto<FeatureDefinitionDto>> GetListAsync(FeatureDefinitionGetListInput input);
}
