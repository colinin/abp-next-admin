using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TextTemplating;

public interface ITextTemplateDefinitionAppService : IApplicationService
{
    Task<TextTemplateDefinitionDto> GetByNameAsync(string name);

    Task<TextTemplateDefinitionDto> CreateAsync(TextTemplateDefinitionCreateDto input);

    Task<TextTemplateDefinitionDto> UpdateAsync(string name, TextTemplateDefinitionUpdateDto input);

    Task DeleteAsync(string name);

    Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input);
}
