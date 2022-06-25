using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Globalization;
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

    public async virtual Task<TextTemplateDto> GetAsync(TextTemplateGetInput input)
    {

        var templateDefinition = GetTemplateDefinition(input.Name);

        var culture = input.Culture ?? CultureInfo.CurrentCulture.Name ?? templateDefinition.DefaultCultureName;

        using (CultureHelper.Use(culture, culture))
        {
            var content = await TemplateContentProvider.GetContentOrNullAsync(templateDefinition.Name, culture);

            return new TextTemplateDto
            {
                Culture = culture,
                Content = content,
                Name = templateDefinition.Name,
                DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
            };
        }
    }

    public Task<ListResultDto<TextTemplateDto>> GetListAsync()
    {
        var templates = new List<TextTemplateDto>();
        var templateDefinitions = TemplateDefinitionManager.GetAll();

        foreach (var templateDefinition in templateDefinitions)
        {
            templates.Add(
                new TextTemplateDto
                {
                    Name = templateDefinition.Name,
                    Culture = CultureInfo.CurrentCulture.Name ?? templateDefinition.DefaultCultureName,
                    DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
                });
        }

        return Task.FromResult(new ListResultDto<TextTemplateDto>(templates));
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Delete)]
    public async virtual Task ResetDefaultAsync(TextTemplateGetInput input)
    {
        var templateDefinition = GetTemplateDefinition(input.Name);

        var culture = input.Culture ?? CultureInfo.CurrentCulture.Name ?? templateDefinition.DefaultCultureName;

        using (CultureHelper.Use(culture, culture))
        {
            var template = await TextTemplateRepository.FindByNameAsync(
                templateDefinition.Name,
                culture);
            if (template != null)
            {
                await TextTemplateRepository.DeleteAsync(template);
            }
        }
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplate.Update)]
    public async virtual Task<TextTemplateDto> UpdateAsync(TextTemplateUpdateInput input)
    {
        var templateDefinition = GetTemplateDefinition(input.Name);

        var culture = input.Culture ?? CultureInfo.CurrentCulture.Name ?? templateDefinition.DefaultCultureName;

        using (CultureHelper.Use(culture, culture))
        {
            var template = await TextTemplateRepository.FindByNameAsync(input.Name, culture);
            if (template == null)
            {
                template = new TextTemplate(
                    GuidGenerator.Create(),
                    templateDefinition.Name,
                    templateDefinition.DisplayName.Localize(StringLocalizerFactory),
                    input.Content,
                    culture);

                template = await TextTemplateRepository.InsertAsync(template);
            }
            else
            {
                template.SetContent(input.Content);

                await TextTemplateRepository.UpdateAsync(template);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<TextTemplate, TextTemplateDto>(template);
        }
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
