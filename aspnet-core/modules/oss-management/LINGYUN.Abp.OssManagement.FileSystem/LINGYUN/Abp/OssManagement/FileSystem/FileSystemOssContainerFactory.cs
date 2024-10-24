using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement.FileSystem;

public class FileSystemOssContainerFactory : IOssContainerFactory
{
    protected ICurrentTenant CurrentTenant { get; }
    protected IHostEnvironment Environment { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IBlobFilePathCalculator FilePathCalculator { get; }
    protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }
    protected IOptions<AbpOssManagementOptions> Options { get; }

    public FileSystemOssContainerFactory(
        ICurrentTenant currentTenant,
        IHostEnvironment environment,
        IServiceScopeFactory serviceScopeFactory,
        IBlobFilePathCalculator blobFilePathCalculator,
        IBlobContainerConfigurationProvider configurationProvider,
        IOptions<AbpOssManagementOptions> options)
    {
        Options = options;
        Environment = environment;
        CurrentTenant = currentTenant;
        ServiceScopeFactory = serviceScopeFactory;
        FilePathCalculator = blobFilePathCalculator;
        ConfigurationProvider = configurationProvider;
    }

    public IOssContainer Create()
    {
        return new FileSystemOssContainer(
            CurrentTenant,
            Environment,
            ServiceScopeFactory,
            FilePathCalculator,
            ConfigurationProvider,
            Options);
    }
}
