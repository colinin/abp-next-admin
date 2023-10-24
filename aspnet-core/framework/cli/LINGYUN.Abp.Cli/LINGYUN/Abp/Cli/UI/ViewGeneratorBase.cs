using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Cli;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace LINGYUN.Abp.Cli.UI;
public abstract class ViewGeneratorBase<T> : IViewGenerator where T : IViewGenerator
{
    public IJsonSerializer JsonSerializer { get; }

    public CliHttpClientFactory CliHttpClientFactory { get; }

    public ILogger<T> Logger { get; set; }

    protected ViewGeneratorBase(CliHttpClientFactory cliHttpClientFactory, IJsonSerializer jsonSerializer)
    {
        CliHttpClientFactory = cliHttpClientFactory;
        JsonSerializer = jsonSerializer;
        Logger = NullLogger<T>.Instance;
    }

    protected virtual async Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateViewArgs args, ApplicationApiDescriptionModelRequestDto requestDto = null)
    {
        Check.NotNull(args.Url, nameof(args.Url));

        var client = CliHttpClientFactory.CreateClient();

        var apiDefinitionResult = await client.GetStringAsync(CliUrls.GetApiDefinitionUrl(args.Url, requestDto));
        var apiDefinition = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(apiDefinitionResult);

        var moduleDefinition = apiDefinition.Modules.FirstOrDefault(x => string.Equals(x.Key, args.Module, StringComparison.CurrentCultureIgnoreCase)).Value;
        if (moduleDefinition == null)
        {
            throw new CliUsageException($"Module name: {args.Module} is invalid");
        }

        var serviceType = GetServiceType(args);
        switch (serviceType)
        {
            case ServiceType.Application:
                moduleDefinition.Controllers.RemoveAll(x => !x.Value.IsRemoteService);
                break;
            case ServiceType.Integration:
                moduleDefinition.Controllers.RemoveAll(x => !x.Value.IsIntegrationService);
                break;
        }

        var apiDescriptionModel = ApplicationApiDescriptionModel.Create();
        apiDescriptionModel.Types = apiDefinition.Types;
        apiDescriptionModel.AddModule(moduleDefinition);
        return apiDescriptionModel;
    }

    protected virtual ServiceType? GetServiceType(GenerateViewArgs args)
    {
        return args.ServiceType ?? GetDefaultServiceType(args);
    }

    protected abstract ServiceType? GetDefaultServiceType(GenerateViewArgs args);

    public abstract Task GenerateAsync(GenerateViewArgs args);
}
