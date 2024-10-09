using LINGYUN.Abp.DataProtection;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.Demo.Books;
public class Book : AuditedAggregateRoot<Guid>, IDataProtected
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public Guid AuthorId { get; set; }
}
