using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// Excel数据导出
/// </summary>
public interface IExcelExporterProvider
{
    /// <summary>
    /// 数据导出
    /// </summary>
    /// <param name="data">导出数据</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> ExportAsync(object data, CancellationToken cancellationToken = default);
}
