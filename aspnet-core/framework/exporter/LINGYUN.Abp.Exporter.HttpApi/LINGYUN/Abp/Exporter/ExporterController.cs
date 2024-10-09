using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Exporter;
public abstract class ExporterController<TEntityExportDto, TEntityListGetInput> : AbpControllerBase, IExporterAppService<TEntityExportDto, TEntityListGetInput>
    where TEntityExportDto : class, new()
{
    private readonly IExporterAppService<TEntityExportDto, TEntityListGetInput> _exportService;
    protected ExporterController(IExporterAppService<TEntityExportDto, TEntityListGetInput> exportService)
    {
        _exportService = exportService;
    }

    [HttpGet]
    [Route("export")]
    public virtual Task<IRemoteStreamContent> ExportAsync(TEntityListGetInput input)
    {
        return _exportService.ExportAsync(input);
    }
}
