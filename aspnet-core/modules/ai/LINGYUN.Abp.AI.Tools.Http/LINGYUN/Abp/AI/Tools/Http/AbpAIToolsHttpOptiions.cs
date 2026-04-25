using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tools.Http;
public class AbpAIToolsHttpOptiions
{
    public List<Action<IHttpClientBuilder>> ClientBuildActions { get; }

    public List<Action<IServiceProvider, HttpClient>> ClientActions { get; }

    public List<Action<IServiceProvider, HttpClientHandler>> ClientHandlerActions { get; }
    /// <summary>
    /// 自定义Http请求响应处理
    /// </summary>
    /// <remarks>
    /// 参数1: HttpAITool工具名称
    /// 参数2: IServiceProvider
    /// 参数3: 请求响应
    /// </remarks>
    public IDictionary<string, Func<IServiceProvider, HttpResponseMessage, Task<object>>> HttpResponseActions { get; }
    public AbpAIToolsHttpOptiions()
    {
        ClientBuildActions = new List<Action<IHttpClientBuilder>>();
        ClientActions = new List<Action<IServiceProvider, HttpClient>>();
        ClientHandlerActions = new List<Action<IServiceProvider, HttpClientHandler>>();
        HttpResponseActions = new Dictionary<string, Func<IServiceProvider, HttpResponseMessage, Task<object>>>();
    }
}
