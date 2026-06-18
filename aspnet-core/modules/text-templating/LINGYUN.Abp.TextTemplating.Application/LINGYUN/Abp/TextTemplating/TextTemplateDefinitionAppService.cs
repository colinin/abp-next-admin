using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Default)]
public class TextTemplateDefinitionAppService : AbpTextTemplatingAppServiceBase, ITextTemplateDefinitionAppService
{
    private readonly ITemplateDefinitionStore _store;
    private readonly ITextTemplateDefinitionRepository _repository;
    private readonly ILocalizableStringSerializer _localizableStringSerializer;

    public TextTemplateDefinitionAppService(
        ITemplateDefinitionStore store, 
        ITextTemplateDefinitionRepository repository,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        _store = store;
        _repository = repository;
        _localizableStringSerializer = localizableStringSerializer;
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Create)]
    public async virtual Task<TextTemplateDefinitionDto> CreateAsync(TextTemplateDefinitionCreateDto input)
    {
        var templateDefinitionRecord = await _repository.FindByNameAsync(input.Name);
        if (templateDefinitionRecord != null)
        {
            throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.NameAlreadyExists)
               .WithData("Name", input.Name);
        }

        templateDefinitionRecord = new TextTemplateDefinition(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.IsLayout,
            input.Layout,
            input.IsInlineLocalized,
            input.DefaultCultureName,
            input.LocalizationResourceName,
            input.RenderEngine);

        await _store.CreateAsync(templateDefinitionRecord);

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(templateDefinitionRecord);
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        await _store.DeleteAsync(name);
    }

    public async virtual Task<TextTemplateDefinitionDto> GetByNameAsync(string name)
    {
        var templateDefinitionRecord = await _repository.FindByNameAsync(name);
        if (templateDefinitionRecord == null)
        {
            return null;
        }
        return DefinitionRecordToDto(templateDefinitionRecord);
    }

    public async virtual Task<ListResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    {
        var templateDtoList = new List<TextTemplateDefinitionDto>();

        var dynamicTemplates = await _repository.GetListAsync(includeDetails: false);
        templateDtoList.AddRange(dynamicTemplates.Select(DefinitionRecordToDto));

        return new ListResultDto<TextTemplateDefinitionDto>(templateDtoList
            .WhereIf(input.IsStatic.HasValue, x => x.IsStatic == input.IsStatic)
            .WhereIf(input.IsLayout.HasValue, x => x.IsLayout == input.IsLayout)
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Filter) || x.DisplayName.Contains(input.Filter))
            .ToList());
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Update)]
    public async virtual Task<TextTemplateDefinitionDto> UpdateAsync(string name, TextTemplateDefinitionUpdateDto input)
    {
        var templateDefinitionRecord = await _repository.FindByNameAsync(name);

        if (templateDefinitionRecord == null)
        {
            templateDefinitionRecord = new TextTemplateDefinition(
                GuidGenerator.Create(),
                name,
                input.DisplayName,
                input.IsLayout,
                input.Layout,
                input.IsInlineLocalized,
                input.DefaultCultureName,
                input.LocalizationResourceName,
                input.RenderEngine);

            UpdateByInput(templateDefinitionRecord, input);

            await _store.CreateAsync(templateDefinitionRecord);
        }
        else
        {
            if (templateDefinitionRecord.IsStatic)
            {
                throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.StaticTemplateNotAllowedChanged)
                    .WithData("Name", name);
            }
            UpdateByInput(templateDefinitionRecord, input);

            await _store.UpdateAsync(templateDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return DefinitionRecordToDto(templateDefinitionRecord);
    }

    protected virtual void UpdateByInput(TextTemplateDefinition templateDefinition, TextTemplateDefinitionCreateOrUpdateDto input)
    {
        templateDefinition.IsInlineLocalized = input.IsInlineLocalized;
        templateDefinition.IsLayout = input.IsLayout;
        if (!string.Equals(templateDefinition.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.DisplayName = input.DisplayName;
        }
        if (!string.Equals(templateDefinition.Layout, input.Layout, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.Layout = input.Layout;
        }
        if (!string.Equals(templateDefinition.DefaultCultureName, input.DefaultCultureName, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.DefaultCultureName = input.DefaultCultureName;
        }
        if (!string.Equals(templateDefinition.LocalizationResourceName, input.LocalizationResourceName, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.LocalizationResourceName = input.LocalizationResourceName;
        }
        if (!string.Equals(templateDefinition.RenderEngine, input.RenderEngine, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.RenderEngine = input.RenderEngine;
        }
    }

    protected virtual TextTemplateDefinitionDto DefinitionRecordToDto(TextTemplateDefinition definitionRecord)
    {
        var dto = new TextTemplateDefinitionDto
        {
            RenderEngine = definitionRecord.RenderEngine,
            DefaultCultureName = definitionRecord.DefaultCultureName,
            IsInlineLocalized = definitionRecord.IsInlineLocalized,
            IsLayout = definitionRecord.IsLayout,
            Layout = definitionRecord.Layout,
            Name = definitionRecord.Name,
            LocalizationResourceName = definitionRecord.LocalizationResourceName,
            DisplayName = definitionRecord.DisplayName,
            ConcurrencyStamp = definitionRecord.ConcurrencyStamp,
            IsStatic = definitionRecord.IsStatic,
        };

        foreach (var property in definitionRecord.ExtraProperties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }

    protected virtual TextTemplateDefinitionDto DefinitionToDto(TemplateDefinition definition, bool isStatic = false)
    {
        var dto = new TextTemplateDefinitionDto
        {
            IsStatic = isStatic,
            RenderEngine = definition.RenderEngine,
            DefaultCultureName = definition.DefaultCultureName,
            IsInlineLocalized = definition.IsInlineLocalized,
            IsLayout = definition.IsLayout,
            Layout = definition.Layout,
            Name = definition.Name,
            LocalizationResourceName = definition.LocalizationResourceName,
            DisplayName = _localizableStringSerializer.Serialize(definition.DisplayName),
        };

        foreach (var property in definition.Properties)
        {
            dto.SetProperty(property.Key, property.Value);
        }

        return dto;
    }
}
