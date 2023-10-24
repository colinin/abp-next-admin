using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Http.Modeling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;

namespace LINGYUN.Abp.Cli.UI.Vben;

public class VbenModelScriptGenerator : IVbenModelScriptGenerator, ISingletonDependency
{
    internal readonly static string[] DisableInputModelFields = new []
    {
        nameof(IEntity<string>.Id),
        nameof(IMultiTenant.TenantId),
        nameof(IHasConcurrencyStamp.ConcurrencyStamp),
        nameof(IFullAuditedObject.CreationTime),
        nameof(IFullAuditedObject.CreatorId),
        nameof(IFullAuditedObject.LastModificationTime),
        nameof(IFullAuditedObject.LastModifierId),
    };
    internal readonly static string[] RemoveInputModelFields = new[]
    {
        nameof(IFullAuditedObject.DeletionTime),
        nameof(IFullAuditedObject.IsDeleted),
        nameof(IFullAuditedObject.DeleterId),
        nameof(IHasExtraProperties.ExtraProperties),
        "SkipCount",
        "MaxResultCount",
        "Sorting",
        "Items",
        "TotalCount",
    };

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
            .Where(action => action.Value.ReturnValue.TypeSimple.Contains("ListResultDto")
                || action.Value.ReturnValue.TypeSimple.Contains("PagedResultDto"))
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
            searchModels.AddRange(GetSearchComponents(appModel, listResultAction.Parameters));

            foreach (var searchParamter in listResultAction.Parameters)
            {
                var abpBaseType = GetAbpBaseType(appModel, searchParamter.TypeSimple);
                if (abpBaseType != null)
                {
                    searchModels.AddRange(GetSearchComponents(appModel, abpBaseType));
                }

                var inputParamterType = ReplaceAbpBaseType(searchParamter.TypeSimple);
                if (appModel.Types.TryGetValue(inputParamterType, out var inputModelType))
                {
                    searchModels.AddRange(GetSearchComponents(appModel, inputModelType));
                }

                searchModels = searchModels.DistinctBy(model => model.Name).ToList();
            }
        }

        var createAndUpdateActions = controllerModel.Actions
            .Where(action => action.Value.UniqueName.Contains("CreateAsync") || action.Value.UniqueName.Contains("UpdateAsync"))
            .Select(action => action.Value)
            .ToList();

        foreach (var createAndUpdateAction in createAndUpdateActions)
        {
            foreach (var createAndUpdateParamter in createAndUpdateAction.Parameters)
            {
                var abpBaseType = GetAbpBaseType(appModel, createAndUpdateParamter.TypeSimple);
                if (abpBaseType != null)
                {
                    inputModels.AddRange(GetComponents(appModel, abpBaseType));
                }

                var inputParamterType = ReplaceAbpBaseType(createAndUpdateParamter.TypeSimple);
                if (appModel.Types.TryGetValue(inputParamterType, out var inputModelType))
                {
                    inputModels.AddRange(GetComponents(appModel, inputModelType));
                }

                inputModels = inputModels.DistinctBy(model => model.Name).ToList();
            }
        }

        var modelDataContent = await _templateRenderer.RenderAsync(
            "VbenModelData",
            new
            {
                Key = "id",
                RemoteService = moduleDefinition.RemoteServiceName,
                ExistsSearchModels = searchModels.Any(),
                SearchModels = searchModels,
                InputModels = inputModels,
            });

        return modelDataContent;
    }

    protected virtual List<ComponentModel> GetSearchComponents(ApplicationApiDescriptionModel appModel, TypeApiDescriptionModel parameter)
    {
        var components = new List<ComponentModel>();

        if (!parameter.BaseType.IsNullOrWhiteSpace())
        {
            var abpBaseType = GetAbpBaseType(appModel, parameter.BaseType);
            if (abpBaseType != null)
            {
                components.AddRange(GetSearchComponents(appModel, abpBaseType));
            }

            var baseParamterType = ReplaceAbpBaseType(parameter.BaseType);
            if (appModel.Types.TryGetValue(baseParamterType, out var baseModelType))
            {
                components.AddRange(GetSearchComponents(appModel, baseModelType));
            }
        }

        components.AddRange(GetSearchComponents(appModel, parameter.Properties));

        return components;
    }

    protected virtual List<ComponentModel> GetSearchComponents(ApplicationApiDescriptionModel appModel, IEnumerable<PropertyApiDescriptionModel> parameters)
    {
        var components = new List<ComponentModel>();

        var span = parameters.Count() > 3 ? 8
            : parameters.Count() > 2 ? 12
            : 24;

        foreach (var inputModelProp in parameters)
        {
            if (RemoveInputModelFields.Contains(inputModelProp.Name, StringComparer.InvariantCultureIgnoreCase))
            {
                continue;
            }
            var component = new ComponentModel
            {
                Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                Component = GetComponentType(inputModelProp.Type),
                DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase(),
                Required = inputModelProp.IsRequired,
                ColProps = "" +
                "{" +
                    $"span: {span}" +
                "}",
                HasDate = inputModelProp.Type.Contains("DateTime")
            };

            if (appModel.Types.TryGetValue(inputModelProp.Type, out var inputModelPropType) &&
                inputModelPropType.IsEnum)
            {
                component.Component = "Select";

                var optionsStr = Environment.NewLine;

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

            components.Add(component);
        }

        return components;
    }

    protected virtual List<ComponentModel> GetSearchComponents(ApplicationApiDescriptionModel appModel, IEnumerable<ParameterApiDescriptionModel> parameters)
    {
        var components = new List<ComponentModel>();

        var span = parameters.Count() > 3 ? 8
            : parameters.Count() > 2 ? 12
            : 24;

        foreach (var inputModelProp in parameters)
        {
            if (RemoveInputModelFields.Contains(inputModelProp.Name, StringComparer.InvariantCultureIgnoreCase))
            {
                continue;
            }
            var component = new ComponentModel
            {
                Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                Component = GetComponentType(inputModelProp.Type),
                DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase(),
                Required = !inputModelProp.IsOptional,
                ColProps = "" +
                "{" +
                    $"span: {span}" +
                "}",
                HasDate = inputModelProp.Type.Contains("DateTime"),
            };

            if (appModel.Types.TryGetValue(inputModelProp.Type, out var inputModelPropType) &&
                inputModelPropType.IsEnum)
            {
                component.Component = "Select";

                var optionsStr = Environment.NewLine;

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

            components.Add(component);
        }

        return components;
    }

    protected virtual List<ComponentModel> GetComponents(ApplicationApiDescriptionModel appModel, TypeApiDescriptionModel parameter)
    {
        var components = new List<ComponentModel>();

        if (!parameter.BaseType.IsNullOrWhiteSpace())
        {
            var abpBaseType = GetAbpBaseType(appModel, parameter.BaseType);
            if (abpBaseType != null)
            {
                components.AddRange(GetComponents(appModel, abpBaseType));
            }

            var baseParamterType = ReplaceAbpBaseType(parameter.BaseType);
            if (appModel.Types.TryGetValue(baseParamterType, out var baseModelType))
            {
                components.AddRange(GetComponents(appModel, baseModelType));
            }
        }

        foreach (var modelProp in parameter.Properties)
        {
            var inputParamterType = ReplaceAbpBaseType(modelProp.TypeSimple);
            if (appModel.Types.TryGetValue(inputParamterType, out var inputModelType))
            {
                if (!inputModelType.BaseType.IsNullOrWhiteSpace())
                {
                    var abpBaseType = GetAbpBaseType(appModel, inputModelType.BaseType);
                    if (abpBaseType != null)
                    {
                        components.AddRange(GetComponents(appModel, abpBaseType));
                    }

                    var baseParamterType = ReplaceAbpBaseType(inputModelType.BaseType);
                    if (appModel.Types.TryGetValue(baseParamterType, out var baseModelType))
                    {
                        components.AddRange(GetComponents(appModel, baseModelType));
                    }
                }

                if (inputModelType.Properties != null)
                {
                    foreach (var inputModelProp in inputModelType.Properties)
                    {
                        if (RemoveInputModelFields.Contains(inputModelProp.Name, StringComparer.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        var component = new ComponentModel
                        {
                            Name = inputModelProp.JsonName ?? inputModelProp.Name.ToCamelCase(),
                            Component = GetComponentType(inputModelProp.Type),
                            DisplayName = "DisplayName:" + inputModelProp.Name.ToPascalCase(),
                            Required = inputModelProp.IsRequired,
                            ColProps = "" +
                            "{" +
                                "span: 24" +
                            "}",
                            ComponentProps = "{}",
                            HasDate = inputModelProp.Type.Contains("DateTime")
                        };

                        if (DisableInputModelFields.Contains(inputModelProp.Name, StringComparer.InvariantCultureIgnoreCase))
                        {
                            component.Show = false;
                            component.Disabled = true;
                        }

                        components.Add(component);
                    }
                }
            }
            else
            {
                if (RemoveInputModelFields.Contains(modelProp.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    return components;
                }

                var component = new ComponentModel
                {
                    Name = modelProp.JsonName ?? modelProp.Name.ToCamelCase(),
                    Component = GetComponentType(modelProp.Type),
                    DisplayName = "DisplayName:" + modelProp.Name.ToPascalCase(),
                    Required = modelProp.IsRequired,
                    ColProps = "" +
                        "{" +
                            "span: 24" +
                        "}",
                    ComponentProps = "{}",
                    HasDate = modelProp.Type.Contains("DateTime")
                };

                if (DisableInputModelFields.Contains(modelProp.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    component.Show = false;
                    component.Disabled = true;
                    component.Width = 1;
                }

                components.Add(component);
            }
        }

        return components;
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
            var abpBaseType = GetAbpBaseType(appModel, listResultAction.ReturnValue.TypeSimple);
            if (abpBaseType != null)
            {
                ouputModels.AddRange(GetComponents(appModel, abpBaseType));
            }

            var returnValueType = ReplaceAbpBaseType(listResultAction.ReturnValue.TypeSimple);
            if (appModel.Types.TryGetValue(returnValueType, out var inputModelType))
            {
                ouputModels.AddRange(GetComponents(appModel, inputModelType));
            }

            ouputModels = ouputModels.DistinctBy(model => model.Name).ToList();
        }

        var tableDataContent = await _templateRenderer.RenderAsync(
            "VbenTableData",
            new
            {
                Key = "id",
                RemoteService = moduleDefinition.RemoteServiceName,
                OuputModels = ouputModels,
            });

        return tableDataContent;
    }

    protected virtual bool IsAbpBaseType(string typeSimple) => TypeScriptModelGenerator.AbpBaseTypes.Any(typeSimple.StartsWith);

    protected virtual string GetComponentType(string typeSimple)
    {
        if (typeSimple.Contains("DateTime"))
        {
            return "DatePicker";
        }

        if (typeSimple.Contains("Boolean"))
        {
            return "Checkbox";
        }

        if (typeSimple.Contains("Enum"))
        {
            return "Select";
        }

        if (typeSimple.Contains("Int") ||
            typeSimple.Contains("Byte") ||
            typeSimple.Contains("Single") ||
            typeSimple.Contains("Double") ||
            typeSimple.Contains("Decimal"))
        {
            return "InputNumber";
        }

        if (typeSimple.Contains("Object"))
        {
            return "CodeEditorX";
        }

        return "Input";
    }

    protected virtual string GetAbpBaseType(string typeSimple) =>
        TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(typeSimple.StartsWith);

    protected virtual TypeApiDescriptionModel GetAbpBaseType(ApplicationApiDescriptionModel appModel, string typeSimple)
    {
        if (IsAbpBaseType(typeSimple))
        {
            var abpBaseType = GetAbpBaseType(typeSimple);

            if (abpBaseType == typeSimple)
            {
                return null;
            }

            return appModel.Types
                .Where(type => type.Key.StartsWith(abpBaseType))
                .Select(type => type.Value)
                .FirstOrDefault();
        }

        return null;
    }

    protected virtual string ReplaceAbpBaseType(string typeSimple)
    {
        var abpBaseType = TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(basType => typeSimple.StartsWith(basType));
        if (!abpBaseType.IsNullOrWhiteSpace())
        {
            typeSimple = typeSimple
                .Replace(abpBaseType, "")
                .Replace("<", "")
                .Replace(">", "");
        }

        return typeSimple;
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
    public bool Show { get; set; } = true;
    public bool Disabled { get; set; } = false;
    public bool Sorter { get; set; } = true;
    public bool Resizable { get; set; } = true;
    public bool Required { get; set; } = false;

    public bool HasDate { get; set; } = false;
}
