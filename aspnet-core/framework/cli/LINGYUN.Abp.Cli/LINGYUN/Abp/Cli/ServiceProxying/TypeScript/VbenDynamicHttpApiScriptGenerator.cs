using System;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

// 用于vben动态axios代理生成
public class VbenDynamicHttpApiScriptGenerator : IHttpApiScriptGenerator, ITransientDependency
{
    public const string Name = "vben-dynamic";

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
                        importModel += modelTypeName + ",";
                    }
                }
            }

            var returnType = action.Value.ReturnValue.TypeSimple;
            if (!returnType.StartsWith("System"))
            {
                var abpBaseType = TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(basType => returnType.StartsWith(basType));
                if (!abpBaseType.IsNullOrWhiteSpace())
                {
                    returnType = returnType
                        .Replace(abpBaseType, "")
                        .Replace("<", "")
                        .Replace(">", "");
                }

                returnType = returnType.ReplaceTypeSimple();
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

            if (TypeScriptModelGenerator.DataInParamMethods.Contains(action.Value.HttpMethod))
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

                if (inPathParams.Any() &&
                    inPathParams.Count() == 1)
                {
                    apiScriptBuilder.AppendFormat("    params: {0},", inPathParams.First().NameOnMethod);
                    apiScriptBuilder.AppendLine("");
                }
                else
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

            apiScriptBuilder.Append("  }");

            if (action.Value.AllowAnonymous == true)
            {
                apiScriptBuilder.AppendLine(",{");
                // 匿名方法无需token
                apiScriptBuilder.AppendLine("    withToken: false,");

                apiScriptBuilder.Append("  }");
            }

            apiScriptBuilder.AppendLine(");");
            apiScriptBuilder.AppendLine("};");
            apiScriptBuilder.AppendLine();
        }

        return apiScriptBuilder.ToString();
    }
}
