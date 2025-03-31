using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.Models;
using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Localization;
using LINGYUN.Abp.Demo.Permissions;
using LINGYUN.Abp.Exporter;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Demo.Books;

[Authorize(DemoPermissions.Books.Default)]
public class BookAppService :
    CrudAppService<
        Book, //The Book entity
        BookDto, //Used to show books
        Guid, //Primary key of the book entity
        PagedAndSortedResultRequestDto, //Used for paging/sorting
        CreateUpdateBookDto>, //Used to create/update a book
    IBookAppService //implement the IBookAppService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;

    private readonly IExporterProvider _exporterProvider;
    private readonly IImporterProvider _importerProvider;

    protected IDataAccessEntityTypeInfoProvider EntityTypeInfoProvider => LazyServiceProvider.LazyGetRequiredService<IDataAccessEntityTypeInfoProvider>();

    public BookAppService(
        IExporterProvider exporterProvider,
        IImporterProvider importerProvider,
        IBookRepository bookRepository,
        AuthorManager authorManager,
        IAuthorRepository authorRepository)
        : base(bookRepository)
    {
        _exporterProvider = exporterProvider;
        _importerProvider = importerProvider;
        _authorManager = authorManager;
        _authorRepository = authorRepository;

        GetPolicyName = DemoPermissions.Books.Default;
        GetListPolicyName = DemoPermissions.Books.Default;
        CreatePolicyName = DemoPermissions.Books.Create;
        UpdatePolicyName = DemoPermissions.Books.Edit;
        DeletePolicyName = DemoPermissions.Books.Delete;

        LocalizationResource = typeof(DemoResource);
        ObjectMapperContext = typeof(AbpDemoApplicationModule);
    }
    
    [DisableDataProtected(DataAccessOperation.Read)]// 更新时禁用查询过滤
    public override Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
    {
        return base.UpdateAsync(id, input);
    }

    [DisableDataProtected(DataAccessOperation.Read)]
    public override Task DeleteAsync(Guid id)
    {
        return base.DeleteAsync(id);
    }

    public async virtual Task ImportAsync(BookImportInput input)
    {
        await CheckCreatePolicyAsync();
        await CheckPolicyAsync(DemoPermissions.Books.Import);

        var stream = input.Content.GetStream();
        var createAuthors = new List<Author>();
        var existsAuthors = new List<Author>();
        var createManyDtos = await _importerProvider.ImportAsync<BookImportDto>(stream);

        foreach (var book in createManyDtos)
        {
            var author = existsAuthors.Find(x => x.Name == book.AuthorName) ??
                await _authorRepository.FindByNameAsync(book.AuthorName);
            if (author == null)
            {
                author = await _authorManager.CreateAsync(book.AuthorName, Clock.Now);
                createAuthors.Add(author);
            }
            if (!existsAuthors.Any(x => x.Name == author.Name))
            {
                existsAuthors.Add(author);
            }
            book.AuthorId = author.Id;
        }

        var createManyBooks = createManyDtos.Select(dto =>
        {
            var book = ObjectMapper.Map<BookImportDto, Book>(dto);

            TryToSetTenantId(book);

            return book;
        });

        if (createAuthors.Count > 0)
        {
            await _authorRepository.InsertManyAsync(createAuthors, autoSave: true);
        }

        await Repository.InsertManyAsync(createManyBooks, autoSave: true);
    }

    public async virtual Task<IRemoteStreamContent> ExportAsync(BookExportListInput input)
    {
        await CheckPolicyAsync(DemoPermissions.Books.Export);

        var bookSet = await Repository.GetQueryableAsync();
        var authorSet = await _authorRepository.GetQueryableAsync();

        var query = from book in bookSet
                    join author in authorSet on book.AuthorId equals author.Id
                    select new { book, author };

        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Take(input.MaxResultCount);

        var queryResult = await AsyncExecuter.ToListAsync(query);

        var bookDtos = queryResult.Select(x =>
        {
            var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
            bookDto.AuthorName = x.author.Name;
            return bookDto;
        }).ToList();

        var stream = await _exporterProvider.ExportAsync(bookDtos);

        return new RemoteStreamContent(stream, input.FileName);
    }

    public override async Task<BookDto> GetAsync(Guid id)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    where book.Id == id
                    select new { book, author };

        //Execute the query and get the book with author
        var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
        if (queryResult == null)
        {
            throw new EntityNotFoundException(typeof(Book), id);
        }

        var bookDto = ObjectMapper.Map<Book, BookDto>(queryResult.book);
        bookDto.AuthorName = queryResult.author.Name;
        return bookDto;
    }

    public override async Task<PagedResultDto<BookDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //Get the IQueryable<Book> from the repository
        var queryable = await Repository.GetQueryableAsync();

        //Prepare a query to join books and authors
        var query = from book in queryable
                    join author in await _authorRepository.GetQueryableAsync() on book.AuthorId equals author.Id
                    select new { book, author };

        //Paging
        query = query
            .OrderBy(NormalizeSorting(input.Sorting))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //Execute the query and get a list
        var queryResult = await AsyncExecuter.ToListAsync(query);

        //Convert the query result to a list of BookDto objects
        var bookDtos = queryResult.Select(x =>
        {
            var bookDto = ObjectMapper.Map<Book, BookDto>(x.book);
            bookDto.AuthorName = x.author.Name;
            return bookDto;
        }).ToList();

        //Get the total count with another query
        var totalCount = await Repository.GetCountAsync();

        return new PagedResultDto<BookDto>(
            totalCount,
            bookDtos
        );
    }

    public async Task<ListResultDto<AuthorLookupDto>> GetAuthorLookupAsync()
    {
        var authors = await _authorRepository.GetListAsync();

        return new ListResultDto<AuthorLookupDto>(
            ObjectMapper.Map<List<Author>, List<AuthorLookupDto>>(authors)
        );
    }

    private static string NormalizeSorting(string? sorting)
    {
        if (sorting.IsNullOrEmpty())
        {
            return $"book.{nameof(Book.Name)}";
        }

        if (sorting.Contains("authorName", StringComparison.OrdinalIgnoreCase))
        {
            return sorting.Replace(
                "authorName",
                "author.Name",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return $"book.{sorting}";
    }

    public async virtual Task<EntityTypeInfoModel> GetEntityRuleAsync(EntityTypeInfoGetModel input)
    {
        var entityType = typeof(Book);
        var resourceType = LocalizationResource ?? typeof(DefaultResource);

        var context = new DataAccessEntitTypeInfoContext(
            entityType,
            resourceType,
            input.Operation,
            LazyServiceProvider);

        return await EntityTypeInfoProvider.GetEntitTypeInfoAsync(context);
    }
}