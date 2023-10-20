using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Cli.ServiceProxying.TypeScript;
public class TypeScriptServiceProxyGenerator : ServiceProxyGeneratorBase<TypeScriptServiceProxyGenerator>, ITransientDependency
{
    public const string Name = "TS";

    private readonly ITypeScriptModelGenerator _typeScriptModelGenerator;
    private readonly TypeScriptServiceProxyOptions _typeScriptServiceProxyOptions;

    public TypeScriptServiceProxyGenerator(
        CliHttpClientFactory cliHttpClientFactory,
        IJsonSerializer jsonSerializer,
        ITypeScriptModelGenerator typeScriptModelGenerator,
        IOptions<TypeScriptServiceProxyOptions> typeScriptServiceProxyOptions)
        : base(cliHttpClientFactory, jsonSerializer)
    {
        _typeScriptModelGenerator = typeScriptModelGenerator;
        _typeScriptServiceProxyOptions = typeScriptServiceProxyOptions.Value;
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
            Logger.LogInformation($"Generating model script with remote service: {module.Value.RemoteServiceName}.");

            foreach (var controller in module.Value.Controllers)
            {
                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating model script with controller: {controller.Value.ControllerName}.");

                var modelScript = _typeScriptModelGenerator
                    .CreateScript(applicationApiDescriptionModel, controller.Value);

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], {controller.Value.ControllerName} model script generated.");

                var modelScriptPath = Path.Combine(
                    outputFolderRoot, 
                    module.Value.RemoteServiceName.ToKebabCase(), 
                    controller.Value.ControllerGroupName.ToKebabCase(),
                    "model");

                DirectoryHelper.CreateIfNotExists(modelScriptPath);

                var modelScriptFile = Path.Combine(modelScriptPath, "index.ts");

                Logger.LogInformation($"The model script output file: {modelScriptFile}.");
                Logger.LogInformation($"Saving model script: {modelScriptFile}.");

                FileHelper.DeleteIfExists(modelScriptFile);

                await File.AppendAllTextAsync(modelScriptFile, modelScript);

                Logger.LogInformation($"Saved model script: {modelScriptFile} has successful.");

                // api script

                var apiScriptType = (args as GenerateProxyArgs).ApiScriptProxy;
                if (!_typeScriptServiceProxyOptions.ScriptGenerators.ContainsKey(apiScriptType))
                {
                    throw new CliUsageException($"Option Api Script Type {apiScriptType} value is invalid.");
                }
                var httpApiScriptProxy = _typeScriptServiceProxyOptions.ScriptGenerators[apiScriptType];

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating api script with {apiScriptType}.");
                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], Generating api script with controller: {controller.Value.ControllerName}.");

                var apiScript = httpApiScriptProxy.CreateScript(
                    applicationApiDescriptionModel,
                    module.Value,
                    controller.Value);

                Logger.LogInformation($"  [{module.Value.RemoteServiceName}], {controller.Value.ControllerName} api script generated.");

                var apiScriptPath = Path.Combine(
                    outputFolderRoot,
                    module.Value.RemoteServiceName.ToKebabCase(),
                    controller.Value.ControllerGroupName.ToKebabCase());

                DirectoryHelper.CreateIfNotExists(apiScriptPath);

                var apiScriptFile = Path.Combine(apiScriptPath, "index.ts");

                Logger.LogInformation($"The api script output file: {apiScriptFile}.");
                Logger.LogInformation($"Saving api script: {apiScriptFile}.");

                FileHelper.DeleteIfExists(apiScriptFile);

                await File.AppendAllTextAsync(apiScriptFile, apiScript);

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
