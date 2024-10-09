using AutoMapper;
using LINGYUN.Abp.Demo.Authors;
using LINGYUN.Abp.Demo.Books;
using Volo.Abp.AutoMapper;

namespace LINGYUN.Abp.Demo;
public class DemoApplicationMapperProfile : Profile
{
    public DemoApplicationMapperProfile()
    {
        CreateMap<Book, BookDto>()
            .Ignore(dto => dto.AuthorName);
        CreateMap<BookImportDto, Book>()
            .IgnoreAuditedObjectProperties()
            .Ignore(dto => dto.Id)
            .Ignore(dto => dto.ExtraProperties)
            .Ignore(dto => dto.ConcurrencyStamp);
        CreateMap<CreateUpdateBookDto, Book>()
            .IgnoreAuditedObjectProperties()
            .Ignore(dto => dto.Id)
            .Ignore(dto => dto.ExtraProperties)
            .Ignore(dto => dto.ConcurrencyStamp);
        CreateMap<Author, AuthorDto>();
        CreateMap<Author, AuthorLookupDto>();
    }
}
