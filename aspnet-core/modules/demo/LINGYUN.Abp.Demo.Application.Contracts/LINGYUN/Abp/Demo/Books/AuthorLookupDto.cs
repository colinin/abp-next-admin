using System;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.Demo.Books;
public class AuthorLookupDto : EntityDto<Guid>
{
    public string Name { get; set; }
}