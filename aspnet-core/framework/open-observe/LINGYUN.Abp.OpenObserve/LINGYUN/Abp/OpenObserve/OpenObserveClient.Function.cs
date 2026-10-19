using LINGYUN.Abp.OpenObserve.Models;
using LINGYUN.Abp.OpenObserve.Models.Function;
using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OpenObserve;

public partial class OpenObserveClient
{
    /// <summary>
    /// 创建函数
    /// </summary>
    /// <remarks>
    /// POST /api/{organization}/functions
    /// 参考: https://openobserve.ai/docs/reference/api/function/
    /// </remarks>
    /// <param name="name">函数名称</param>
    /// <param name="function">函数体</param>
    /// <param name="transType">函数语言: 0 = VRL, 1 = JavaScript</param>
    /// <param name="streams">函数生效的数据流</param>
    /// <param name="order">执行顺序(文档示例字段, 当前实现未使用)</param>
    public async virtual Task<OpenObserveCodeMessageResponse> CreateFunctionAsync(
        string name,
        string function,
        string? functionParams = null,
        int? numArgs = null,
        FunctionTransType? transType = null,
        FunctionStreamInfo[]? streams = null,
        int? order = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/functions";

        var request = new SaveFunctionRequest
        {
            Name = name,
            Function = function,
            Params = functionParams,
            NumArgs = numArgs,
            TransType = transType,
            Streams = streams,
            Order = order,
        };

        return (await PostJsonAsync<SaveFunctionRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 更新函数
    /// </summary>
    /// <remarks>
    /// PUT /api/{organization}/functions/{name}
    /// 参考: https://openobserve.ai/docs/reference/api/function/
    /// </remarks>
    public async virtual Task<OpenObserveCodeMessageResponse> UpdateFunctionAsync(
        string name,
        string function,
        string? functionParams = null,
        int? numArgs = null,
        FunctionTransType? transType = null,
        FunctionStreamInfo[]? streams = null,
        int? order = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/functions/{Uri.EscapeDataString(name)}";

        var request = new SaveFunctionRequest
        {
            Name = name,
            Function = function,
            Params = functionParams,
            NumArgs = numArgs,
            TransType = transType,
            Streams = streams,
            Order = order,
        };

        return (await PutJsonAsync<SaveFunctionRequest, OpenObserveCodeMessageResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 删除函数
    /// </summary>
    /// <remarks>
    /// DELETE /api/{organization}/functions/{name}
    /// 当流水线依赖该函数时返回 409
    /// 参考: https://openobserve.ai/docs/reference/api/function/
    /// </remarks>
    /// <param name="force">实现声明了该参数用于强制删除, 当前版本服务端未使用</param>
    public async virtual Task<OpenObserveCodeMessageResponse> DeleteFunctionAsync(
        string name,
        bool? force = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var queryString = Utils.OpenObserveQueryString.Build(("force", force));

        var requestUri = $"/api/{ResolveOrganization(organization)}/functions/{Uri.EscapeDataString(name)}{queryString}";

        return (await DeleteAsync<OpenObserveCodeMessageResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 列出函数
    /// </summary>
    /// <remarks>
    /// GET /api/{organization}/functions
    /// 参考: https://openobserve.ai/docs/reference/api/function/
    /// </remarks>
    public async virtual Task<FunctionListResponse> GetFunctionsAsync(
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/functions";

        return (await GetAsync<FunctionListResponse>(requestUri, cancellationToken: cancellationToken))!;
    }

    /// <summary>
    /// 测试函数
    /// </summary>
    /// <remarks>
    /// POST /api/{org_id}/functions/test
    /// 注意: 该接口未出现在 API 文档中, 来自实现源码
    /// </remarks>
    /// <param name="function">函数体</param>
    /// <param name="events">用于测试的样例数据</param>
    /// <param name="transType">函数语言, 不传时由服务端自动识别</param>
    public async virtual Task<TestFunctionResponse> TestFunctionAsync(
        string function,
        JsonNode[] events,
        FunctionTransType? transType = null,
        string? organization = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"/api/{ResolveOrganization(organization)}/functions/test";

        var request = new TestFunctionRequest
        {
            Function = function,
            Events = events,
            TransType = transType,
        };

        return (await PostJsonAsync<TestFunctionRequest, TestFunctionResponse>(
            requestUri,
            request,
            cancellationToken: cancellationToken))!;
    }
}
