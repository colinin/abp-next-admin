using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.ObjectExtending;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
public class TypeScriptProxyGenerator : ITypeScriptProxyGenerator, ITransientDependency
{
    protected static string[] AbpBaseTypes = new string[]
    {
        typeof(ExtensibleObject).FullName,
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
    };
    protected static string[] DataInParamMethods = new string[]
    {
        "GET",
        "DELETE"
    };

    public ILogger<TypeScriptProxyGenerator> Logger { protected get; set; }
    public TypeScriptProxyGenerator()
    {
        Logger = NullLogger<TypeScriptProxyGenerator>.Instance;
    }

    public string CreateModelScript(ApplicationApiDescriptionModel appModel, ControllerApiDescriptionModel actionModel)
    {
        var modelScriptBuilder = new StringBuilder();
        var modelBaseTypes = new List<string>();
        var modelTypes = new List<string>();

        foreach (var action in actionModel.Actions)
        {
            foreach (var paramter in action.Value.Parameters)
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

            returnType = ReplaceTypeSimple(returnType);

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

    public string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ModuleApiDescriptionModel apiModel,
        ControllerApiDescriptionModel actionModel)
    {
        var apiScriptBuilder = new StringBuilder();

        apiScriptBuilder.AppendLine("import { defAbpHttp } from '/@/utils/http/abp';");

        var importModel = "";

        foreach (var action in actionModel.Actions)
        {
            foreach (var paramter in action.Value.ParametersOnMethod)
            {
                if (appModel.Types.TryGetValue(paramter.Type, out var _))
                {
                    var modelTypeName = paramter.Type[(paramter.Type.LastIndexOf('.') + 1)..];

                    if (!importModel.Contains(modelTypeName))
                    {
                        importModel += modelTypeName + ", ";
                    }
                }
            }

            var returnType = action.Value.ReturnValue.TypeSimple;
            if (!returnType.StartsWith("System"))
            {
                var abpBaseType = AbpBaseTypes.FirstOrDefault(basType => returnType.StartsWith(basType));
                if (!abpBaseType.IsNullOrWhiteSpace())
                {
                    returnType = returnType
                        .Replace(abpBaseType, "")
                        .Replace("<", "")
                        .Replace(">", "");
                }

                returnType = ReplaceTypeSimple(returnType);
                returnType = returnType[(returnType.LastIndexOf('.') + 1)..];
                if (!importModel.Contains(returnType))
                {
                    importModel += returnType + ",";
                }
            }
        }
        importModel = importModel.RemovePostFix(",");

        apiScriptBuilder.AppendLine("import { " + importModel + " } from './model';");
        apiScriptBuilder.AppendLine("");
        apiScriptBuilder.AppendFormat("const remoteServiceName = '{0}';", apiModel.RemoteServiceName);
        apiScriptBuilder.AppendLine("");
        apiScriptBuilder.AppendFormat("const controllerName = '{0}';", actionModel.ControllerName);
        apiScriptBuilder.AppendLine("");
        apiScriptBuilder.AppendLine("");

        foreach (var action in actionModel.Actions)
        {
            apiScriptBuilder.AppendFormat("export const {0} = (", action.Value.UniqueName);

            for (var index = 0; index < action.Value.ParametersOnMethod.Count; index++)
            {
                var paramter = action.Value.ParametersOnMethod[index];
                var apiParamCharacter = paramter.IsOptional ? "?:" : ":";
                var apiParamName = paramter.TypeSimple;
                apiParamName = apiParamName[(apiParamName.LastIndexOf('.') + 1)..];
                apiScriptBuilder.AppendFormat("{0}{1} {2}", paramter.Name, apiParamCharacter, apiParamName);

                if (index < action.Value.ParametersOnMethod.Count - 1)
                {
                    apiScriptBuilder.Append(", ");
                }
            }

            apiScriptBuilder.AppendLine(") => {");

            var apiRequestName = "request";
            var apiRetuanName = action.Value.ReturnValue.TypeSimple;

            if (apiRetuanName.Contains("ListResultDto"))
            {
                apiRequestName = "listRequest";
                apiRetuanName = apiRetuanName[(apiRetuanName.IndexOf("<") + 1)..];
                apiRetuanName = apiRetuanName[..^1];
            }
            else if (apiRetuanName.Contains("PagedResultDto"))
            {
                apiRequestName = "pagedRequest";
                apiRetuanName = apiRetuanName[(apiRetuanName.IndexOf("<") + 1)..];
                apiRetuanName = apiRetuanName[..^1];
            }

            apiRetuanName = apiRetuanName[(apiRetuanName.LastIndexOf('.') + 1)..];

            if (action.Value.ReturnValue.TypeSimple.Contains("System."))
            {
                apiRetuanName = apiRetuanName.ToLower();
            }

            apiScriptBuilder.AppendFormat("  return defAbpHttp.{0}<{1}>(", apiRequestName, apiRetuanName);
            apiScriptBuilder.AppendLine("{");
            apiScriptBuilder.AppendLine("    service: remoteServiceName,");
            apiScriptBuilder.AppendLine("    controller: controllerName,");
            apiScriptBuilder.AppendFormat("    action: '{0}',", action.Value.Name);
            apiScriptBuilder.AppendLine("");
            apiScriptBuilder.AppendFormat("    uniqueName: '{0}',", action.Value.UniqueName);
            apiScriptBuilder.AppendLine("");

            if (DataInParamMethods.Contains(action.Value.HttpMethod))
            {
                if (action.Value.ParametersOnMethod.Any())
                {
                    apiScriptBuilder.AppendLine("    params: {");

                    foreach (var paramter in action.Value.ParametersOnMethod)
                    {
                        apiScriptBuilder.AppendFormat("      {0}: {1},", paramter.Name, paramter.Name);
                        apiScriptBuilder.AppendLine("");
                    }

                    apiScriptBuilder.AppendLine("    },");
                }
            }
            else
            {
                var inPathParams = action.Value.Parameters.Where(p => p.BindingSourceId == "Path");
                var inBodyParams = action.Value.Parameters.Where(p => p.BindingSourceId == "Body");

                if (inPathParams.Any())
                {
                    apiScriptBuilder.AppendLine("    params: {");
                    foreach (var paramter in inPathParams)
                    {
                        apiScriptBuilder.AppendFormat("      {0}: {1},", paramter.Name, paramter.Name);
                        apiScriptBuilder.AppendLine("");
                    }
                    apiScriptBuilder.AppendLine("    },");
                }

                if (inBodyParams.Any() &&
                    inBodyParams.Count() == 1)
                {
                    apiScriptBuilder.AppendFormat("    data: {0},", inBodyParams.First().NameOnMethod);
                    apiScriptBuilder.AppendLine("");
                }
                else
                {
                    apiScriptBuilder.AppendLine("    data: {");
                    foreach (var paramter in inBodyParams)
                    {
                        apiScriptBuilder.AppendFormat("      {0}: {1},", paramter.Name, paramter.Name);
                        apiScriptBuilder.AppendLine("");
                    }
                    apiScriptBuilder.AppendLine("    },");
                }
            }

            apiScriptBuilder.AppendLine("  });");

            apiScriptBuilder.AppendLine("};");
        }

        return apiScriptBuilder.ToString();
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
                baseType = ReplaceTypeSimple(baseType);

                modelBuilder.AppendFormat("extends {0} ", baseType[(baseType.LastIndexOf('.') + 1)..]);
            }
            modelBuilder.AppendLine("{");

            for (var index = 0; index < model.Properties.Length; index++)
            {
                modelBuilder.AppendFormat("  {0}", model.Properties[index].Name.ToCamelCase());
                var propCharacter = model.Properties[index].IsRequired ? ": " : "?: ";
                var propTypeName = ReplaceTypeSimple(model.Properties[index].TypeSimple);
                if (propTypeName.LastIndexOf('.') >= 0)
                {
                    propTypeName = propTypeName[(propTypeName.LastIndexOf('.') + 1)..];
                }
                
                modelBuilder.AppendFormat("{0}{1};", propCharacter, ReplaceTypeSimple(propTypeName));
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

        var propertityType = ReplaceTypeSimple(model.TypeSimple);

        if (!AbpBaseTypes.Contains(propertityType) &&
            apiModel.Types.TryGetValue(propertityType, out var baseType))
        {
            types.Add(propertityType);

            types.AddRange(FindBaseTypes(apiModel, baseType));
        }

        return types;
    }

    protected virtual string ReplaceTypeSimple(string typeSimple)
    {
        typeSimple = typeSimple
            .Replace("?", "")
            .Replace("<System.String>", "<string>")
            .Replace("<System.Guid>", "<string>")
            .Replace("<System.Int32>", "<number>")
            .Replace("<System.Int64>", "<number>")
            .Replace("{string:string}", "Dictionary<string, string>")
            .Replace("{number:string}", "Dictionary<number, string>")
            .Replace("{string:number}", "Dictionary<string, number>")
            .Replace("{string:object}", "Dictionary<string, any>");

        if (typeSimple.StartsWith("[") && typeSimple.EndsWith("]"))
        {
            typeSimple = typeSimple.ReplaceFirst("[", "").RemovePostFix("]", "");
            typeSimple = typeSimple.Replace(typeSimple, $"{typeSimple}[]");
        }

        return typeSimple;
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
