using System;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.Demo.Books;
public class UpdateBookDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    public BookType? Type { get; set; }

    [DataType(DataType.Date)]
    public DateTime? PublishDate { get; set; }

    public float? Price { get; set; }

    public Guid AuthorId { get; set; }
}