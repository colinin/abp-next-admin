using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TextTemplating;

public interface ITextTemplateAppService : IApplicationService
{
    Task<TextTemplateDefinitionDto> GetAsync(string name);

    Task<TextTemplateContentDto> GetContentAsync(TextTemplateContentGetInput input);

    Task RestoreToDefaultAsync(TextTemplateRestoreInput input);

    Task<TextTemplateDefinitionDto> UpdateAsync(TextTemplateUpdateInput input);

    Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input);
}
