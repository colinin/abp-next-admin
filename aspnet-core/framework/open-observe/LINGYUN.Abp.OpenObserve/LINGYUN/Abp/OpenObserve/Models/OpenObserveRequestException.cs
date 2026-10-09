using System.Net.Http;

namespace LINGYUN.Abp.OpenObserve.Models;

/// <summary>
/// OpenObserve 请求异常
/// </summary>
/// <remarks>
/// 响应内容无法解析为 OpenObserve 标准错误响应时抛出
/// </remarks>
public class OpenObserveRequestException : OpenObserveException
{
    public HttpResponseMessage HttpResponseMessage { get; }

    public OpenObserveRequestException(
        HttpResponseMessage responseMessage,
        string? responseBody = null)
        : base(
            $"OpenObserve request failed with status code {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase})",
            null,
            (int)responseMessage.StatusCode,
            responseBody)
    {
        HttpResponseMessage = responseMessage;
    }
}
