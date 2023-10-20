using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.Json.SystemTextJson;

namespace LINGYUN.Abp.AspNetCore.SignalR.Protocol.Json
{
    public class JsonHubProtocolOptionsSetup : IConfigureOptions<JsonHubProtocolOptions>
    {
        protected IServiceProvider ServiceProvider { get; }

        public JsonHubProtocolOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(JsonHubProtocolOptions options)
        {
            options.PayloadSerializerOptions = ServiceProvider.GetRequiredService<IOptions<AbpSystemTextJsonSerializerOptions>>().Value.JsonSerializerOptions;
        }
    }
}
