using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Demo.Books;
public class BookImportDto
{
    [Required]
    [StringLength(128)]
    [DisplayName("名称")]
    public string Name { get; set; }

    [Required]
    [DisplayName("类型")]
    public BookType Type { get; set; } = BookType.Undefined;

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("出版日期")]
    public DateTime PublishDate { get; set; } = DateTime.Now;

    [Required]
    [DisplayName("价格")]
    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    [DisplayName("作者")]
    public string AuthorName { get; set; }
}
