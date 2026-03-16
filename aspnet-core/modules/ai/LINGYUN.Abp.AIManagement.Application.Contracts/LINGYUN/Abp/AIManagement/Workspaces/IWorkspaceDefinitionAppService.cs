using LINGYUN.Abp.AIManagement.Workspaces.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public interface IWorkspaceDefinitionAppService :
    ICrudAppService<
        WorkspaceDefinitionRecordDto,
        Guid,
        WorkspaceDefinitionRecordGetListInput,
        WorkspaceDefinitionRecordCreateDto,
        WorkspaceDefinitionRecordUpdateDto>
{
    Task<ListResultDto<ChatClientProviderDto>> GetAvailableProvidersAsync();
}
