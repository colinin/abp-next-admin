using LINGYUN.Abp.Exporter;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Demo.Books;
public interface IBookAppService :
    ICrudAppService< //Defines CRUD methods
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IExporterAppService<BookDto, BookExportListInput>,
    IImporterAppService<BookImportInput>
{
    Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync();
}