using LINGYUN.Abp.AIManagement.Localization;
using LINGYUN.Abp.AIManagement.Workspaces.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Security.Encryption;

namespace LINGYUN.Abp.AIManagement.Workspaces;
public class WorkspaceDefinitionAppService :
    CrudAppService<
        WorkspaceDefinitionRecord,
        WorkspaceDefinitionRecordDto,
        Guid,
        WorkspaceDefinitionRecordGetListInput,
        WorkspaceDefinitionRecordCreateDto,
        WorkspaceDefinitionRecordUpdateDto>,
    IWorkspaceDefinitionAppService
{
    protected IStringEncryptionService StringEncryptionService { get; }
    protected IWorkspaceDefinitionRecordRepository WorkspaceDefinitionRecordRepository { get; }
    public WorkspaceDefinitionAppService(
        IStringEncryptionService stringEncryptionService,
        IWorkspaceDefinitionRecordRepository repository) : base(repository)
    {
        StringEncryptionService = stringEncryptionService;
        WorkspaceDefinitionRecordRepository = repository;

        LocalizationResource = typeof(AIManagementResource);
        ObjectMapperContext = typeof(AbpAIManagementApplicationModule);
    }

    protected async override Task<IQueryable<WorkspaceDefinitionRecord>> CreateFilteredQueryAsync(WorkspaceDefinitionRecordGetListInput input)
    {
        var queryable = await base.CreateFilteredQueryAsync(input);

        return queryable
            .WhereIf(!input.Provider.IsNullOrWhiteSpace(), x => x.Provider == input.Provider)
            .WhereIf(!input.ModelName.IsNullOrWhiteSpace(), x => x.ModelName == input.ModelName)
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Provider.Contains(input.Filter!) ||
                x.ModelName.Contains(input.Filter!) || x.DisplayName.Contains(input.Filter!) ||
                (!x.Description.IsNullOrWhiteSpace() && x.Description.Contains(input.Filter!)) || 
                (!x.SystemPrompt.IsNullOrWhiteSpace() && x.SystemPrompt.Contains(input.Filter!)) ||
                (!x.Instructions.IsNullOrWhiteSpace() && x.Instructions.Contains(input.Filter!)));
    }

    protected async override Task<WorkspaceDefinitionRecord> MapToEntityAsync(WorkspaceDefinitionRecordCreateDto createInput)
    {
        if (await WorkspaceDefinitionRecordRepository.FindByNameAsync(createInput.Name) != null)
        {
            throw new WorkspaceAlreadyExistsException(createInput.Name);
        }

        var record = new WorkspaceDefinitionRecord(
            GuidGenerator.Create(),
            createInput.Name,
            createInput.Provider,
            createInput.ModelName,
            createInput.DisplayName,
            createInput.Description,
            createInput.SystemPrompt,
            createInput.Instructions,
            createInput.Temperature,
            createInput.MaxOutputTokens,
            createInput.FrequencyPenalty,
            createInput.PresencePenalty,
            createInput.StateCheckers);

        if (!createInput.ApiKey.IsNullOrWhiteSpace())
        {
            var encryptApiKey = StringEncryptionService.Encrypt(createInput.ApiKey);
            record.SetApiKey(encryptApiKey, createInput.ApiBaseUrl);
        }

        return record;
    }

    protected override void MapToEntity(WorkspaceDefinitionRecordUpdateDto updateInput, WorkspaceDefinitionRecord entity)
    {
        if (entity.DisplayName != updateInput.DisplayName)
        {
            entity.SetDisplayName(updateInput.DisplayName);
        }

        if (entity.Description != updateInput.Description)
        {
            entity.Description = updateInput.Description;
        }

        if (entity.Provider != updateInput.Provider || entity.ModelName != updateInput.ModelName)
        {
            entity.SetModel(updateInput.Provider, updateInput.ModelName);
        }

        if (entity.SystemPrompt != updateInput.SystemPrompt)
        {
            entity.SystemPrompt = updateInput.SystemPrompt;
        }

        if (entity.Instructions != updateInput.Instructions)
        {
            entity.Instructions = updateInput.Instructions;
        }

        if (entity.IsEnabled != updateInput.IsEnabled)
        {
            entity.IsEnabled = updateInput.IsEnabled;
        }

        if (entity.Temperature != updateInput.Temperature)
        {
            entity.Temperature = updateInput.Temperature;
        }

        if (entity.MaxOutputTokens != updateInput.MaxOutputTokens)
        {
            entity.MaxOutputTokens = updateInput.MaxOutputTokens;
        }

        if (entity.FrequencyPenalty != updateInput.FrequencyPenalty)
        {
            entity.FrequencyPenalty = updateInput.FrequencyPenalty;
        }

        if (entity.PresencePenalty != updateInput.PresencePenalty)
        {
            entity.PresencePenalty = updateInput.PresencePenalty;
        }

        if (entity.StateCheckers != updateInput.StateCheckers)
        {
            entity.StateCheckers = updateInput.StateCheckers;
        }

        if (!updateInput.ApiKey.IsNullOrWhiteSpace())
        {
            var encryptApiKey = StringEncryptionService.Encrypt(updateInput.ApiKey);
            entity.SetApiKey(encryptApiKey, updateInput.ApiBaseUrl);
        }

        if (!entity.HasSameExtraProperties(updateInput))
        {
            entity.ExtraProperties.Clear();

            foreach (var property in updateInput.ExtraProperties)
            {
                entity.ExtraProperties.Add(property.Key, property.Value);
            }
        }
    }
}
