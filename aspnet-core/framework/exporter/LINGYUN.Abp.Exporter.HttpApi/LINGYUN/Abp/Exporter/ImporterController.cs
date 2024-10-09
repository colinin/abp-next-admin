using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Exporter;

public abstract class ImporterController<TEntityImportDto> : AbpControllerBase, IImporterAppService<TEntityImportDto>
    where TEntityImportDto : class, new()
{
    private readonly IImporterAppService<TEntityImportDto> _importService;

    protected ImporterController(IImporterAppService<TEntityImportDto> importService)
    {
        _importService = importService;
    }

    [HttpPost]
    [Route("import")]
    public virtual Task ImportAsync(IRemoteStreamContent input)
    {
        return _importService.ImportAsync(input);
    }
}
