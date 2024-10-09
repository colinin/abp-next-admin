using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Exporter;

public abstract class ImporterAppService<TEntity, TEntityImportDto> : ApplicationService, IImporterAppService<TEntityImportDto>
    where TEntityImportDto : class, new()
{
    private readonly IImporterProvider _importerProvider;
    protected ImporterAppService(IImporterProvider importerProvider)
    {
        _importerProvider = importerProvider;
    }

    public async virtual Task ImportAsync(TEntityImportDto input)
    {
        var stream = await GetImportStream(input);
        var entitieDtoList = await _importerProvider.ImportAsync<TEntityImportDto>(stream);

        var entities = MapDtoToEntities([.. entitieDtoList]);

        await SaveManyEntities(entities);
    }
    /// <summary>
    /// 实现方法用以从导入数据中提取文件流
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected abstract Task<Stream> GetImportStream(TEntityImportDto input);
    /// <summary>
    /// 实现方法用以保存实体列表到持久化设施
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    protected abstract Task SaveManyEntities(ICollection<TEntity> entities);
    /// <summary>
    /// 实现方法用以实体数据传输对象映射
    /// </summary>
    /// <param name="entitieDtoList"></param>
    /// <returns></returns>
    protected virtual List<TEntity> MapDtoToEntities(List<TEntityImportDto> entitieDtoList)
    {
        return ObjectMapper.Map<List<TEntityImportDto>, List<TEntity>>(entitieDtoList);
    }
}
