using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.FileManagement.FileSystem
{
    public class FileSystemOssContainerFactory : IOssContainerFactory
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IHostEnvironment Environment { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IBlobFilePathCalculator FilePathCalculator { get; }
        protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }
        protected IOptions<FileSystemOssOptions> Options { get; }

        public FileSystemOssContainerFactory(
            ICurrentTenant currentTenant,
            IHostEnvironment environment,
            IServiceProvider serviceProvider,
            IBlobFilePathCalculator blobFilePathCalculator,
            IBlobContainerConfigurationProvider configurationProvider,
            IOptions<FileSystemOssOptions> options)
        {
            Environment = environment;
            CurrentTenant = currentTenant;
            ServiceProvider = serviceProvider;
            FilePathCalculator = blobFilePathCalculator;
            ConfigurationProvider = configurationProvider;
            Options = options;
        }

        public IOssContainer Create()
        {
            return new FileSystemOssContainer(
                CurrentTenant,
                Environment,
                ServiceProvider,
                FilePathCalculator,
                ConfigurationProvider,
                Options);
        }
    }
}
