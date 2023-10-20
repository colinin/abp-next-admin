using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.Flutter;

public interface IFlutterModelScriptGenerator
{
    string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel actionModel);
}
