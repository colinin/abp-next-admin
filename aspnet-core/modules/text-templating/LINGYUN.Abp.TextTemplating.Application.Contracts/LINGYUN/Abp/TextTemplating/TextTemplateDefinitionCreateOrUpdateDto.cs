using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.TextTemplating;

public abstract class TextTemplateDefinitionCreateOrUpdateDto
{
    [Required]
    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxDisplayNameLength))]
    public string DisplayName { get; set; }

    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxDefaultCultureNameLength))]
    public string DefaultCultureName { get; set; }

    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxLocalizationResourceNameLength))]
    public string LocalizationResourceName { get; set; }

    public bool IsInlineLocalized { get; set; }

    public bool IsLayout { get; set; }

    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxLayoutLength))]
    public string Layout { get; set; }

    [DynamicStringLength(typeof(TextTemplateDefinitionConsts), nameof(TextTemplateDefinitionConsts.MaxRenderEngineLength))]
    public string RenderEngine { get; set; }
}
