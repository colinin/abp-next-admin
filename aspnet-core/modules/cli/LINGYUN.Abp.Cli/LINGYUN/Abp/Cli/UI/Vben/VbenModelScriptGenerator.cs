using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Cli.UI.Vben;

public class VbenModelScriptGenerator : IVbenModelScriptGenerator, ISingletonDependency
{
    private readonly ITemplateRenderer _templateRenderer;

    public VbenModelScriptGenerator(
        ITemplateRenderer templateRenderer)
    {
        _templateRenderer = templateRenderer;
    }

    public async virtual Task<string> CreateModel(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel)
    {
        var searchModels = new List<ComponentModel>();
        var inputModels = new List<ComponentModel>();

        var moduleDefinition = appModel.Modules
            .Where(module => module.Key.Equals(args.Module))
            .Select(module => module.Value)
            .FirstOrDefault();

        var listResultAction = controllerModel.Actions
            .Where(action => action.Value.ReturnValue.TypeSimple.Contains("ListResultDto"))
            .Select(action => action.Value)
            .FirstOrDefault();

        searchModels.Add(
            new ComponentModel
            {
                Name = "filter",
                Component = "Input",
                DisplayName = "Search",
                ColProps = "{" +
                                "span: 24" +
                            "}",
            });

        if (listResultAction != null)
        {
            foreach (var inputParamter in listResultAction.Parameters)
            {
                if (appModel.Types.TryGetValue(inputParamter.Type, out var inputModelType))
                {
                    var span = inputModelType.Properties.Length > 3 ? 8
                        : inputModelType.Properties.Length > 2 ? 12
                        : 24;
                    foreach (var inputModelProp in inputModelType.Properties)
                    {
                        var component = new ComponentModel
                        {
                            Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                            Component = "Input",
                            DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase(),
                            Required = inputModelProp.IsRequired,
                            ColProps = "" +
                            "{" +
                                $"span: {span}" +
                            "}",
                        };

                        if (appModel.Types.TryGetValue(inputModelProp.Type, out var inputModelPropType) &&
                            inputModelPropType.IsEnum)
                        {
                            component.Component = "Select";

                            var optionsStr = Environment.NewLine;

                            var options = new object[inputModelPropType.EnumNames.Length]; 
                            for (var index = 0; index < inputModelPropType.EnumNames.Length; index++)
                            {
                                optionsStr += "" +
                                "{" + 
                                    $"label: {inputModelPropType.EnumNames[index]}," +
                                    $"value: {inputModelPropType.EnumValues[index]}," +
                                "}" +
                                "" + Environment.NewLine;
                            }
                            component.ComponentProps = "" +
                            "{" +
                                $"options: [{optionsStr}]" +
                            "}";
                        }

                        searchModels.Add(component);
                    }
                }
            }
        }

        var createOrUpdateAction = controllerModel.Actions
            .Where(action => action.Value.UniqueName.Contains("Create") || action.Value.UniqueName.Contains("Update"))
            .Select(action => action.Value)
            .FirstOrDefault();

        if (createOrUpdateAction != null)
        {
            foreach (var createOrUpdateParamter in createOrUpdateAction.Parameters)
            {
                if (appModel.Types.TryGetValue(createOrUpdateParamter.Type, out var inputModelType))
                {
                    foreach (var inputModelProp in inputModelType.Properties)
                    {
                        var component = new ComponentModel
                        {
                            Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                            Component = "Input",
                            DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase(),
                            Required = inputModelProp.IsRequired,
                            ColProps = "" +
                            "{" +
                                "span: 24" +
                            "}",
                            ComponentProps = "{}"
                        };

                        inputModels.Add(component);
                    }
                }
            }
        }

        var modelDataContent = await _templateRenderer.RenderAsync(
            "VbenModelData",
            new
            {
                RemoteService = moduleDefinition.RemoteServiceName,
                ExistsSearchModels = searchModels.Any(),
                SearchModels = searchModels,
                InputModels = inputModels,
            });

        return modelDataContent;
    }

    public async virtual Task<string> CreateTable(
        GenerateViewArgs args,
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel controllerModel)
    {
        var ouputModels = new List<ComponentModel>();

        var moduleDefinition = appModel.Modules
            .Where(module => module.Key.Equals(args.Module))
            .Select(module => module.Value)
            .FirstOrDefault();

        var listResultAction = controllerModel.Actions
            .Where(action => action.Value.ReturnValue.TypeSimple.Contains("ListResultDto")
                || action.Value.ReturnValue.TypeSimple.Contains("PagedResultDto"))
            .Select(action => action.Value)
            .FirstOrDefault();

        if (listResultAction != null)
        {
            foreach (var inputParamter in listResultAction.Parameters)
            {
                if (appModel.Types.TryGetValue(inputParamter.Type, out var inputModelType))
                {
                    var span = inputModelType.Properties.Length > 3 ? 8
                        : inputModelType.Properties.Length > 2 ? 12
                        : 24;
                    foreach (var inputModelProp in inputModelType.Properties)
                    {
                        var component = new ComponentModel
                        {
                            Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                            DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase()
                        };

                        ouputModels.Add(component);
                    }
                }
            }
        }

        var tableDataContent = await _templateRenderer.RenderAsync(
            "VbenTableData",
            new
            {
                RemoteService = moduleDefinition.RemoteServiceName,
                OuputModels = ouputModels,
            });

        return tableDataContent;
    }
}

public class ComponentModel
{
    public string Name { get; set; }
    public string Component { get; set; }
    public string DisplayName { get; set; }
    public object ColProps { get; set; }
    public object ComponentProps { get; set; }
    public string Align { get; set; } = "left";
    public int Width { get; set; } = 120;
    public bool Sorter { get; set; } = true;
    public bool Resizable { get; set; } = true;
    public bool Required { get; set; } = false;
}
