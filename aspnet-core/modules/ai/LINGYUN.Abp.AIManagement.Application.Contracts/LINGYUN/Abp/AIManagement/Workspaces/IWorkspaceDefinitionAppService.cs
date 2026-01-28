using LINGYUN.Abp.AIManagement.Workspaces.Dtos;
using System;
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
}
