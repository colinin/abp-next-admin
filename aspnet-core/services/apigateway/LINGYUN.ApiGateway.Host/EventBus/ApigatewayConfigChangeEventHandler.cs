using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace LINGYUN.ApiGateway.EventBus
{
    public class ApigatewayConfigChangeEventHandler : IDistributedEventHandler<ApigatewayConfigChangeEventData>, ITransientDependency
    {
        protected ApiGatewayOptions Options { get; }
        private readonly ILogger<ApigatewayConfigChangeEventHandler> _logger;

        private readonly IFileConfigurationRepository _fileConfigRepo;
        private readonly IInternalConfigurationRepository _internalConfigRepo;
        private readonly IInternalConfigurationCreator _internalConfigCreator;
        public ApigatewayConfigChangeEventHandler(
            IOptions<ApiGatewayOptions> options,
            IFileConfigurationRepository fileConfigRepo,
            IInternalConfigurationRepository internalConfigRepo,
            IInternalConfigurationCreator internalConfigCreator,
            ILogger<ApigatewayConfigChangeEventHandler> logger)
        {
            _fileConfigRepo = fileConfigRepo;
            _internalConfigRepo = internalConfigRepo;
            _internalConfigCreator = internalConfigCreator;
            _logger = logger;

            Options = options.Value;
        }

        public async Task HandleEventAsync(ApigatewayConfigChangeEventData eventData)
        {
            if (Options.AppId.Equals(eventData.AppId))
            {
                var fileConfig = await _fileConfigRepo.Get();

                if (fileConfig.IsError)
                {
                    _logger.LogWarning($"error geting file config, errors are {string.Join(",", fileConfig.Errors.Select(x => x.Message))}");
                    return;
                }
                else
                {
                    var config = await _internalConfigCreator.Create(fileConfig.Data);
                    if (!config.IsError)
                    {
                        _internalConfigRepo.AddOrReplace(config.Data);
                    }
                }
                _logger.LogInformation("ocelot configuration changed on {0}", eventData.DateTime);
            }
        }
    }
}
