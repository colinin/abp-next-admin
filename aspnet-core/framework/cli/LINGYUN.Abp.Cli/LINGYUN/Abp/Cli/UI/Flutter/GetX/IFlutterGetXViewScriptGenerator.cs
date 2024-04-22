using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.UI.Flutter.GetX;
public interface IFlutterGetXViewScriptGenerator
{
    Task<string> CreateView(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);
}
