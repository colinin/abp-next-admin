using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// 数据导出提供者
/// </summary>
[Obsolete("This interface will be deprecated in future versions. Please use IExcelExporterProvider instead.")]
public interface IExporterProvider
{
    /// <summary>
    /// 数据导出
    /// </summary>
    /// <param name="data">导出数据</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Stream> ExportAsync<T>(ICollection<T> data, CancellationToken cancellationToken = default)
        where T : class, new();
}
