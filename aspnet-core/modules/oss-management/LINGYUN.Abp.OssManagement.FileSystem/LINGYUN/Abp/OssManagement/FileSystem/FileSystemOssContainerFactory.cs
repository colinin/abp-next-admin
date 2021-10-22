using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.OssManagement.FileSystem
{
    public class FileSystemOssContainerFactory : IOssContainerFactory
    {
        protected ICurrentTenant CurrentTenant { get; }
        protected IHostEnvironment Environment { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IBlobFilePathCalculator FilePathCalculator { get; }
        protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }
        protected IOptions<FileSystemOssOptions> Options { get; }
        protected IOptions<AbpOssManagementOptions> OssOptions { get; }

        public FileSystemOssContainerFactory(
            ICurrentTenant currentTenant,
            IHostEnvironment environment,
            IServiceProvider serviceProvider,
            IBlobFilePathCalculator blobFilePathCalculator,
            IBlobContainerConfigurationProvider configurationProvider,
            IOptions<FileSystemOssOptions> options,
            IOptions<AbpOssManagementOptions> ossOptions)
        {
            Environment = environment;
            CurrentTenant = currentTenant;
            ServiceProvider = serviceProvider;
            FilePathCalculator = blobFilePathCalculator;
            ConfigurationProvider = configurationProvider;
            Options = options;
            OssOptions = ossOptions;
        }

        public IOssContainer Create()
        {
            return new FileSystemOssContainer(
                CurrentTenant,
                Environment,
                ServiceProvider,
                FilePathCalculator,
                ConfigurationProvider,
                Options,
                OssOptions);
        }
    }
}
