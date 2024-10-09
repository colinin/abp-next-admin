using Volo.Abp;

namespace LINGYUN.Abp.Demo.Authors;
public class AuthorAlreadyExistsException : BusinessException
{
    public AuthorAlreadyExistsException(string name)
        : base(DemoErrorCodes.Author.AuthorAlreadyExists)
    {
        WithData("Name", name);
    }
}
