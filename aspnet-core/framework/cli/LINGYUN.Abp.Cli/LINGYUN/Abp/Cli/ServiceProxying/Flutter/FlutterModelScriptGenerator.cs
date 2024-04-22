using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.Flutter;
public class FlutterModelScriptGenerator : IFlutterModelScriptGenerator, ITransientDependency
{
    public ILogger<FlutterModelScriptGenerator> Logger { protected get; set; }

    public FlutterModelScriptGenerator()
    {
        Logger = NullLogger<FlutterModelScriptGenerator>.Instance;
    }

    public string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ControllerApiDescriptionModel actionModel)
    {
        var modelScriptBuilder = new StringBuilder();
        // dto基类
        modelScriptBuilder.AppendLine("import 'package:core/models/abp.dto.dart';");
        // json库
        // flutter pub add json_annotation
        // flutter pub add build_runner --dev
        // flutter pub add json_serializable --dev
        // flutter pub run build_runner build --delete-conflicting-outputs
        modelScriptBuilder.AppendLine("import 'package:json_annotation/json_annotation.dart';");
        // json库实现, 
        modelScriptBuilder.AppendLine("part 'models.g.dart';");
        // modelScriptBuilder.AppendLine($"part '{actionModel.ControllerName.ToSnakeCase()}.g.dart';");
        modelScriptBuilder.AppendLine();

        var modelBaseTypes = new List<string>();
        var modelTypes = new List<string>();

        foreach (var action in actionModel.Actions)
        {
            foreach (var paramter in action.Value.Parameters)
            {
                if (!TypeScriptModelGenerator.AbpBaseTypes.Contains(paramter.TypeSimple) &&
                    appModel.Types.TryGetValue(paramter.Type, out var modelType))
                {
                    var modelTypeName = paramter.Type[(paramter.Type.LastIndexOf('.') + 1)..];

                    if (!modelTypes.Contains(modelTypeName))
                    {
                        Logger.LogInformation($"      Generating flutter model: {modelTypeName} script.");

                        modelScriptBuilder.AppendLine(CreateModel(modelTypeName, appModel, modelType));

                        Logger.LogInformation($"      Flutter model: {modelTypeName} generated successful.");

                        modelTypes.AddIfNotContains(modelTypeName);
                    }

                    // 字段类型
                    foreach (var propertity in modelType.Properties)
                    {
                        modelBaseTypes.AddIfNotContains(FindBaseTypes(appModel, propertity));
                    }

                    // 类型基类
                    modelBaseTypes.AddIfNotContains(FindBaseTypes(appModel, modelType));
                }
            }

            foreach (var paramter in action.Value.ParametersOnMethod)
            {
                if (appModel.Types.TryGetValue(paramter.Type, out var modelType))
                {
                    var modelTypeName = paramter.Type[(paramter.Type.LastIndexOf('.') + 1)..];

                    if (!modelTypes.Contains(modelTypeName))
                    {
                        Logger.LogInformation($"      Generating flutter model: {modelTypeName} script.");

                        modelScriptBuilder.AppendLine(CreateModel(modelTypeName, appModel, modelType));

                        Logger.LogInformation($"      Flutter model: {modelTypeName} generated successful.");

                        modelTypes.AddIfNotContains(modelTypeName);
                    }

                    // 字段类型
                    foreach (var propertity in modelType.Properties)
                    {
                        modelBaseTypes.AddIfNotContains(FindBaseTypes(appModel, propertity));
                    }

                    // 类型基类
                    modelBaseTypes.AddIfNotContains(FindBaseTypes(appModel, modelType));
                }
            }


            // 返回类型
            var returnType = action.Value.ReturnValue.TypeSimple;
            var abpBaseType = TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(basType => returnType.StartsWith(basType));
            if (!abpBaseType.IsNullOrWhiteSpace())
            {
                returnType = returnType
                    .Replace(abpBaseType, "")
                    .Replace("<", "")
                    .Replace(">", "");
            }

            returnType = returnType.ReplaceFlutterTypeSimple();

            if (appModel.Types.TryGetValue(returnType, out var returnBaseType))
            {
                foreach (var propertity in returnBaseType.Properties)
                {
                    var propType = propertity.TypeSimple;
                    if (propertity.TypeSimple.StartsWith("[") && propertity.TypeSimple.EndsWith("]"))
                    {
                        propType = propType.ReplaceFirst("[", "").RemovePostFix("]", "");
                    }

                    if (appModel.Types.TryGetValue(propType, out var propBaseType))
                    {
                        modelBaseTypes.AddIfNotContains(propType);
                        modelBaseTypes.AddIfNotContains(FindBaseTypes(appModel, propBaseType));
                    }
                }
            }

            modelBaseTypes.AddIfNotContains(returnType);
        }

        // 基类导出
        foreach (var baseType in modelBaseTypes)
        {
            if (appModel.Types.TryGetValue(baseType, out var modelType))
            {
                var modelTypeName = baseType[(baseType.LastIndexOf('.') + 1)..];

                Logger.LogInformation($"  Generating base flutter model: {modelTypeName} script.");

                modelScriptBuilder.AppendLine(CreateModel(modelTypeName, appModel, modelType));

                Logger.LogInformation($"    The base flutter model: {modelTypeName} generate successful.");
            }
        }

        return modelScriptBuilder.ToString();
    }

    protected virtual string CreateModel(
        string modelName,
        ApplicationApiDescriptionModel appModel,
        TypeApiDescriptionModel model)
    {
        var modelBuilder = new StringBuilder();

        if (model.IsEnum)
        {
            modelBuilder.AppendLine($"enum {modelName} {{");
            for (var index = 0; index < model.EnumNames.Length; index++)
            {
                var enumName = model.EnumNames[index].Replace("\"", "'");
                modelBuilder.AppendFormat("  {0}('{1}', {2})", enumName.ToCamelCase().Replace("'", ""), enumName, model.EnumValues[index]);
                if (index == model.EnumNames.Length - 1)
                {
                    modelBuilder.Append(';');
                    modelBuilder.AppendLine();
                }
                else
                {
                    modelBuilder.Append(',');
                    modelBuilder.AppendLine();
                }
            }

            modelBuilder.AppendLine("  final String name;");
            modelBuilder.AppendLine("  final int value;");
            modelBuilder.AppendLine($"  const {modelName}(this.name, this.value);");
            modelBuilder.AppendLine("}");
        }
        else
        {
            // example:
            /*
                @JsonSerializable()
                class RemoteServiceErrorInfo {
                  RemoteServiceErrorInfo({
                    required this.code,
                    required this.message,
                    this.details,
                    this.data,
                    this.validationErrors,
                  });
                  String code;
                  String message;
                  String? details;
                  Map<String, String>? data;
                  List<RemoteServiceValidationErrorInfo>? validationErrors;

                  factory RemoteServiceErrorInfo.fromJson(Map<String, dynamic> json) => _$RemoteServiceErrorInfoFromJson(json);
                  Map<String, dynamic> toJson() => _$RemoteServiceErrorInfoToJson(this);
                }
             */

            modelBuilder.AppendLine("@JsonSerializable()");
            modelBuilder.AppendFormat("class {0} ", modelName);

            if (!model.BaseType.IsNullOrWhiteSpace())
            {
                var baseType = ReplaceAbpBaseType(model.BaseType);
                baseType = baseType.ReplaceFlutterTypeSimple();

                modelBuilder.AppendFormat("extends {0} ", baseType[(baseType.LastIndexOf('.') + 1)..]);
            }
            modelBuilder.AppendLine("{");

            modelBuilder.AppendLine($"  {modelName}({{");

            CreateCtorProperties(modelBuilder, appModel, model);

            modelBuilder.AppendLine("  });");

            CreateProperties(modelBuilder, model.Properties);

            modelBuilder.AppendLine($"  factory {modelName}.fromJson(Map<String, dynamic> json) => _${modelName}FromJson(json);");
            modelBuilder.AppendLine($"  Map<String, dynamic> toJson() => _${modelName}ToJson(this);");
            modelBuilder.AppendLine("}");
            modelBuilder.AppendLine("");
        }

        return modelBuilder.ToString();
    }

    protected virtual void CreateCtorProperties(
        StringBuilder modelScript,
        ApplicationApiDescriptionModel appModel,
        TypeApiDescriptionModel model,
        bool abstractMember = false)
    {
        for (var index = 0; index < model.Properties.Length; index++)
        {
            var isRequired = model.Properties[index].IsRequired;
            if (!isRequired)
            {
                isRequired = !model.Properties[index].Type.Equals("System.String") && model.Properties[index].Type.EndsWith("?");
            }
            var propCharacter = isRequired ? "    required" : "   ";
            modelScript.AppendFormat("{0} {1}.{2},", propCharacter, abstractMember ? "super" : "this", model.Properties[index].Name.ToCamelCase());
            modelScript.AppendLine("");
        }

        if (!model.BaseType.IsNullOrWhiteSpace())
        {
            var replaceKey = model.BaseType.MiddleString("<", ">");
            if (replaceKey.IsNullOrWhiteSpace())
            {
                replaceKey = "<TPrimaryKey>";
            }

            if (appModel.Types.TryGetValue(model.BaseType.Replace(replaceKey, "<T0>"), out var abpBaseModel))
            {
                CreateCtorProperties(modelScript, appModel, abpBaseModel, true);
            }
            else
            {
                if (appModel.Types.TryGetValue(model.BaseType, out var baseModel))
                {
                    CreateCtorProperties(modelScript, appModel, baseModel, true);
                }
            }
        }
    }

    protected virtual void CreateProperties(StringBuilder modelScript, PropertyApiDescriptionModel[] properties)
    {
        for (var index = 0; index < properties.Length; index++)
        {
            var propTypeName = properties[index].Type.ReplaceFlutterType();
            if (!properties[index].IsRequired && properties[index].Type.Equals("System.String"))
            {
                propTypeName += "?";
            }
            modelScript.AppendFormat("  {0}", propTypeName);
            //var propCharacter = properties[index].IsRequired ? " " : "? ";
            modelScript.AppendFormat("{0}{1};", " ", properties[index].Name.ToCamelCase());
            modelScript.AppendLine("");
        }
    }

    protected virtual bool IsAbpBaseType(string typeSimple) => TypeScriptModelGenerator.AbpBaseTypes.Any(typeSimple.StartsWith);

    protected virtual List<string> FindBaseTypes(ApplicationApiDescriptionModel apiModel, TypeApiDescriptionModel model)
    {
        var types = new List<string>();

        if (!model.BaseType.IsNullOrWhiteSpace() &&
            !TypeScriptModelGenerator.AbpBaseTypes.Contains(model.BaseType) &&
            apiModel.Types.TryGetValue(model.BaseType, out var baseType))
        {
            types.Add(model.BaseType);

            types.AddRange(FindBaseTypes(apiModel, baseType));
        }

        return types;
    }

    protected virtual List<string> FindBaseTypes(ApplicationApiDescriptionModel apiModel, PropertyApiDescriptionModel model)
    {
        var types = new List<string>();

        var propertityType = model.Type.ReplaceFlutterType();

        if (!TypeScriptModelGenerator.AbpBaseTypes.Contains(propertityType) &&
            apiModel.Types.TryGetValue(propertityType, out var baseType))
        {
            types.Add(propertityType);

            types.AddRange(FindBaseTypes(apiModel, baseType));
        }

        return types;
    }

    protected virtual string ReplaceAbpBaseType(string typeSimple)
    {
        var abpBaseType = TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(t => t.StartsWith(typeSimple));
        if (abpBaseType.IsNullOrWhiteSpace())
        {
            return typeSimple;
        }

        return abpBaseType[(abpBaseType.LastIndexOf('.') + 1)..];
    }
}
