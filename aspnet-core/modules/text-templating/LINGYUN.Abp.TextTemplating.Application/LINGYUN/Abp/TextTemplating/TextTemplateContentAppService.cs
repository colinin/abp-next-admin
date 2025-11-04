using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating;

[Authorize(AbpTextTemplatingPermissions.TextTemplateContent.Default)]
public class TextTemplateContentAppService : AbpTextTemplatingAppServiceBase, ITextTemplateContentAppService
{
    protected ITextTemplateRepository TextTemplateRepository { get; }
    protected ITemplateContentProvider TemplateContentProvider { get; }
    protected ITemplateDefinitionManager TemplateDefinitionManager { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public TextTemplateContentAppService(
        ITextTemplateRepository textTemplateRepository,
        ITemplateContentProvider templateContentProvider,
        ITemplateDefinitionManager templateDefinitionManager,
        ILocalizableStringSerializer localizableStringSerializer)
    {
        TextTemplateRepository = textTemplateRepository;
        TemplateContentProvider = templateContentProvider;
        TemplateDefinitionManager = templateDefinitionManager;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    public async virtual Task<TextTemplateContentDto> GetAsync(TextTemplateContentGetInput input)
    {
        var templateDefinition = await GetTemplateDefinition(input.Name);
        string content = null;

        try
        {
            content = await TemplateContentProvider.GetContentOrNullAsync(templateDefinition.Name, input.Culture);
        }
        catch
        {
            // Ignore 
            // 场景: 模板未在当前宿主服务中注册时, VirtualPath将抛出异常, 应忽略此异常
            // See: https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.TextTemplating.Core/Volo/Abp/TextTemplating/VirtualFiles/LocalizedTemplateContentReaderFactory.cs#L66
        }

        if (content.IsNullOrWhiteSpace())
        {
            var textTemplate = await TextTemplateRepository.FindByNameAsync(templateDefinition.Name, input.Culture);
            content = textTemplate?.Content;
        }

        return new TextTemplateContentDto
        {
            Name = templateDefinition.Name,
            Culture = input.Culture,
            Content = content,
        };
    }

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

        var template = await TextTemplateRepository.FindByNameAsync(templateDefinition.Name, input.Culture);
        if (template == null)
        {
            template = new TextTemplate(
                GuidGenerator.Create(),
                templateDefinition.Name,
                LocalizableStringSerializer.Serialize(templateDefinition.DisplayName),
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
