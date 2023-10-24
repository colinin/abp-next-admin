using LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
using System;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace LINGYUN.Abp.Cli.ServiceProxying.Flutter;
public class RestServiceScriptGenerator : IFlutterHttpScriptGenerator, ITransientDependency
{
    public const string Name = "rest-service";

    public string CreateScript(
        ApplicationApiDescriptionModel appModel, 
        ModuleApiDescriptionModel apiModel, 
        ControllerApiDescriptionModel actionModel)
    {
        var apiScriptBuilder = new StringBuilder();

        apiScriptBuilder.AppendLine($"import 'models.dart';");
        apiScriptBuilder.AppendLine("import 'package:core/modles/abp.dto.dart';");
        apiScriptBuilder.AppendLine("import 'package:core/services/rest.service.dart';");
        apiScriptBuilder.AppendLine("import 'package:dio/dio.dart';");
        apiScriptBuilder.AppendLine("import 'package:get/get.dart';");
        apiScriptBuilder.AppendLine("");
        apiScriptBuilder.AppendLine($"class {actionModel.ControllerName.ToPascalCase()}Service {{");
        apiScriptBuilder.AppendLine("  RestService get _restService => Get.find();");
        apiScriptBuilder.AppendLine("");

        foreach (var action in actionModel.Actions)
        {
            var url = action.Value.Url.EnsureStartsWith('/');
            var isFormatUrl = false;
            var formatUrlIndex = 0;

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

            apiRetuanName = apiRetuanName.Replace("Void", "void");

            apiScriptBuilder.AppendFormat("  Future<{0}> {1}(", apiRetuanName, action.Value.UniqueName.ToCamelCase());

            var optionalParams = action.Value.ParametersOnMethod.Where(p => p.IsOptional).ToList();
            var notOptionalParams = action.Value.ParametersOnMethod.Where(p => !p.IsOptional).ToList();

            for (var index = 0; index < notOptionalParams.Count; index++)
            {
                var paramter = notOptionalParams[index];
                var apiParamName = paramter.Type.ReplaceFlutterType();
                apiParamName = apiParamName[(apiParamName.LastIndexOf('.') + 1)..];
                apiScriptBuilder.AppendFormat("{0} {1}", apiParamName, paramter.Name);

                if (index < notOptionalParams.Count - 1)
                {
                    apiScriptBuilder.Append(", ");
                }

                // 需要格式化url
                var formatUrlPath = paramter.Name;
                if (url.Contains(formatUrlPath))
                {
                    formatUrlIndex = url.IndexOf(formatUrlPath) + formatUrlPath.Length;
                    // 'api/platform/packages/{id}/blob/{Name}' => `api/platform/packages/$id/blob/{input.name}`
                    url = url.Replace(formatUrlPath, $"${formatUrlPath}").Replace($"{{${formatUrlPath}}}", $"${formatUrlPath}");
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

            if (optionalParams.Count > 0)
            {
                apiScriptBuilder.AppendLine(",{");
                for (var index = 0; index < optionalParams.Count; index++)
                {
                    var paramter = optionalParams[index];
                    var apiParamName = paramter.Type.ReplaceFlutterType();
                    apiParamName = apiParamName[(apiParamName.LastIndexOf('.') + 1)..];
                    apiScriptBuilder.AppendFormat("{0} {1}", apiParamName, paramter.Name);
                    apiScriptBuilder.AppendLine("");
                }
                apiScriptBuilder.AppendLine("}");
            }

            apiScriptBuilder.AppendLine(") {");

            apiScriptBuilder.AppendFormat("    return _restService.{0}('{1}',", action.Value.HttpMethod.ToLower(), url);
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
                apiScriptBuilder.AppendLine("      queryParameters: {");
                foreach (var paramter in inPathParams)
                {
                    apiScriptBuilder.AppendFormat("        {0}: {1}.{2},", paramter.Name, paramter.NameOnMethod, paramter.Name);
                    apiScriptBuilder.AppendLine("");
                }
                apiScriptBuilder.AppendLine("      },");
            }

            if (inBodyParams.Any())
            {
                apiScriptBuilder.AppendFormat("      data: {0},", inBodyParams.First().NameOnMethod);
                apiScriptBuilder.AppendLine("");
            }

            if (inFormParams.Any())
            {
                apiScriptBuilder.AppendFormat("      data: {0},", inFormParams.First().NameOnMethod);
                apiScriptBuilder.AppendLine("");
            }

            if (action.Value.AllowAnonymous == true || inFormParams.Any())
            {
                apiScriptBuilder.AppendLine("      options: Options(");
                if (action.Value.AllowAnonymous == true)
                {
                    apiScriptBuilder.AppendLine("        extra: {");
                    apiScriptBuilder.AppendLine("          'ignore_token': true");
                    apiScriptBuilder.AppendLine("        },");
                }
                apiScriptBuilder.AppendLine("        headers: {");
                if (inFormParams.Any())
                {
                    apiScriptBuilder.AppendLine("          'Content-type': 'multipart/form-data'");
                }
                apiScriptBuilder.AppendLine("        },");
                apiScriptBuilder.AppendLine($"      ),");
            }

            if (apiRetuanName.Equals("void"))
            {
                apiScriptBuilder.AppendLine($"    );");
            }
            else
            {
                apiScriptBuilder.AppendLine($"    ).then((json) => {apiRetuanName}.fromJson(json));");
            }
            apiScriptBuilder.AppendLine("  }");
            apiScriptBuilder.AppendLine();
        }

        apiScriptBuilder.AppendLine("}");
        apiScriptBuilder.AppendLine();

        return apiScriptBuilder.ToString();
    }
}
