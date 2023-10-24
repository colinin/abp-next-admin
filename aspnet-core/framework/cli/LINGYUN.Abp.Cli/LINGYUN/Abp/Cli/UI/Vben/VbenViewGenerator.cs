using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Cli.UI.Vben;
public class VbenViewGenerator : ViewGeneratorBase<VbenViewGenerator>, ISingletonDependency
{
    public const string Name = "Vben-View";

    protected IVbenModelScriptGenerator ModelScriptGenerator { get; }
    protected IVbenViewScriptGenerator ViewScriptGenerator { get; }
    public VbenViewGenerator(
        CliHttpClientFactory cliHttpClientFactory, 
        IJsonSerializer jsonSerializer,
        IVbenModelScriptGenerator modelScriptGenerator,
        IVbenViewScriptGenerator viewScriptGenerator)
        : base(cliHttpClientFactory, jsonSerializer)
    {
        ModelScriptGenerator = modelScriptGenerator;
        ViewScriptGenerator = viewScriptGenerator;
    }

    private async Task CreateAndSaveSciptToDisk(string scriptFile, string script)
    {
        Logger.LogInformation($"The script output file: {scriptFile}.");
        Logger.LogInformation($"Saving script: {scriptFile}.");

        FileHelper.DeleteIfExists(scriptFile);

        await File.AppendAllTextAsync(scriptFile, script);

        Logger.LogInformation($"Saved script: {scriptFile} has successful.");
    }

    public async override Task GenerateAsync(GenerateViewArgs args)
    {
        var applicationApiDescriptionModel = await GetApplicationApiDescriptionModelAsync(
            args,
            new ApplicationApiDescriptionModelRequestDto
            {
                IncludeTypes = true
            });
        var outputFolderRoot = args.Output;

        foreach (var module in applicationApiDescriptionModel.Modules)
        {
            Logger.LogInformation($"Generating script with remote service: {module.Value.RemoteServiceName}.");

            foreach (var controller in module.Value.Controllers)
            {
                var componentScriptPath = Path.Combine(
                    outputFolderRoot,
                    module.Value.RemoteServiceName.ToKebabCase(),
                    controller.Value.ControllerName.ToKebabCase());

                var modelScriptPath = Path.Combine(componentScriptPath, "datas");
                DirectoryHelper.CreateIfNotExists(modelScriptPath);

                await CreateAndSaveSciptToDisk(
                    Path.Combine(modelScriptPath, "ModalData.ts"),
                    await ModelScriptGenerator.CreateModel(
                        args,
                        applicationApiDescriptionModel,
                        controller.Value));

                await CreateAndSaveSciptToDisk(
                    Path.Combine(modelScriptPath, "TableData.ts"),
                    await ModelScriptGenerator.CreateTable(
                        args,
                        applicationApiDescriptionModel,
                        controller.Value));

                var viewScriptPath = Path.Combine(componentScriptPath, "components");
                DirectoryHelper.CreateIfNotExists(viewScriptPath);

                await CreateAndSaveSciptToDisk(
                    Path.Combine(viewScriptPath, $"{controller.Value.ControllerName.ToPascalCase()}Modal.vue"),
                    await ViewScriptGenerator.CreateModal(
                        args,
                        applicationApiDescriptionModel,
                        controller.Value));

                await CreateAndSaveSciptToDisk(
                    Path.Combine(viewScriptPath, $"{controller.Value.ControllerName.ToPascalCase()}Table.vue"),
                    await ViewScriptGenerator.CreateTable(
                        args,
                        applicationApiDescriptionModel,
                        controller.Value));

                await CreateAndSaveSciptToDisk(
                    Path.Combine(componentScriptPath, "index.vue"),
                    await ViewScriptGenerator.CreateIndex(
                        args,
                        applicationApiDescriptionModel,
                        controller.Value));
            }
        }
    }

    protected override ServiceType? GetDefaultServiceType(GenerateViewArgs args)
    {
        return ServiceType.Application;
    }
}
