using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Default)]
public class TextTemplateContentAppService : AbpTextTemplatingAppServiceBase, ITextTemplateContentAppService
{
    protected ITextTemplateRepository TextTemplateRepository { get; }
    protected ITemplateContentProvider TemplateContentProvider { get; }
    protected ITemplateDefinitionManager TemplateDefinitionManager { get; }

    public TextTemplateContentAppService(
        ITextTemplateRepository textTemplateRepository,
        ITemplateContentProvider templateContentProvider,
        ITemplateDefinitionManager templateDefinitionManager)
    {
        TextTemplateRepository = textTemplateRepository;
        TemplateContentProvider = templateContentProvider;
        TemplateDefinitionManager = templateDefinitionManager;
    }

    public async virtual Task<TextTemplateContentDto> GetAsync(TextTemplateContentGetInput input)
    {
        var templateDefinition = await GetTemplateDefinition(input.Name);

        var content = await TemplateContentProvider.GetContentOrNullAsync(templateDefinition.Name, input.Culture);

        return new TextTemplateContentDto
        {
            Name = templateDefinition.Name,
            Culture = input.Culture,
            Content = content,
        };
    }

    //public virtual Task<PagedResultDto<TextTemplateDefinitionDto>> GetListAsync(TextTemplateDefinitionGetListInput input)
    //{
    //    var templates = new List<TextTemplateDefinitionDto>();
    //    var templateDefinitions = TemplateDefinitionManager.GetAll();
    //    var filterTemplates = templateDefinitions
    //        .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>
    //            x.Name.Contains(input.Filter) || x.Layout.Contains(input.Filter))
    //        .Skip(input.SkipCount)
    //        .Take(input.MaxResultCount);

    //    foreach (var templateDefinition in filterTemplates)
    //    {
    //        var layout = templateDefinition.Layout;
    //        if (!layout.IsNullOrWhiteSpace())
    //        {
    //            var layoutDefinition = GetTemplateDefinition(templateDefinition.Layout);
    //            layout = layoutDefinition.DisplayName.Localize(StringLocalizerFactory);
    //        }

    //        var result = new TextTemplateDefinitionDto
    //        {
    //            DefaultCultureName = templateDefinition.DefaultCultureName,
    //            IsInlineLocalized = templateDefinition.IsInlineLocalized,
    //            IsLayout = templateDefinition.IsLayout,
    //            Layout = layout,
    //            Name = templateDefinition.Name,
    //            DisplayName = templateDefinition.DisplayName.Localize(StringLocalizerFactory),
    //        };

    //        templates.Add(result);
    //    }

    //    return Task.FromResult(new PagedResultDto<TextTemplateDefinitionDto>(templateDefinitions.Count, templates));
    //}

    [Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Delete)]
    public async virtual Task RestoreToDefaultAsync(string name, TextTemplateRestoreInput input)
    {
        var templateDefinition = await GetTemplateDefinition(name);

        var templates = await TextTemplateRepository
            .GetListAsync(x => x.Name.Equals(templateDefinition.Name) && x.Culture.Equals(input.Culture));

        await TextTemplateRepository.DeleteManyAsync(templates);

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    [Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Update)]
    public async virtual Task<TextTemplateContentDto> UpdateAsync(string name, TextTemplateContentUpdateDto input)
    {
        var templateDefinition = await GetTemplateDefinition(name);

        var template = await TextTemplateRepository.FindByNameAsync(name, input.Culture);
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

        return new TextTemplateContentDto
        {
            Name = templateDefinition.Name,
            Culture = input.Culture,
            Content = template.Content,
        };
    }

    protected async virtual Task<TemplateDefinition> GetTemplateDefinition(string name)
    {
        return await TemplateDefinitionManager.GetOrNullAsync(name)
            ?? throw new BusinessException(AbpTextTemplatingErrorCodes.TextTemplateDefinition.TemplateNotFound)
                    .WithData(nameof(TextTemplateDefinition.Name), name);
    }
}
