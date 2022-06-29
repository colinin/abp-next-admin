using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Authorize(AbpTextTemplatingPermissions.TextTemplate.Default)]
public class TextTemplateAppService : AbpTextTemplatingAppServiceBase, ITextTemplateAppService
{
    protected ITextTemplateRepository TextTemplateRepository { get; }
    protected ITemplateContentProvider TemplateContentProvider { get; }
    protected ITemplateDefinitionManager TemplateDefinitionManager { get; }

    public TextTemplateAppService(
        ITextTemplateRepository textTemplateRepository,
        ITemplateContentProvider templateContentProvider,
        ITemplateDefinitionManager templateDefinitionManager)
    {
        TextTemplateRepository = textTemplateRepository;
        TemplateContentProvider = templateContentProvider;
        TemplateDefinitionManager = templateDefinitionManager;
    }

    public virtual Task<TextTemplateDefinitionDto> GetAsync(string name)
    {
        var templateDefinition = GetTemplateDefinition(name);

        var layout = templateDefinition.Layout;
        if (!layout.IsNullOrWhiteSpace())
        {
            var layoutDefinition = GetTemplateDefinition(templateDefinition.Layout);
            layout = layoutDefinition.DisplayName.Localize(StringLocalizerFactory);
        }

        var result = new TextTemplateDefinitionDto
        {
            DefaultCultureName = templateDefinition.DefaultCultureName,
            IsInlineLocalized = templateDefinition.IsInlineLocalized,
            IsLayout = templateDefinition.IsLayout,
            Layout = layout,
            Name = templateDefinition.Name,
            DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
        };

        return Task.FromResult(result);
    }

    public async virtual Task<TextTemplateContentDto> GetContentAsync(TextTemplateContentGetInput input)
    {
        var templateDefinition = GetTemplateDefinition(input.Name);

        var content = await TemplateContentProvider.GetContentOrNullAsync(templateDefinition.Name, input.Culture);

        return new TextTemplateContentDto
        {
            Name = templateDefinition.Name,
            Culture = input.Culture,
            Content = content,
        };
    }

    public virtual Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    {
        var templates = new List<TextTemplateDefinitionDto>();
        var templateDefinitions = TemplateDefinitionManager.GetAll();
        var filterTemplates = templateDefinitions
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>
                x.Name.Contains(input.Filter) || x.Layout.Contains(input.Filter))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        foreach (var templateDefinition in filterTemplates)
        {
            var layout = templateDefinition.Layout;
            if (!layout.IsNullOrWhiteSpace())
            {
                var layoutDefinition = GetTemplateDefinition(templateDefinition.Layout);
                layout = layoutDefinition.DisplayName.Localize(StringLocalizerFactory);
            }

            var result = new TextTemplateDefinitionDto
            {
                DefaultCultureName = templateDefinition.DefaultCultureName,
                IsInlineLocalized = templateDefinition.IsInlineLocalized,
                IsLayout = templateDefinition.IsLayout,
                Layout = layout,
                Name = templateDefinition.Name,
                DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
            };

            templates.Add(result);
        }

        return Task.FromResult(new PagedResultDto<TextTemplateDefinitionDto>(templateDefinitions.Count, templates));
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Delete)]
    public async virtual Task RestoreToDefaultAsync(TextTemplateRestoreInput input)
    {
        var templateDefinition = GetTemplateDefinition(input.Name);

        var templates = await TextTemplateRepository
            .GetListAsync(x => x.Name.Equals(templateDefinition.Name) && x.Culture.Equals(input.Culture));

        await TextTemplateRepository.DeleteManyAsync(templates);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Update)]
    public async virtual Task<TextTemplateDefinitionDto> UpdateAsync(TextTemplateUpdateInput input)
    {
        var templateDefinition = GetTemplateDefinition(input.Name);

        var template = await TextTemplateRepository.FindByNameAsync(input.Name, input.Culture);
        if (template == null)
        {
            template = new TextTemplate(
                GuidGenerator.Create(),
                templateDefinition.Name,
                templateDefinition.DisplayName.Localize(StringLocalizerFactory),
                input.Content,
                input.Culture);

            await TextTemplateRepository.InsertAsync(template);
        }
        else
        {
            template.SetContent(input.Content);

            await TextTemplateRepository.UpdateAsync(template);
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        var layout = templateDefinition.Layout;
        if (!layout.IsNullOrWhiteSpace())
        {
            var layoutDefinition = GetTemplateDefinition(templateDefinition.Layout);
            layout = layoutDefinition.DisplayName.Localize(StringLocalizerFactory);
        }

        return new TextTemplateDefinitionDto
        {
            DefaultCultureName = templateDefinition.DefaultCultureName,
            IsInlineLocalized = templateDefinition.IsInlineLocalized,
            IsLayout = templateDefinition.IsLayout,
            Layout = layout,
            Name = templateDefinition.Name,
            DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
        };
    }

    protected virtual TemplateDefinition GetTemplateDefinition(string name)
    {
        var template = TemplateDefinitionManager.GetOrNull(name);
        if (template == null)
        {
            throw new BusinessException(
                AbpTextTemplatingErrorCodes.TemplateNotFound,
                $"The text template {name} does not exist!")
                .WithData("Name", name);
        }

        return template;
    }
}
