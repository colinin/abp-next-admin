using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Cli.UI.Flutter.GetX;
public class FlutterViewGenerator : ViewGeneratorBase<FlutterViewGenerator>, ISingletonDependency
{
    public const string Name = "Flutter-View";

    protected IFlutterGetXViewScriptGenerator ViewScriptGenerator { get; }

    public FlutterViewGenerator(
        CliHttpClientFactory cliHttpClientFactory, 
        IJsonSerializer jsonSerializer,
        IFlutterGetXViewScriptGenerator viewScriptGenerator) 
        : base(cliHttpClientFactory, jsonSerializer)
    {
        ViewScriptGenerator = viewScriptGenerator;
    }

    public override Task GenerateAsync(GenerateViewArgs args)
    {
        throw new System.NotImplementedException();
    }

    protected override ServiceType? GetDefaultServiceType(GenerateViewArgs args)
    {
        return ServiceType.Application;
    }

    private async Task CreateAndSaveSciptToDisk(string scriptFile, string script)
    {
        Logger.LogInformation($"The script output file: {scriptFile}.");
        Logger.LogInformation($"Saving script: {scriptFile}.");

        FileHelper.DeleteIfExists(scriptFile);

        await File.AppendAllTextAsync(scriptFile, script);

        Logger.LogInformation($"Saved script: {scriptFile} has successful.");
    }
}
