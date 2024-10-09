using LINGYUN.Abp.Demo.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Demo.Books;

[Controller]
[Authorize(DemoPermissions.Books.Default)]
[RemoteService(Name = DemoRemoteServiceConsts.RemoteServiceName)]
[Area(DemoRemoteServiceConsts.ModuleName)]
[Route($"api/{DemoRemoteServiceConsts.ModuleName}/books")]
public class BookController : AbpControllerBase, IBookAppService
{
    private readonly IBookAppService _service;

    public BookController(IBookAppService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(DemoPermissions.Books.Create)]
    public virtual Task<BookDto> CreateAsync(CreateUpdateBookDto input)
    {
        return _service.CreateAsync(input);
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(DemoPermissions.Books.Delete)]
    public virtual Task DeleteAsync(Guid id)
    {
        return _service.DeleteAsync(id);
    }

    [HttpPost]
    [Route("import")]
    public virtual Task ImportAsync([FromForm] BookImportInput input)
    {
        return _service.ImportAsync(input);
    }

    [HttpGet]
    [Route("export")]
    public virtual Task<IRemoteStreamContent> ExportAsync(BookExportListInput input)
    {
        return _service.ExportAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<BookDto> GetAsync(Guid id)
    {
        return _service.GetAsync(id);
    }

    [HttpGet]
    [Route("lookup")]
    public virtual Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        return _service.GetAuthorLookupAsync();
    }

    [HttpGet]
    public virtual Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        return _service.GetListAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(DemoPermissions.Books.Edit)]
    public virtual Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
    {
        return _service.UpdateAsync(id, input);
    }
}
