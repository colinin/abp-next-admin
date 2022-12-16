using System;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;

// 自用，请勿使用
// 适用于uni-app的axios
public class UniAppAxiosHttpApiScriptGenerator : IHttpApiScriptGenerator, ITransientDependency
{
    public const string Name = "uni-app-axios";

    public string CreateScript(
        ApplicationApiDescriptionModel appModel,
        ModuleApiDescriptionModel apiModel,
        ControllerApiDescriptionModel actionModel)
    {
        var apiScriptBuilder = new StringBuilder();

        apiScriptBuilder.AppendLine("import http from '../http'");

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
                var abpBaseType = TypeScriptModelGenerator.AbpBaseTypes.FirstOrDefault(basType => returnType.StartsWith(basType));
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

        apiScriptBuilder.AppendLine("import { " + importModel + " } from './model'");
        apiScriptBuilder.AppendLine("");

        foreach (var action in actionModel.Actions)
        {
            var url = action.Value.Url;
            var isFormatUrl = false;

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
                if (url.Contains('{') && url.Contains(paramter.Name))
                {
                    var formatUrl = MiddleString(url, "{", "}");
                    url = url.Replace(formatUrl, "");
                    url = "'" + url + "'" + " + " + paramter.Name;
                    isFormatUrl = true;
                }
            }

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

            apiScriptBuilder.AppendFormat("): Promise<{0}> ", apiRetuanName);

            apiScriptBuilder.AppendLine("=> {");

            if (!url.StartsWith("'") && !url.EndsWith("'"))
            {
                url = "'" + url + "'";
            }
            apiScriptBuilder.AppendLine("  return http.request({");
            apiScriptBuilder.AppendFormat("    method: '{0}',", action.Value.HttpMethod);
            apiScriptBuilder.AppendLine("");
            apiScriptBuilder.AppendFormat("    url: {0},", url);
            apiScriptBuilder.AppendLine("");

            if (TypeScriptModelGenerator.DataInParamMethods.Contains(action.Value.HttpMethod))
            {
                if (!isFormatUrl && action.Value.ParametersOnMethod.Any())
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

                if (!isFormatUrl && inPathParams.Any())
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

            apiScriptBuilder.AppendLine("  })");
            apiScriptBuilder.AppendLine("}");
        }

        return apiScriptBuilder.ToString();
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

    public static string MiddleString(string sourse, string startstr, string endstr)
    {
        var result = string.Empty;
        int startindex, endindex;
        startindex = sourse.IndexOf(startstr);
        if (startindex == -1)
        {
            return result;
        }
        var tmpstr = sourse.Substring(startindex + startstr.Length - 1);
        endindex = tmpstr.IndexOf(endstr);
        if (endindex == -1)
        {
            return result;
        }
        result = tmpstr.Remove(endindex + 1);

        return result;
    }
}
