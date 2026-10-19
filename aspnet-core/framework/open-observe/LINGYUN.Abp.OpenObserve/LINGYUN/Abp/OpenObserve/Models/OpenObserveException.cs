using System;

namespace LINGYUN.Abp.OpenObserve.Models;

/// <summary>
/// OpenObserve 业务异常基类
/// </summary>
public class OpenObserveException : Exception
{
    /// <summary>
    /// 错误码
    /// </summary>
    /// <remarks>
    /// 业务错误取 OpenObserve 响应中的 code, 否则取 HTTP 状态码
    /// </remarks>
    public int Code => Error?.Code ?? StatusCode;
    /// <summary>
    /// HTTP 状态码
    /// </summary>
    public int StatusCode { get; }
    /// <summary>
    /// OpenObserve 错误响应
    /// </summary>
    public OpenObserveErrorResponse? Error { get; }
    /// <summary>
    /// 原始响应内容
    /// </summary>
    public string? ResponseBody { get; }

    public OpenObserveException(
        string message,
        OpenObserveErrorResponse? error = null,
        int statusCode = 0,
        string? responseBody = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Error = error;
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
