using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;

namespace LINGYUN.Platform.BlobStoring
{
    public class BlobStoringManager : DomainService
    {
        private IBlobRepository _blobRepository;
        protected IBlobRepository BlobRepository => LazyGetRequiredService(ref _blobRepository);

        private IBlobContainerRepository _containerRepository;
        protected IBlobContainerRepository ContainerRepository => LazyGetRequiredService(ref _containerRepository);

        protected IBlobContainerFactory BlobContainerFactory { get; }
        protected IBlobContainerConfigurationProvider BlobContainerConfigurationProvider { get; }

        public virtual async Task CreateContainerAsync(string name)
        {
            var containerConfiguration = BlobContainerConfigurationProvider.Get<OSSContainer>();
            var containerName = NormalizeContainerName(containerConfiguration, name);
            if (await ContainerRepository.ContainerExistsAsync(name))
            {

            }
            // 框架暂时未实现创建Container ，这里采用保存一个空文件，然后删除此文件的方法来创建Container
            var blobContainer = BlobContainerFactory.Create(containerName);
            try
            {
                var emptyBlobData = System.Text.Encoding.UTF8.GetBytes("");
                await blobContainer.SaveAsync("empty.txt", emptyBlobData, true);
                var container = new BlobContainer(GuidGenerator.Create(), containerName, CurrentTenant.Id)
                {
                    CreationTime = Clock.Now
                };
                await ContainerRepository.InsertAsync(container);
            }
            finally
            {
                await blobContainer.DeleteAsync("empty.txt");
            }
        }

        protected virtual string NormalizeContainerName(BlobContainerConfiguration configuration, string containerName)
        {
            if (!configuration.NamingNormalizers.Any())
            {
                return containerName;
            }

            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var normalizerType in configuration.NamingNormalizers)
                {
                    var normalizer = scope.ServiceProvider
                        .GetRequiredService(normalizerType)
                        .As<IBlobNamingNormalizer>();

                    containerName = normalizer.NormalizeContainerName(containerName);
                }

                return containerName;
            }
        }
    }
}
