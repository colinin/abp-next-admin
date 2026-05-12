using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// Word数据导出
/// </summary>
public interface IWordExporterProvider
{
    /// <summary>
    /// 数据导出
    /// </summary>
    /// <param name="data">导出数据</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default);
    /// <summary>
    /// 数据导出
    /// </summary>
    /// <param name="template">模板数据</param>
    /// <param name="data">导出数据</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> ExportAsync(byte[] template, object data, CancellationToken cancellationToken = default);
}
