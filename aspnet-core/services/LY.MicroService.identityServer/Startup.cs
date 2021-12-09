using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Volo.Abp.IO;
using Volo.Abp.Modularity.PlugIns;

namespace LY.MicroService.IdentityServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<IdentityServerModule>(options =>
        {
            // 搜索 Modules 目录下所有文件作为插件
            // 取消显示引用所有其他项目的模块，改为通过插件的形式引用
            var pluginFolder = Path.Combine(
                    Directory.GetCurrentDirectory(), "Modules");
            DirectoryHelper.CreateIfNotExists(pluginFolder);
            options.PlugInSources.AddFolder(
                pluginFolder,
                SearchOption.AllDirectories);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
