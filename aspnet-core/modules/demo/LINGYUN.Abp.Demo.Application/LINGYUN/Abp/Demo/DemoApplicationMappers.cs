using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace LINGYUN.Abp.Demo;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BookToBookDtoMapper : MapperBase<Book, BookDto>
{
    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial BookDto Map(Book source);

    [MapperIgnoreTarget(nameof(BookDto.AuthorName))]
    public override partial void Map(Book source, BookDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AuthorToAuthorDtoMapper : MapperBase<Author, AuthorDto>
{
    public override partial AuthorDto Map(Author source);
    public override partial void Map(Author source, AuthorDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AuthorToAuthorLookupDtoMapper : MapperBase<Author, AuthorLookupDto>
{
    public override partial AuthorLookupDto Map(Author source);
    public override partial void Map(Author source, AuthorLookupDto destination);
}
