using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LINGYUN.Abp.Cli.ServiceProxying;

public class GenerateProxyArgs : Volo.Abp.Cli.ServiceProxying.GenerateProxyArgs
{
    public string Provider { get; }

    public string ApiScriptProxy { get; }

    public GenerateProxyArgs(
        [NotNull] string commandName,
        [NotNull] string workDirectory,
        string module,
        string url,
        string output,
        string target,
        string apiName,
        string source,
        string folder,
        string provider,
        string apiScriptProxy,
        Volo.Abp.Cli.ServiceProxying.ServiceType? serviceType,
        string entryPoint,
        bool withoutContracts,
        Dictionary<string, string> extraProperties = null)
        : base(commandName, workDirectory, module, url, output, target, apiName, source, folder, serviceType, entryPoint, withoutContracts, extraProperties)
    {
        ApiScriptProxy = apiScriptProxy;
        Provider = provider;
    }
}
