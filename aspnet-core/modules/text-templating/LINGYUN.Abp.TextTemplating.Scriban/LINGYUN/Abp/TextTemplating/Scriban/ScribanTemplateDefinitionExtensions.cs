using JetBrains.Annotations;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.TextTemplating.Scriban;

public static class ScribanTemplateDefinitionExtensions
{
    public static TemplateDefinition WithScribanEngine([NotNull] this TemplateDefinition templateDefinition)
    {
        return templateDefinition.WithRenderEngine(ScribanTemplateRenderingEngine.EngineName);
    }
}
