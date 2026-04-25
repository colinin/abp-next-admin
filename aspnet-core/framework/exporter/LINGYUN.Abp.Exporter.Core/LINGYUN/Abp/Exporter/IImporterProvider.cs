using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// 数据导入提供者
/// </summary>
[Obsolete("This interface will be deprecated in future versions. Please use IExcelImporterProvider instead.")]
public interface IImporterProvider
{
    /// <summary>
    /// 数据导入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<T>> ImportAsync<T>(Stream stream)
        where T : class, new();
}
