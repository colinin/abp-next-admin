using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

public interface ITypeScriptProxyGenerator
{
    string CreateModelScript(
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel actionModel);

    string CreateScript(
        ApplicationApiDescriptionModel appModel, 
        ModuleApiDescriptionModel apiModel,
        ControllerApiDescriptionModel actionModel);
}
