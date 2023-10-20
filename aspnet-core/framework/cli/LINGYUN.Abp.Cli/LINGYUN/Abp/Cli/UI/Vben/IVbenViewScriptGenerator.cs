using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.UI.Vben;
public interface IVbenViewScriptGenerator
{
    Task<string> CreateModal(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);

    Task<string> CreateTable(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);

    Task<string> CreateIndex(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);
}
