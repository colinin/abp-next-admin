using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

public interface IHttpApiScriptGenerator
{
    string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ModuleApiDescriptionModel apiModel,
        ControllerApiDescriptionModel actionModel);
}
