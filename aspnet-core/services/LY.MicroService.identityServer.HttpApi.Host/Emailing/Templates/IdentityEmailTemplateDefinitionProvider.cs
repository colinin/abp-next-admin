using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;
using Volo.Abp.Identity.Localization;

namespace LY.MicroService.IdentityServer.Emailing.Templates
{
    public class IdentityEmailTemplateDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(
                 new TemplateDefinition(
                     IdentityEmailTemplates.EmailConfirmed,
                     displayName: LocalizableString.Create<IdentityResource>($"TextTemplate:{IdentityEmailTemplates.EmailConfirmed}"),
                     layout: StandardEmailTemplates.Layout,
                     localizationResource: typeof(IdentityResource)
                 ).WithVirtualFilePath("/LINGYUN/Abp/IdentityServer4/Emailing/Templates/EmailConfirmed.tpl", true)
             );
        }
    }
}
