using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using Ocelot.Configuration.ChangeTracking;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.IO;

namespace LINGYUN.MicroService.Internal.ApiGateway.Ocelot.Configuration.Repository
{
    public class DiskFileConfigurationAggragatorRepository : IFileConfigurationRepository
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IOcelotConfigurationChangeTokenSource _changeTokenSource;

        private readonly string[] _ocelotFiles;

        private static readonly AsyncLock _lock = new AsyncLock();

        private readonly string _ocelotFilePath;
        private readonly string _environmentFilePath;

        public DiskFileConfigurationAggragatorRepository(
            IWebHostEnvironment hostingEnvironment, 
            IOcelotConfigurationChangeTokenSource changeTokenSource)
        {
            _changeTokenSource = changeTokenSource;
            _hostEnvironment = hostingEnvironment;

            _ocelotFilePath = Path.Combine(_hostEnvironment.ContentRootPath, "ocelot.json");
            _environmentFilePath = Path.Combine(
                _hostEnvironment.ContentRootPath, 
                $"ocelot.{hostingEnvironment.EnvironmentName ?? "Development"}.json");
            _ocelotFiles = new string[7] 
            {
                "ocelot.global.json",
                "ocelot.backendadmin.json",
                "ocelot.idsadmin.json",
                "ocelot.localization.json",
                "ocelot.messages.json",
                "ocelot.platform.json",
                "ocelot.aggregate.json"
            };
        }

        public async Task<Response<FileConfiguration>> Get()
        {
            JObject configObject = null;
            JsonMergeSettings mergeSetting = new()
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                PropertyNameComparison = System.StringComparison.CurrentCultureIgnoreCase
            };
            using (await _lock.LockAsync())
            {
                if (File.Exists(_environmentFilePath))
                {
                    var configValue = await FileHelper.ReadAllTextAsync(_environmentFilePath);
                    configObject = JObject.Parse(configValue);
                }
                else
                {
                    foreach (var ocelotFile in _ocelotFiles)
                    {
                        var configValue = await FileHelper.ReadAllTextAsync(
                            Path.Combine(_hostEnvironment.ContentRootPath, ocelotFile));

                        if (configObject == null)
                        {
                            configObject = JObject.Parse(configValue);
                        }
                        else
                        {
                            configObject.Merge(JObject.Parse(configValue), mergeSetting);
                        }
                    }

                    await File.WriteAllTextAsync(_environmentFilePath, configObject.ToString(), encoding: System.Text.Encoding.UTF8);
                }

                var aggregatorConfig = configObject.ToObject<FileConfiguration>();

                return new OkResponse<FileConfiguration>(aggregatorConfig);
            }
        }

        public Task<Response> Set(FileConfiguration fileConfiguration)
        {
            string contents = JsonConvert.SerializeObject(fileConfiguration, Formatting.Indented);
            lock (_lock)
            {
                if (System.IO.File.Exists(_environmentFilePath))
                {
                    System.IO.File.Delete(_environmentFilePath);
                }

                System.IO.File.WriteAllText(_environmentFilePath, contents);
                if (System.IO.File.Exists(_ocelotFilePath))
                {
                    System.IO.File.Delete(_ocelotFilePath);
                }

                System.IO.File.WriteAllText(_ocelotFilePath, contents);
            }

            _changeTokenSource.Activate();
            return Task.FromResult((Response)new OkResponse());
        }
    }
}
