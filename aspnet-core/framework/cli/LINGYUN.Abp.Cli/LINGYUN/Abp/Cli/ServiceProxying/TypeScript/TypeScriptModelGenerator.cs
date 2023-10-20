using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
public class TypeScriptModelGenerator : ITypeScriptModelGenerator, ITransientDependency
{
    internal readonly static string[] AbpBaseTypes = new string[]
    {
        "Volo.Abp.Content.IRemoteStreamContent",
        "Volo.Abp.Content.RemoteStreamContent",
        "Volo.Abp.Application.Dtos.AuditedEntityDto",
        "Volo.Abp.Application.Dtos.AuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.CreationAuditedEntityDto",
        "Volo.Abp.Application.Dtos.CreationAuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.EntityDto",
        "Volo.Abp.Application.Dtos.ExtensibleAuditedEntityDto",
        "Volo.Abp.Application.Dtos.ExtensibleAuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.ExtensibleCreationAuditedEntityDto",
        "Volo.Abp.Application.Dtos.ExtensibleCreationAuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.ExtensibleEntityDto",
        "Volo.Abp.Application.Dtos.ExtensibleFullAuditedEntityDto",
        "Volo.Abp.Application.Dtos.ExtensibleFullAuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.FullAuditedEntityDto",
        "Volo.Abp.Application.Dtos.FullAuditedEntityWithUserDto",
        "Volo.Abp.Application.Dtos.LimitedResultRequestDto",
        "Volo.Abp.Application.Dtos.ListResultDto",
        "Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto",
        "Volo.Abp.Application.Dtos.PagedResultDto",
        "Volo.Abp.Application.Dtos.PagedResultRequestDto",
        "Volo.Abp.ObjectExtending.ExtensibleObject",
    };
    internal readonly static string[] DataInParamMethods = new string[]
    {
        "GET",
        "DELETE"
    };
    internal readonly static string[] DataInParamSources = new string[]
    {
        "Path",
        "ModelBinding"
    };
    internal readonly static string[] DataInFormSources = new string[]
    {
        "Form",
        "FormFile"
    };

    public ILogger<TypeScriptModelGenerator> Logger { protected get; set; }

    public TypeScriptModelGenerator()
    {
        Logger = NullLogger<TypeScriptModelGenerator>.Instance;
    }

    public string CreateScript(
        ApplicationApiDescriptionModel appModel, 
        ControllerApiDescriptionModel actionModel)
    {
        var modelScriptBuilder = new StringBuilder();
        var modelBaseTypes = new List<string>();
        var modelTypes = new List<string>();

        foreach (var action in actionModel.Actions)
        {
            foreach (var paramter in action.Value.Parameters)
            {
                if (!AbpBaseTypes.Contains(paramter.TypeSimple) &&
                    appModel.Types.TryGetValue(paramter.Type, out var modelType))
                {
                    var modelTypeName = paramter.Type[(paramter.Type.LastIndexOf('.') + 1)..];

                    if (!modelTypes.Contains(modelTypeName))
                    {
                        Logger.LogInformation($"      Generating model: {modelTypeName} script.");

                        modelScriptBuilder.AppendLine(CreateModel(modelTypeName, modelType));

                        Logger.LogInformation($"      Model: {modelTypeName} generate successful.");

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
                        Logger.LogInformation($"      Generating model: {modelTypeName} script.");

                        modelScriptBuilder.AppendLine(CreateModel(modelTypeName, modelType));

                        Logger.LogInformation($"      Model: {modelTypeName} generate successful.");

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
            var abpBaseType = AbpBaseTypes.FirstOrDefault(basType => returnType.StartsWith(basType));
            if (!abpBaseType.IsNullOrWhiteSpace())
            {
                returnType = returnType
                    .Replace(abpBaseType, "")
                    .Replace("<", "")
                    .Replace(">", "");
            }

            returnType = returnType.ReplaceTypeSimple();

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

                Logger.LogInformation($"  Generating base model: {modelTypeName} script.");

                modelScriptBuilder.AppendLine(CreateModel(modelTypeName, modelType));

                Logger.LogInformation($"    The base model: {modelTypeName} generate successful.");
            }
        }

        return modelScriptBuilder.ToString();
    }

    protected virtual string CreateModel(
        string modelName, 
        TypeApiDescriptionModel model)
    {
        var modelBuilder = new StringBuilder();

        if (model.IsEnum)
        {
            modelBuilder.AppendLine($"export enum {modelName} {{");
            for (var index = 0; index < model.EnumNames.Length; index++)
            {
                modelBuilder.AppendFormat("  {0} = {1},", model.EnumNames[index], model.EnumValues[index]);
                modelBuilder.AppendLine();
            }

            modelBuilder.AppendLine("}");
        }
        else
        {
            modelBuilder.AppendFormat("export interface {0} ", modelName);

            if (!model.BaseType.IsNullOrWhiteSpace())
            {
                var baseType = ReplaceAbpBaseType(model.BaseType);
                baseType = baseType.ReplaceTypeSimple();

                modelBuilder.AppendFormat("extends {0} ", baseType[(baseType.LastIndexOf('.') + 1)..]);
            }
            modelBuilder.AppendLine("{");

            for (var index = 0; index < model.Properties.Length; index++)
            {
                modelBuilder.AppendFormat("  {0}", model.Properties[index].Name.ToCamelCase());
                var propCharacter = model.Properties[index].IsRequired ? ": " : "?: ";
                var propTypeName = model.Properties[index].TypeSimple.ReplaceTypeSimple();
                if (propTypeName.LastIndexOf('.') >= 0)
                {
                    propTypeName = propTypeName[(propTypeName.LastIndexOf('.') + 1)..];
                }
                
                modelBuilder.AppendFormat("{0}{1};", propCharacter, propTypeName.ReplaceTypeSimple());
                modelBuilder.AppendLine("");
            }

            modelBuilder.AppendLine("}");
        }

        return modelBuilder.ToString();
    }

    protected virtual List<string> FindBaseTypes(ApplicationApiDescriptionModel apiModel, TypeApiDescriptionModel model)
    {
        var types = new List<string>();

        if (!model.BaseType.IsNullOrWhiteSpace() &&
            !AbpBaseTypes.Contains(model.BaseType) &&
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

        var propertityType = model.TypeSimple.ReplaceTypeSimple();

        if (!AbpBaseTypes.Contains(propertityType) &&
            apiModel.Types.TryGetValue(propertityType, out var baseType))
        {
            types.Add(propertityType);

            types.AddRange(FindBaseTypes(apiModel, baseType));
        }

        return types;
    }

    protected virtual string ReplaceAbpBaseType(string typeSimple)
    {
        var abpBaseType = AbpBaseTypes.FirstOrDefault(t => t.StartsWith(typeSimple));
        if (abpBaseType.IsNullOrWhiteSpace())
        {
            return typeSimple;
        }

        return abpBaseType[(abpBaseType.LastIndexOf('.') + 1)..];
    }
}
