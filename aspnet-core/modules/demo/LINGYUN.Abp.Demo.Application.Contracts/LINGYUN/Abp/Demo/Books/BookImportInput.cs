using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Demo.Books;
public class BookImportInput
{
    [Required]
    public IRemoteStreamContent Content { get; set; }   
}
