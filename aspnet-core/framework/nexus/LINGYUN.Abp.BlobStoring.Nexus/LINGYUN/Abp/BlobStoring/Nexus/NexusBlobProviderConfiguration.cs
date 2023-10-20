using Volo.Abp;
using Volo.Abp.BlobStoring;

namespace LINGYUN.Abp.BlobStoring.Nexus;
public class NexusBlobProviderConfiguration
{
    public string BasePath {
        get => _containerConfiguration.GetConfiguration<string>(NexusBlobProviderConfigurationNames.BasePath);
        set => _containerConfiguration.SetConfiguration(NexusBlobProviderConfigurationNames.BasePath, value);
    }

    public bool AppendContainerNameToBasePath {
        get => _containerConfiguration.GetConfigurationOrDefault(NexusBlobProviderConfigurationNames.AppendContainerNameToBasePath, true);
        set => _containerConfiguration.SetConfiguration(NexusBlobProviderConfigurationNames.AppendContainerNameToBasePath, value);
    }

    public string Repository {
        get => _containerConfiguration.GetConfiguration<string>(NexusBlobProviderConfigurationNames.Repository);
        set => _containerConfiguration.SetConfiguration(NexusBlobProviderConfigurationNames.Repository, Check.NotNullOrWhiteSpace(value, nameof(value)));
    }

    private readonly BlobContainerConfiguration _containerConfiguration;

    public NexusBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
    {
        _containerConfiguration = containerConfiguration;
    }
}
