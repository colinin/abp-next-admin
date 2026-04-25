using LINGYUN.Abp.AIManagement.Tools.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement.Tools;
public interface IAIToolDefinitionAppService :
    ICrudAppService<
        AIToolDefinitionRecordDto,
        Guid,
        AIToolDefinitionRecordGetPagedListInput,
        AIToolDefinitionRecordCreateDto,
        AIToolDefinitionRecordUpdateDto>
{
    Task<ListResultDto<AIToolProviderDto>> GetAvailableProvidersAsync();
}
