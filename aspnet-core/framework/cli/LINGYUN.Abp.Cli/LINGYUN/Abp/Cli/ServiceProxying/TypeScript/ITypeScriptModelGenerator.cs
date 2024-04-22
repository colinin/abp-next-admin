using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

public interface ITypeScriptModelGenerator
{
    string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel actionModel);
}
