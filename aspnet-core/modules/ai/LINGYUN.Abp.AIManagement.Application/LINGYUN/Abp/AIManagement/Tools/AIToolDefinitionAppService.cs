using LINGYUN.Abp.AI.Tools;
using LINGYUN.Abp.AIManagement.Localization;
using LINGYUN.Abp.AIManagement.Permissions;
using LINGYUN.Abp.AIManagement.Tools.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.AIManagement.Tools;
public class AIToolDefinitionAppService :
    CrudAppService<
        AIToolDefinitionRecord,
        AIToolDefinitionRecordDto,
        Guid,
        AIToolDefinitionRecordGetPagedListInput,
        AIToolDefinitionRecordCreateDto,
        AIToolDefinitionRecordUpdateDto>,
    IAIToolDefinitionAppService
{
    protected AbpAIToolsOptions AIToolsOptions { get; }
    protected IAIToolDefinitionRecordRepository AIToolDefinitionRecordRepository { get; }

    public AIToolDefinitionAppService(
        IOptions<AbpAIToolsOptions> aiToolsOptions,
        IAIToolDefinitionRecordRepository repository) 
        : base(repository)
    {
        AIToolsOptions = aiToolsOptions.Value;
        AIToolDefinitionRecordRepository = repository;

        LocalizationResource = typeof(AIManagementResource);
        ObjectMapperContext = typeof(AbpAIManagementApplicationModule);

        CreatePolicyName = AIManagementPermissionNames.AIToolDefinition.Create;
        UpdatePolicyName = AIManagementPermissionNames.AIToolDefinition.Update;
        DeletePolicyName = AIManagementPermissionNames.AIToolDefinition.Delete;
        GetListPolicyName = AIManagementPermissionNames.AIToolDefinition.Default;
        GetPolicyName = AIManagementPermissionNames.AIToolDefinition.Default;
    }

    public virtual Task<ListResultDto<AIToolProviderDto>> GetAvailableProvidersAsync()
    {
        var providers = AIToolsOptions.AIToolProviders
            .Select(LazyServiceProvider.GetRequiredService)
            .OfType<IAIToolProvider>()
            .Select(provider =>
            {
                var properties = provider.GetPropertites();

                return new AIToolProviderDto(
                    provider.Name,
                    properties.Select(prop =>
                    {
                        var property = new AIToolPropertyDescriptorDto
                        {
                            Name = prop.Name,
                            Options = prop.Options,
                            Required = prop.Required,
                            ValueType = prop.ValueType.ToString(),
                            DisplayName = prop.DisplayName.Localize(StringLocalizerFactory),
                            Dependencies = prop.Dependencies,
                        };
                        if (prop.Description != null)
                        {
                            property.Description = prop.Description.Localize(StringLocalizerFactory);
                        }

                        return property;
                    }).ToArray());
            });

        return Task.FromResult(new ListResultDto<AIToolProviderDto>(providers.ToImmutableArray()));
    }

    protected async override Task DeleteByIdAsync(Guid id)
    {
        var aiTool = await Repository.GetAsync(id);

        if (aiTool.IsSystem)
        {
            throw new BusinessException(
                AIManagementErrorCodes.AITool.SystemAIToolNotAllowedToBeDeleted,
                $"System AITool {aiTool.Name} is not allowed to be deleted!")
                .WithData("AITool", aiTool.Name);
        }

        await Repository.DeleteAsync(aiTool);
    }

    protected async override Task<IQueryable<AIToolDefinitionRecord>> CreateFilteredQueryAsync(AIToolDefinitionRecordGetPagedListInput input)
    {
        var queryable = await base.CreateFilteredQueryAsync(input);

        return queryable
            .WhereIf(input.IsEnabled.HasValue, x => x.IsEnabled == input.IsEnabled)
            .WhereIf(!input.Provider.IsNullOrWhiteSpace(), x => x.Provider == input.Provider)
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Provider.Contains(input.Filter!) ||
                x.Name.Contains(input.Filter!) || x.Description!.Contains(input.Filter!));
    }

    protected async override Task<AIToolDefinitionRecord> MapToEntityAsync(AIToolDefinitionRecordCreateDto createInput)
    {
        if (await AIToolDefinitionRecordRepository.FindByNameAsync(createInput.Name) != null)
        {
            throw new AIToolAlreadyExistsException(createInput.Name);
        }

        var entity = new AIToolDefinitionRecord(
            GuidGenerator.Create(),
            createInput.Name,
            createInput.Provider,
            createInput.Description,
            createInput.StateCheckers)
        {
            IsEnabled = createInput.IsEnabled,
            IsGlobal = createInput.IsGlobal,
        };

        if (!entity.HasSameExtraProperties(createInput))
        {
            entity.ExtraProperties.Clear();

            foreach (var property in createInput.ExtraProperties)
            {
                entity.ExtraProperties.Add(property.Key, property.Value);
            }
        }

        return entity;
    }

    protected override void MapToEntity(AIToolDefinitionRecordUpdateDto updateInput, AIToolDefinitionRecord entity)
    {
        if (entity.Description != updateInput.Description)
        {
            entity.Description = updateInput.Description;
        }

        if (entity.IsEnabled != updateInput.IsEnabled)
        {
            entity.IsEnabled = updateInput.IsEnabled;
        }

        if (entity.IsGlobal != updateInput.IsGlobal)
        {
            entity.IsGlobal = updateInput.IsGlobal;
        }

        if (!entity.HasSameExtraProperties(updateInput))
        {
            entity.ExtraProperties.Clear();

            foreach (var property in updateInput.ExtraProperties)
            {
                entity.ExtraProperties.Add(property.Key, property.Value);
            }
        }

        entity.SetConcurrencyStampIfNotNull(updateInput.ConcurrencyStamp);
    }
}
