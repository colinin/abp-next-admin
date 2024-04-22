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

        var updateAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("UpdateAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();
        var createAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("CreateAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();
        var getAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("GetAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();

        var modalContent = await _templateRenderer.RenderAsync(
            "VbenModalView",
            new
            {
                Key = "id",
                HasCreate = createAction != null,
                GetAction = getAction?.UniqueName ?? "GetAsyncById",
                CreateAction = createAction?.UniqueName ?? "CreateAsyncByInput",
                HasUpdate = updateAction != null,
                UpdateAction = updateAction?.UniqueName ?? "UpdateAsyncByIdAndInput",
                HasSubmit = createAction != null || updateAction != null,
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
        var createAction = controllerModel.Actions
            .Where(action => action.Value.Name.Contains("CreateAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();

        // 高级查询
        var getAvailableFieldsAction = controllerModel.Actions
            .Where(action => action.Value.Name.Equals("GetAvailableFieldsAsync"))
            .Select(action => action.Value)
            .FirstOrDefault();
        var advancedSearchAction = controllerModel.Actions
            .Where(action => action.Value.Name.Equals("GetListAsyncByDynamicInput"))
            .Select(action => action.Value)
            .FirstOrDefault();

        var tableContent = await _templateRenderer.RenderAsync(
            "VbenTableView",
            new
            {
                Key = "id",
                PagedRequest = pagedResultAction != null,
                HasAdvancedSearch = getAvailableFieldsAction != null && advancedSearchAction != null,
                AvailableFieldsAction = getAvailableFieldsAction?.UniqueName ?? "GetAvailableFieldsAsync",
                AdvancedSearchAction = advancedSearchAction?.UniqueName ?? "GetListAsyncByDynamicInput",
                GetListAction = pagedResultAction?.UniqueName ?? "GetListAsyncByInput",
                HasCreate = createAction != null,
                CreatePermission = createAction != null && createAction.AllowAnonymous != true,
                CreatePermissionName = $"{moduleDefinition.RemoteServiceName.ToPascalCase()}.{controllerModel.ControllerName.ToPascalCase()}.Create",
                HasUpdate = updateAction != null,
                UpdatePermission = updateAction != null && updateAction.AllowAnonymous != true,
                UpdatePermissionName = $"{moduleDefinition.RemoteServiceName.ToPascalCase()}.{controllerModel.ControllerName.ToPascalCase()}.Update",
                HasDelete = deleteAction != null,
                DeleteAction = deleteAction?.UniqueName ?? "DeleteAsyncById",
                DeletePermission = deleteAction != null && deleteAction.AllowAnonymous != true,
                DeletePermissionName = $"{moduleDefinition.RemoteServiceName.ToPascalCase()}.{controllerModel.ControllerName.ToPascalCase()}.Delete",
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
