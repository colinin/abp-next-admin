using LINGYUN.Abp.DataProtection;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;

namespace LINGYUN.Abp.Demo.Books;
public class Book : AuditedAggregateRoot<Guid>, IDataProtected
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public Guid AuthorId { get; set; }

    protected Book()
    {
        ExtraProperties = new ExtraPropertyDictionary();
    }

    public Book(
        Guid id,
        string name, 
        BookType type, 
        DateTime publishDate,
        float price, 
        Guid authorId)
        : base(id)
    {
        Name = name;
        Type = type;
        PublishDate = publishDate;
        Price = price;
        AuthorId = authorId;

        ExtraProperties = new ExtraPropertyDictionary();
    }
}
