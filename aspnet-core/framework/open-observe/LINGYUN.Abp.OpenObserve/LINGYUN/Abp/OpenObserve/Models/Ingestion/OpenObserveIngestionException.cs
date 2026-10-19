namespace LINGYUN.Abp.OpenObserve.Models.Ingestion;

/// <summary>
/// OpenObserve 数据摄取异常
/// </summary>
public class OpenObserveIngestionException : OpenObserveException
{
    /// <summary>
    /// 数据流名称
    /// </summary>
    public string Stream { get; }

    public OpenObserveIngestionException(
        string stream,
        OpenObserveErrorResponse error,
        int statusCode = 0,
        string? responseBody = null)
        : base(error.Message, error, statusCode, responseBody)
    {
        Stream = stream;
    }
}
