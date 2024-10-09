using System.Threading.Tasks;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// 数据导出服务接口
/// </summary>
/// <typeparam name="TEntityExportDto">导出的实体数据传输对象</typeparam>
/// <typeparam name="TEntityListGetInput">实体数据过滤数据对象</typeparam>
public interface IExporterAppService<TEntityExportDto, TEntityListGetInput>
     where TEntityExportDto : class, new()
{
    /// <summary>
    /// 导出数据（默认实现Excel）
    /// </summary>
    /// <param name="input">数据过滤条件</param>
    /// <returns></returns>
    Task<IRemoteStreamContent> ExportAsync(TEntityListGetInput input);
}
