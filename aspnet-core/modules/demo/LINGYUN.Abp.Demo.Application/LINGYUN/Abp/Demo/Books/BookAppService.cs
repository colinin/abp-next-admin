using LINGYUN.Abp.DataProtection;
using LINGYUN.Abp.DataProtection.Models;
using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Localization;
using LINGYUN.Abp.Demo.Permissions;
using LINGYUN.Abp.Exporter;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Localization;

namespace LINGYUN.Abp.Demo.Books;

[Authorize(DemoPermissions.Books.Default)]
public class BookAppService : DemoApplicationServiceBase, IBookAppService //implement the IBookAppService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly AuthorManager _authorManager;

    private readonly IExcelExporterProvider _exporterProvider;
    private readonly IExcelImporterProvider _importerProvider;

    protected IDataAccessEntityTypeInfoProvider EntityTypeInfoProvider => LazyServiceProvider.LazyGetRequiredService<IDataAccessEntityTypeInfoProvider>();

    public BookAppService(
        IExcelExporterProvider exporterProvider,
        IExcelImporterProvider importerProvider,
        IBookRepository bookRepository,
        AuthorManager authorManager,
        IAuthorRepository authorRepository)
    {
        _exporterProvider = exporterProvider;
        _importerProvider = importerProvider;
        _authorManager = authorManager;
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;

        LocalizationResource = typeof(DemoResource);
        ObjectMapperContext = typeof(AbpDemoApplicationModule);
    }

    [Authorize(DemoPermissions.Books.Create)]
    [DisableDataProtected]// 任何人都可创建
    public async virtual Task<BookDto> CreateAsync(CreateBookDto input)
    {
        var author = await _authorRepository.GetAsync(input.AuthorId);
        var book = new Book(
            GuidGenerator.Create(),
            input.Name,
            input.Type,
            input.PublishDate,
            input.Price,
            author.Id);

        await _bookRepository.InsertAsync(book);

        var bookDto = ObjectMapper.Map<Book, BookDto>(book);
        bookDto.AuthorName = author.Name;

        return bookDto;
    }

    [Authorize(DemoPermissions.Books.Edit)]
    [DataProtected(DataAccessOperation.Write)]// 仅启用数据更新过滤器
    public async virtual Task<BookDto> UpdateAsync(Guid id, UpdateBookDto input)
    {
        var book = await _bookRepository.GetAsync(id);

        if (!input.Name.IsNullOrWhiteSpace() && !string.Equals(input.Name, book.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            book.Name = input.Name;
        }
        if (input.Type.HasValue && input.Type != book.Type)
        {
            book.Type = input.Type.Value;
        }
        if (input.PublishDate.HasValue && input.PublishDate != book.PublishDate)
        {
            book.PublishDate = input.PublishDate.Value;
        }
        if (input.Price.HasValue && input.Price != book.Price)
        {
            book.Price = input.Price.Value;
        }

        if (input.AuthorId != book.AuthorId)
        {
            var newAuthor = await _authorRepository.GetAsync(input.AuthorId);
            book.AuthorId = newAuthor.Id;
        }

        await _bookRepository.UpdateAsync(book);
        var author = await _authorRepository.GetAsync(input.AuthorId);

        var bookDto = ObjectMapper.Map<Book, BookDto>(book);
        bookDto.Name = author.Name;

        return bookDto;
    }

    public async virtual Task<BookDto> GetAsync(Guid id)
    {
        var book = await _bookRepository.GetAsync(id);
        var author = await _authorRepository.GetAsync(book.AuthorId);

        var bookDto = ObjectMapper.Map<Book, BookDto>(book);
        bookDto.AuthorName = author.Name;
        return bookDto;
    }

    public async virtual Task<PagedResultDto<BookDto>> GetListAsync(BookGetListInput input)
    {
        Expression<Func<Book, bool>> expression = _ => true;
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            expression = expression.And(x => x.Name.Contains(input.Filter!));
        }
        var specification = new Volo.Abp.Specifications.ExpressionSpecification<Book>(expression);

        var totalCount = await _bookRepository.GetCountAsync(specification);
        var books = await _bookRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        var bookDtos = ObjectMapper.Map<List<Book>, List<BookDto>>(books);

        var authors = await _authorRepository.GetListAsync(
            new Volo.Abp.Specifications.ExpressionSpecification<Author>(
                x => bookDtos.Select(b => b.AuthorId).Distinct().Contains(x.Id)));

        foreach (var bookDto in bookDtos)
        {
            bookDto.AuthorName = authors.Find(x => x.Id == bookDto.AuthorId)!.Name;
        }

        return new PagedResultDto<BookDto>(
            totalCount,
            bookDtos
        );
    }


    [Authorize(DemoPermissions.Books.Delete)]
    [DataProtected(DataAccessOperation.Delete)]// 仅启用数据删除过滤器
    public async virtual Task DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetAsync(id);

        await _bookRepository.DeleteAsync(book);
    }

    [Authorize(DemoPermissions.Books.Import)]
    [DisableDataProtected]// 任何人都可创建
    public async virtual Task ImportAsync(BookImportInput input)
    {
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
            var book = new Book(
                GuidGenerator.Create(),
                dto.Name,
                dto.Type,
                dto.PublishDate,
                dto.Price,
                dto.AuthorId);

            return book;
        });

        if (createAuthors.Count > 0)
        {
            await _authorRepository.InsertManyAsync(createAuthors, autoSave: true);
        }

        await _bookRepository.InsertManyAsync(createManyBooks, autoSave: true);
    }

    [Authorize(DemoPermissions.Books.Export)]
    public async virtual Task<IRemoteStreamContent> ExportAsync(BookExportListInput input)
    {
        var bookSet = await _bookRepository.GetQueryableAsync();
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
