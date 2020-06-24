using LINGYUN.Abp.ExceptionHandling.Emailing.Localization;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.ExceptionHandling.Emailing.Templates
{
    public class ExceptionHandlingTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
               new TemplateDefinition(
                   ExceptionHandlingTemplates.SendEmail,
                   displayName: LocalizableString.Create<ExceptionHandlingResource>("TextTemplate:ExceptionHandlingTemplates.SendEmail"),
                   defaultCultureName: "en"
               ).WithVirtualFilePath("/LINGYUN/Abp/ExceptionHandling/Emailing/Templates/SendEmail", false)
           );
        }
    }
}
