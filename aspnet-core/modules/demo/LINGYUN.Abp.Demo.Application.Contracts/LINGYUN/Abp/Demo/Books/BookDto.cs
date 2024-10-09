using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Demo.Books;
public class BookDto : AuditedEntityDto<Guid>
{
    [DisplayName("名称")]
    public string Name { get; set; }

    [DisplayName("类型")]
    public BookType Type { get; set; }

    [DisplayName("出版日期")]
    public DateTime PublishDate { get; set; }

    [DisplayName("价格")]
    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    [DisplayName("作者")]
    public string AuthorName { get; set; }
}
