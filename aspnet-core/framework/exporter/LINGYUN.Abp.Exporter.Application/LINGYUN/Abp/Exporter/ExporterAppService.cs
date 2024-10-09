using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Exporter;

public abstract class ExporterAppService<TEntity, TEntityExportDto, TEntityListGetInput> : ApplicationService, IExporterAppService<TEntityExportDto, TEntityListGetInput>
    where TEntityExportDto : class, new()
{
    private readonly IExporterProvider _exporterProvider;

    protected ExporterAppService(IExporterProvider exporterProvider)
    {
        _exporterProvider = exporterProvider;
    }

    public async virtual Task<IRemoteStreamContent> ExportAsync(TEntityListGetInput input)
    {
        var fileName = GetExportFileName(input);

        var entities = await GetListAsync(input);
        var entitieDtoList = MapEntitiesToDto(entities);

        var stream = await _exporterProvider.ExportAsync(entitieDtoList);

        return new RemoteStreamContent(stream, fileName);
    }
    /// <summary>
    /// 实现方法用以返回导出文件名
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected abstract string GetExportFileName(TEntityListGetInput input);
    /// <summary>
    /// 实现方法用以查询需要导出的实体列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected abstract Task<List<TEntity>> GetListAsync(TEntityListGetInput input);
    /// <summary>
    /// 实现方法用以实体数据传输对象映射
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    protected virtual List<TEntityExportDto> MapEntitiesToDto(List<TEntity> entities)
    {
        return ObjectMapper.Map<List<TEntity>, List<TEntityExportDto>>(entities);
    }
}
