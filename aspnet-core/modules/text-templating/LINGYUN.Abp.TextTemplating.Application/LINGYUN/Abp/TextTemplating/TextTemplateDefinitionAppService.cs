using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
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
        var template = await _store.GetOrNullAsync(input.Name);
        if (template != null)
        {
            throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.NameAlreadyExists)
                .WithData(nameof(TextTemplateDefinition.Name), input.Name);
        }

        var layout = input.Layout;
        if (!layout.IsNullOrWhiteSpace())
        {
            var layoutDefinition = await _store.GetAsync(layout);
            layout = await layoutDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
        }

        var formatDisplayName = input.DisplayName;
        if (!formatDisplayName.IsNullOrWhiteSpace())
        {
            var displayName = _localizableStringSerializer.Deserialize(formatDisplayName);
            formatDisplayName = await displayName.LocalizeAsync(StringLocalizerFactory);
        }

        var templateDefinition = new TextTemplateDefinition(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.IsLayout,
            input.Layout,
            input.IsInlineLocalized,
            input.DefaultCultureName,
            input.RenderEngine);

        await _store.CreateAsync(templateDefinition);

        await CurrentUnitOfWork.SaveChangesAsync();

        var result = new TextTemplateDefinitionDto
        {
            DefaultCultureName = templateDefinition.DefaultCultureName,
            IsInlineLocalized = templateDefinition.IsInlineLocalized,
            IsLayout = templateDefinition.IsLayout,
            Layout = layout,
            LayoutName = templateDefinition.Layout,
            Name = templateDefinition.Name,
            DisplayName = formatDisplayName,
            FormatedDisplayName = templateDefinition.DisplayName,
            IsStatic = templateDefinition.IsStatic,
            ConcurrencyStamp = templateDefinition.ConcurrencyStamp,
        };

        return result;
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Delete)]
    public async virtual Task DeleteAsync(string name)
    {
        await _store.DeleteAsync(name);
    }

    public async virtual Task<TextTemplateDefinitionDto> GetByNameAsync(string name)
    {
        var template = await _store.GetAsync(name);

        var layout = template.Layout;
        if (!layout.IsNullOrWhiteSpace())
        {
            var layoutDefinition = await _store.GetAsync(template.Layout);
            layout = await layoutDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
        }

        var result = new TextTemplateDefinitionDto
        {
            DefaultCultureName = template.DefaultCultureName,
            IsInlineLocalized = template.IsInlineLocalized,
            IsLayout = template.IsLayout,
            Layout = layout,
            LayoutName = template.Layout,
            Name = template.Name,
            DisplayName = await template.DisplayName.LocalizeAsync(StringLocalizerFactory),
            FormatedDisplayName = _localizableStringSerializer.Serialize(template.DisplayName),
        };

        var staticState = template.Properties.GetOrDefault(nameof(TextTemplateDefinition.IsStatic));
        if (staticState != null && staticState is bool isStatic)
        {
            result.IsStatic = isStatic;
        }

        return result;
    }

    public async virtual Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    {
        var templates = new List<TextTemplateDefinitionDto>();

        var templateDefinitions = await _store.GetAllAsync();

        var templateDefinitionFilter = templateDefinitions.AsQueryable()
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>
                x.Name.Contains(input.Filter) ||
                (!x.Layout.IsNullOrWhiteSpace() && x.Layout.Contains(input.Filter)))
            .WhereIf(input.IsStatic.HasValue, x => input.IsStatic == IsStatic(x))
            .WhereIf(input.IsLayout.HasValue, x => x.IsLayout == input.IsLayout);

        var sorting = input.Sorting;
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(TextTemplateDefinition.Name);
        }

        var filterTemplateCount = templateDefinitionFilter.Count();
        var filterTemplates = templateDefinitionFilter
            .OrderBy(sorting)
            .PageBy(input.SkipCount, input.MaxResultCount);

        foreach (var templateDefinition in filterTemplates)
        {
            var layout = templateDefinition.Layout;
            if (!layout.IsNullOrWhiteSpace())
            {
                var layoutDefinition = await _store.GetOrNullAsync(templateDefinition.Layout);
                if (layoutDefinition != null)
                {
                    layout = await layoutDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
                }
            }

            var result = new TextTemplateDefinitionDto
            {
                DefaultCultureName = templateDefinition.DefaultCultureName,
                IsInlineLocalized = templateDefinition.IsInlineLocalized,
                IsLayout = templateDefinition.IsLayout,
                Layout = layout,
                LayoutName = templateDefinition.Layout,
                Name = templateDefinition.Name,
                DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
                FormatedDisplayName = _localizableStringSerializer.Serialize(templateDefinition.DisplayName),
            };

            var staticState = templateDefinition.Properties.GetOrDefault(nameof(TextTemplateDefinition.IsStatic));
            if (staticState != null && staticState is bool isStatic)
            {
                result.IsStatic = isStatic;
            }

            templates.Add(result);
        }

        return new PagedResultDto<TextTemplateDefinitionDto>(filterTemplateCount, templates);
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateDefinition.Update)]
    public async virtual Task<TextTemplateDefinitionDto> UpdateAsync(string name, TextTemplateDefinitionUpdateDto input)
    {
        var templateDefinitionRecord = await _repository.FindByNameAsync(name);

        if (templateDefinitionRecord == null)
        {
            var templateDefinition = await _store.GetAsync(name);

            templateDefinitionRecord = new TextTemplateDefinition(
                GuidGenerator.Create(),
                templateDefinition.Name,
                _localizableStringSerializer.Serialize(templateDefinition.DisplayName),
                templateDefinition.IsLayout,
                templateDefinition.Layout,
                templateDefinition.IsInlineLocalized,
                templateDefinition.DefaultCultureName,
                templateDefinition.RenderEngine);

            UpdateByInput(templateDefinitionRecord, input);

            await _store.CreateAsync(templateDefinitionRecord);
        }
        else
        {
            UpdateByInput(templateDefinitionRecord, input);

            if (!string.Equals(templateDefinitionRecord.DisplayName, input.DisplayName, StringComparison.InvariantCultureIgnoreCase))
            {
                var displayNameD = _localizableStringSerializer.Deserialize(input.DisplayName);

                templateDefinitionRecord.DisplayName = await displayNameD.LocalizeAsync(StringLocalizerFactory);
            }

            await _store.UpdateAsync(templateDefinitionRecord);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var layout = templateDefinitionRecord.Layout;
        if (!layout.IsNullOrWhiteSpace())
        {
            var layoutDefinition = await _store.GetAsync(layout);
            layout = await layoutDefinition.DisplayName.LocalizeAsync(StringLocalizerFactory);
        }

        var displayName = templateDefinitionRecord.DisplayName;
        if (!displayName.IsNullOrWhiteSpace())
        {
            var displayNameD = _localizableStringSerializer.Deserialize(displayName);
            displayName = await displayNameD.LocalizeAsync(StringLocalizerFactory);
        }

        var result = new TextTemplateDefinitionDto
        {
            DefaultCultureName = templateDefinitionRecord.DefaultCultureName,
            IsInlineLocalized = templateDefinitionRecord.IsInlineLocalized,
            IsLayout = templateDefinitionRecord.IsLayout,
            Layout = layout,
            LayoutName = templateDefinitionRecord.Layout,
            Name = templateDefinitionRecord.Name,
            DisplayName = displayName,
            FormatedDisplayName = templateDefinitionRecord.DisplayName,
            IsStatic = templateDefinitionRecord.IsStatic,
            ConcurrencyStamp = templateDefinitionRecord.ConcurrencyStamp,
        };

        return result;
    }

    protected virtual void UpdateByInput(TextTemplateDefinition templateDefinition, TextTemplateDefinitionCreateOrUpdateDto input)
    {
        templateDefinition.IsInlineLocalized = input.IsInlineLocalized;
        templateDefinition.IsLayout = input.IsLayout;
        if (!string.Equals(templateDefinition.Layout, input.Layout, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.Layout = input.Layout;
        }
        if (!string.Equals(templateDefinition.DefaultCultureName, input.DefaultCultureName, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.DefaultCultureName = input.DefaultCultureName;
        }
        if (!string.Equals(templateDefinition.RenderEngine, input.RenderEngine, StringComparison.InvariantCultureIgnoreCase))
        {
            templateDefinition.RenderEngine = input.RenderEngine;
        }
    }

    private bool IsStatic(TemplateDefinition definition)
    {
        if (definition.Properties.TryGetValue(nameof(TextTemplateDefinition.IsStatic), out var isStaticObj) &&
            isStaticObj is bool isStatic)
        {
            return isStatic;
        }
        return false;
    }
}
