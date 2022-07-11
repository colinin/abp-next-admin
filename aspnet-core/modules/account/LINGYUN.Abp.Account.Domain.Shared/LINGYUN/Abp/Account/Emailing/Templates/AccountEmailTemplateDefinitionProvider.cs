using LINGYUN.Abp.Account.Localization;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace LY.MicroService.IdentityServer.Emailing.Templates;

public class AccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.MailSecurityVerifyLink,
                displayName: LocalizableString.Create<AbpAccountResource>(
                    $"TextTemplate:{AccountEmailTemplates.MailSecurityVerifyLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AbpAccountResource)
            ).WithVirtualFilePath("/LINGYUN/Abp/Account/Emailing/Templates/MailSecurityVerify.tpl", true)
        );
    }
}
