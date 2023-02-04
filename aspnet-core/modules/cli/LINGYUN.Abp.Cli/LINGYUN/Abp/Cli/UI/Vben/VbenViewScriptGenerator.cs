using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Cli.UI.Vben;
public class VbenViewScriptGenerator : IVbenViewScriptGenerator, ISingletonDependency
{
    private readonly ITemplateRenderer _templateRenderer;

    public VbenViewScriptGenerator(
        ITemplateRenderer templateRenderer)
    {
        _templateRenderer = templateRenderer;
    }

    public async virtual Task<string> CreateModal(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel)
    {
        var moduleDefinition = appModel.Modules
            .Where(module => module.Key.Equals(args.Module))
            .Select(module => module.Value)
            .FirstOrDefault();

        var modalContent = await _templateRenderer.RenderAsync(
            "VbenModalView",
            new
            {
                Key = "id",
                Application = controllerModel.ControllerName,
                ApiPath = $"/@/api/{moduleDefinition.RemoteServiceName.ToKebabCase()}/{controllerModel.ControllerName.ToKebabCase()}",
                RemoteService = moduleDefinition.RemoteServiceName,
            });

        return modalContent;
    }

    public async virtual Task<string> CreateTable(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel)
    {
        var moduleDefinition = appModel.Modules
            .Where(module => module.Key.Equals(args.Module))
            .Select(module => module.Value)
            .FirstOrDefault();

        var pagedResultAction = controllerModel.Actions
            .Where(action => action.Value.ReturnValue.TypeSimple.Contains("PagedResultDto"))
            .Select(action => action.Value)
            .FirstOrDefault();

        var deleteAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("DeleteAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();
        var updateAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("UpdateAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();

        var tableContent = await _templateRenderer.RenderAsync(
            "VbenTableView",
            new
            {
                Key = "id",
                PagedRequest = pagedResultAction != null,
                UpdatePermission = updateAction != null && updateAction.AllowAnonymous != null && updateAction.AllowAnonymous != true,
                UpdatePermissionName = $"{moduleDefinition.RemoteServiceName.ToKebabCase()}.{controllerModel.ControllerName.ToKebabCase()}.Update",
                DeletePermission = deleteAction != null && deleteAction.AllowAnonymous != null && deleteAction.AllowAnonymous != true,
                DeletePermissionName = $"{moduleDefinition.RemoteServiceName.ToKebabCase()}.{controllerModel.ControllerName.ToKebabCase()}.Delete",
                Application = controllerModel.ControllerName,
                ModalName = $"{controllerModel.ControllerName.ToPascalCase()}Modal",
                ApiPath = $"/@/api/{moduleDefinition.RemoteServiceName.ToKebabCase()}/{controllerModel.ControllerName.ToKebabCase()}",
                RemoteService = moduleDefinition.RemoteServiceName,
            });

        return tableContent;
    }

    public async virtual Task<string> CreateIndex(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel)
    {
        var moduleDefinition = appModel.Modules
            .Where(module => module.Key.Equals(args.Module))
            .Select(module => module.Value)
            .FirstOrDefault();

        var modalContent = await _templateRenderer.RenderAsync(
            "VbenComponentIndex",
            new
            {
                TableName = $"{controllerModel.ControllerName}Table",
                Application = $"{moduleDefinition.RemoteServiceName.ToPascalCase()}{controllerModel.ControllerName.ToPascalCase()}".EnsureEndsWith('s')
            });

        return modalContent;
    }
}
