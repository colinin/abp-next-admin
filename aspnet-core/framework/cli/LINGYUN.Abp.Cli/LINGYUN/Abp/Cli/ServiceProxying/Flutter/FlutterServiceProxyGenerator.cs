using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Cli.ServiceProxying.Flutter;
public class FlutterServiceProxyGenerator : ServiceProxyGeneratorBase<FlutterServiceProxyGenerator>, ITransientDependency
{
    public const string Name = "FLUTTER";

    private readonly IFlutterModelScriptGenerator _flutterModelScriptGenerator;
    private readonly FlutterServiceProxyOptions _flutterServiceProxyOptions;

    public FlutterServiceProxyGenerator(
        CliHttpClientFactory cliHttpClientFactory,
        IJsonSerializer jsonSerializer,
        IFlutterModelScriptGenerator flutterModelScriptGenerator,
        IOptions<FlutterServiceProxyOptions> flutterServiceProxyOptions)
        : base(cliHttpClientFactory, jsonSerializer)
    {
        _flutterModelScriptGenerator = flutterModelScriptGenerator;
        _flutterServiceProxyOptions = flutterServiceProxyOptions.Value;
    }

    public async override Task GenerateProxyAsync(Volo.Abp.Cli.ServiceProxying.GenerateProxyArgs args)
    {
        var applicationApiDescriptionModel = await GetApplicationApiDescriptionModelAsync(
            args,
            new Volo.Abp.Http.Modeling.ApplicationApiDescriptionModelRequestDto
            {
                IncludeTypes = true
            });
        var outputFolderRoot = args.Output;

        foreach (var module in applicationApiDescriptionModel.Modules)
        {
            Logger.LogInformation($"Generating flutter model script with remote service: {module.Value.RemoteServiceName}.");

            foreach (var controller in module.Value.Controllers)
            {
                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating flutter model script with controller: {controller.Value.ControllerName}.");

                var modelScript = _flutterModelScriptGenerator
                    .CreateScript(applicationApiDescriptionModel, controller.Value);

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], {controller.Value.ControllerName} model script generated.");

                var scriptPath = Path.Combine(
                    outputFolderRoot,
                    module.Value.RemoteServiceName.ToKebabCase(),
                    controller.Value.ControllerGroupName.ToKebabCase());

                DirectoryHelper.CreateIfNotExists(scriptPath);

                var modelScriptFile = Path.Combine(scriptPath, "models.dart");

                Logger.LogInformation($"The flutter model script output file: {modelScriptFile}.");
                Logger.LogInformation($"Saving flutter model script: {modelScriptFile}.");

                FileHelper.DeleteIfExists(modelScriptFile);

                await File.AppendAllTextAsync(modelScriptFile, modelScript);

                Logger.LogInformation($"Saved flutter model script: {modelScriptFile} has successful.");

                // api script

                var apiScriptType = (args as GenerateProxyArgs).ApiScriptProxy;
                if (!_flutterServiceProxyOptions.ScriptGenerators.ContainsKey(apiScriptType))
                {
                    throw new CliUsageException($"Option Api Script Type {apiScriptType} value is invalid.");
                }
                var httpApiScriptProxy = _flutterServiceProxyOptions.ScriptGenerators[apiScriptType];

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating flutter api script with {apiScriptType}.");
                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating flutter api script with controller: {controller.Value.ControllerName}.");

                var apiScript = httpApiScriptProxy.CreateScript(
                    applicationApiDescriptionModel,
                    module.Value,
                    controller.Value);

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], {controller.Value.ControllerName} api script generated.");

                DirectoryHelper.CreateIfNotExists(scriptPath);

                var apiScriptFile = Path.Combine(scriptPath, "service.dart");

                Logger.LogInformation($"The flutter api script output file: {apiScriptFile}.");
                Logger.LogInformation($"Saving flutter api script: {apiScriptFile}.");

                FileHelper.DeleteIfExists(apiScriptFile);

                await File.AppendAllTextAsync(apiScriptFile, apiScript);

                var scriptExportFile = Path.Combine(scriptPath, "index.dart");

                Logger.LogInformation($"The flutter export script output file: {scriptExportFile}.");
                Logger.LogInformation($"Saving flutter export script: {scriptExportFile}.");

                FileHelper.DeleteIfExists(scriptExportFile);

                var scriptExportScript = new StringBuilder();
                scriptExportScript.AppendLine("export 'models.dart';");
                scriptExportScript.AppendLine("export 'service.dart';");

                await File.AppendAllTextAsync(scriptExportFile, scriptExportScript.ToString());

                Logger.LogInformation($"Saved api script: {apiScriptFile} has successful.");
            }
        }

        Logger.LogInformation($"Generate type script proxy has completed.");
    }

    protected override ServiceType? GetDefaultServiceType(Volo.Abp.Cli.ServiceProxying.GenerateProxyArgs args)
    {
        return ServiceType.Application;
    }
}
