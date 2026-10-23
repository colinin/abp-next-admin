using System;
using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

public interface IFormDefinitionAppService : IApplicationService
{
    Task<PagedResultDto<FormDefinitionDto>> GetPagedListAsync(FormDefinitionGetPagedListInput input);

    Task<FormDefinitionDto> GetAsync(Guid id);

    Task<FormDefinitionDto> CreateAsync(FormDefinitionCreateDto input);

    Task<FormDefinitionDto> UpdateAsync(Guid id, FormDefinitionUpdateDto input);

    Task DeleteAsync(Guid id);

    Task<FormDefinitionDto> PublishAsync(Guid id);

    Task<FormDefinitionDto> AddFieldAsync(Guid id, FormFieldCreateDto input);

    Task<FormDefinitionDto> UpdateFieldAsync(Guid id, Guid fieldId, FormFieldUpdateDto input);

    Task<FormDefinitionDto> RemoveFieldAsync(Guid id, Guid fieldId);

    Task<FormDataDto> SaveDataAsync(Guid id, string instanceId, FormDataSaveDto input);

    Task<FormDataDto?> GetDataAsync(Guid id, string instanceId);
}
