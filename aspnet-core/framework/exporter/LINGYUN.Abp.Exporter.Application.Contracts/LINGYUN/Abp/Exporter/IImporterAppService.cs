using System.Threading.Tasks;

namespace LINGYUN.Abp.Exporter;
/// <summary>
/// 数据导入服务接口
/// </summary>
/// <typeparam name="TEntityImportDto"></typeparam>
public interface IImporterAppService<TEntityImportDto>
    where TEntityImportDto: class, new()
{
    /// <summary>
    /// 导入数据（默认从Excel）
    /// </summary>
    /// <param name="input">导入的数据文件流</param>
    /// <returns></returns>
    Task ImportAsync(TEntityImportDto input);
}
