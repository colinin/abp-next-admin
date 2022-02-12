using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AuditLogging.Elasticsearch
{
    public class IndexInitializerService : BackgroundService
    {
        private readonly IIndexInitializer _indexInitializer;

        public IndexInitializerService(IIndexInitializer indexInitializer)
        {
            _indexInitializer = indexInitializer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _indexInitializer.InitializeAsync();
        }
    }
}
