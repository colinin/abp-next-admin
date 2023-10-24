using System;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

// axios原生代理生成
public class AxiosHttpApiScriptGenerator : IHttpApiScriptGenerator, ITransientDependency
{
    public const string Name = "axios";

    public string CreateScript(
        ApplicationApiDescriptionModel appModel, 
        ModuleApiDescriptionModel apiModel, 
        ControllerApiDescriptionModel actionModel)
    {
        var apiScriptBuilder = new StringBuilder();

        apiScriptBuilder.AppendLine("import axios from 'axios';");

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

        foreach (var action in actionModel.Actions)
        {
            var url = action.Value.Url.EnsureStartsWith('/');
            var isFormatUrl = false;
            var formatUrlIndex = 0;

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

                // 需要格式化url
                var formatUrlPath = string.Concat("{", paramter.Name, "}");
                if (url.Contains(formatUrlPath))
                {
                    formatUrlIndex = url.IndexOf(formatUrlPath) + formatUrlPath.Length;
                    // 'api/platform/packages/{id}/blob/{Name}' => `api/platform/packages/${id}/blob/{input.name}`
                    url = url.Replace(formatUrlPath, $"${formatUrlPath}");
                    isFormatUrl = true;
                }

                if (formatUrlIndex >= 0 && formatUrlIndex + formatUrlPath.Length <= url.Length)
                {
                    var formatUrl = url[(formatUrlIndex + formatUrlPath.Length)..].MiddleString("{", "}");
                    if (!formatUrl.IsNullOrWhiteSpace())
                    {
                        if (appModel.Types.TryGetValue(paramter.Type, out var paramType))
                        {
                            var formatParamInUrl = paramType.Properties
                                .FirstOrDefault(p => formatUrl.Contains(p.Name));

                            if (formatParamInUrl != null)
                            {
                                // 'api/platform/packages/xxx/blob/{Name}' => `api/platform/packages/xxx/blob/${input.name}`
                                url = url.Replace(
                                    formatUrl,
                                    string.Concat("${", paramter.Name, ".", formatParamInUrl.Name.ToCamelCase(), "}"));
                                isFormatUrl = true;
                            }
                        }
                    }
                }
            }

            apiScriptBuilder.AppendLine(") => {");

            var apiRetuanName = action.Value.ReturnValue.TypeSimple;

            if (apiRetuanName.Contains("ListResultDto"))
            {
                apiRetuanName = apiRetuanName[(apiRetuanName.IndexOf("<") + 1)..];
                apiRetuanName = apiRetuanName[..^1];
                apiRetuanName = apiRetuanName[(apiRetuanName.LastIndexOf('.') + 1)..];
                apiRetuanName = $"ListResultDto<{apiRetuanName}>";
            }
            else if (apiRetuanName.Contains("PagedResultDto"))
            {
                apiRetuanName = apiRetuanName[(apiRetuanName.IndexOf("<") + 1)..];
                apiRetuanName = apiRetuanName[..^1];
                apiRetuanName = apiRetuanName[(apiRetuanName.LastIndexOf('.') + 1)..];
                apiRetuanName = $"PagedResultDto<{apiRetuanName}>";
            }
            else
            {
                apiRetuanName = apiRetuanName[(apiRetuanName.LastIndexOf('.') + 1)..];
            }

            if (action.Value.ReturnValue.TypeSimple.Contains("System."))
            {
                apiRetuanName = apiRetuanName.ToLower();
            }

            if (isFormatUrl && !url.StartsWith("`") && !url.EndsWith("`"))
            {
                url = "`" + url + "`";
            }
            else
            {
                url = "'" + url + "'";
            }

            apiScriptBuilder.AppendFormat("  return axios.request<{0}>(", apiRetuanName);
            apiScriptBuilder.AppendLine("{");
            apiScriptBuilder.AppendFormat("    method: '{0}',", action.Value.HttpMethod);
            apiScriptBuilder.AppendLine("");
            apiScriptBuilder.AppendFormat("    url: {0},", url);
            apiScriptBuilder.AppendLine("");

            var inPathParams = action.Value.Parameters
                .Where(p => TypeScriptModelGenerator.DataInParamSources.Contains(p.BindingSourceId))
                .DistinctBy(p => p.NameOnMethod);
            var inBodyParams = action.Value.Parameters.Where(p => p.BindingSourceId == "Body");
            var inFormParams = action.Value.Parameters
                .Where(p => TypeScriptModelGenerator.DataInFormSources.Contains(p.BindingSourceId))
                .DistinctBy(p => p.NameOnMethod);

            if (!isFormatUrl && inPathParams.Any())
            {
                if (inPathParams.Count() == 1)
                {
                    apiScriptBuilder.AppendFormat("    params: {0},", inPathParams.First().NameOnMethod);
                    apiScriptBuilder.AppendLine("");
                }
                else
                {
                    apiScriptBuilder.AppendLine("    params: {");
                    foreach (var paramter in inPathParams)
                    {
                        apiScriptBuilder.AppendFormat("      {0}: {1}.{2},", paramter.Name, paramter.NameOnMethod, paramter.Name);
                        apiScriptBuilder.AppendLine("");
                    }
                    apiScriptBuilder.AppendLine("    },");
                }
            }

            if (inBodyParams.Any())
            {
                if (inBodyParams.Count() == 1)
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

            if (inFormParams.Any())
            {
                if (inFormParams.Count() == 1)
                {
                    apiScriptBuilder.AppendFormat("    data: {0},", inFormParams.First().NameOnMethod);
                    apiScriptBuilder.AppendLine("");
                }
                else
                {
                    apiScriptBuilder.AppendLine("    data: {");
                    foreach (var paramter in inFormParams)
                    {
                        apiScriptBuilder.AppendFormat("      {0}: {1}.{2},", paramter.Name, paramter.NameOnMethod, paramter.Name);
                        apiScriptBuilder.AppendLine("");
                    }
                }
                apiScriptBuilder.AppendLine("    headers: {");
                apiScriptBuilder.AppendLine("      'Content-type': 'multipart/form-data'");
                apiScriptBuilder.AppendLine("    },");
            }

            apiScriptBuilder.AppendLine("  });");
            apiScriptBuilder.AppendLine("};");
            apiScriptBuilder.AppendLine();
        }

        return apiScriptBuilder.ToString();
    }
}
