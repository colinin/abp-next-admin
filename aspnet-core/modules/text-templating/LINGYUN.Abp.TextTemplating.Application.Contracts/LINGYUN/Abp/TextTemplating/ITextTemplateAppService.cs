using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.TextTemplating;

public interface ITextTemplateAppService : IApplicationService
{
    Task<TextTemplateDto> GetAsync(TextTemplateGetInput input);

    Task ResetDefaultAsync(TextTemplateGetInput input);

    Task<TextTemplateDto> UpdateAsync(TextTemplateUpdateInput input);

    Task<ListResultDto<TextTemplateDto>> GetListAsync();
}
