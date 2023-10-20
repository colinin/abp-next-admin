using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;
using static Volo.Abp.Cli.Commands.AddPackageCommand.Options;

namespace LINGYUN.Abp.Cli.UI.Vben;

public class VbenScriptTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(CreateCliTemplates());
    }

    private static TemplateDefinition[] CreateCliTemplates()
    {
        return new[]
        {
            new TemplateDefinition(
                "VbenModelData",
                typeof(DefaultResource),
                L("Templates:VbenModelData")
            ).WithVirtualFilePath("/LINGYUN/Abp/Cli/UI/Vben/Templates/VbenModelDataScript.tpl", true),
            new TemplateDefinition(
                "VbenModalView",
                typeof(DefaultResource),
                L("Templates:VbenModalView")
            ).WithVirtualFilePath("/LINGYUN/Abp/Cli/UI/Vben/Templates/VbenModalViewScript.tpl", true),
            new TemplateDefinition(
                "VbenTableData",
                typeof(DefaultResource),
                L("Templates:VbenTableData")
            ).WithVirtualFilePath("/LINGYUN/Abp/Cli/UI/Vben/Templates/VbenTableDataScript.tpl", true),
            new TemplateDefinition(
                "VbenTableView",
                typeof(DefaultResource),
                L("Templates:VbenTableView")
            ).WithVirtualFilePath("/LINGYUN/Abp/Cli/UI/Vben/Templates/VbenTableViewScript.tpl", true),
            new TemplateDefinition(
                "VbenComponentIndex",
                typeof(DefaultResource),
                L("Templates:VbenComponentIndex")
            ).WithVirtualFilePath("/LINGYUN/Abp/Cli/UI/Vben/Templates/VbenComponentIndexScript.tpl", true),
        };
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DefaultResource>(name);
    }
}
