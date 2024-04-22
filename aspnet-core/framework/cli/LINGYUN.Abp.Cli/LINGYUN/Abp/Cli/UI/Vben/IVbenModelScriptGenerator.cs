using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.UI.Vben;

public interface IVbenModelScriptGenerator
{
    Task<string> CreateModel(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);

    Task<string> CreateTable(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel);
}
