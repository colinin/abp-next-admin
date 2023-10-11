using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.BlobStoring.Nexus;
public class DefaultBlobRawPathCalculator : IBlobRawPathCalculator, ITransientDependency
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

    public DefaultBlobRawPathCalculator(
        ICurrentTenant currentTenant,
        IBlobContainerConfigurationProvider configurationProvider)
    {
        CurrentTenant = currentTenant;
        ConfigurationProvider = configurationProvider;
    }

    public string CalculateGroup(string containerName, string blobName)
    {
        var blobPath = CalculateBasePath(containerName);

        var lastFolderIndex = blobName.LastIndexOf("/");
        if (lastFolderIndex > 0)
        {
            blobPath = blobPath.EnsureEndsWith('/');
            blobPath += blobName.Substring(0, lastFolderIndex);
        }

        return blobPath.EnsureStartsWith('/').RemovePostFix("/");
    }

    public string CalculateName(string containerName, string blobName, bool replacePath = false)
    {
        var blobPath = CalculateBasePath(containerName);
        blobPath = blobPath.EnsureEndsWith('/');
        blobPath += blobName;

        if (replacePath)
        {
            return blobName.Replace(blobPath.RemovePreFix("/"), "").RemovePreFix("/");
        }

        return blobPath.RemovePreFix("/");
    }

    protected virtual string CalculateBasePath(string containerName)
    {
        var configuration = ConfigurationProvider.Get<DefaultContainer>();
        var nexusConfiguration = configuration.GetNexusConfiguration();
        var blobPath = nexusConfiguration.BasePath;

        if (CurrentTenant.Id == null)
        {
            blobPath = $"{blobPath}/host";
        }
        else
        {
            blobPath = $"{blobPath}/tenants/{CurrentTenant.Id.Value.ToString("D")}";
        }

        if (nexusConfiguration.AppendContainerNameToBasePath)
        {
            blobPath = $"{blobPath}/{containerName.RemovePreFix("/")}";
        }

        return blobPath.EnsureStartsWith('/');
    }
}
