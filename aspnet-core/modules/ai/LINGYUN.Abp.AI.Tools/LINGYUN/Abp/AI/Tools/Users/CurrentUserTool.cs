using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace LINGYUN.Abp.AI.Tools.Users;

public class CurrentUserTool : ITransientDependency
{
    public const string Name = "GetUserInfo";

    private readonly ICurrentUser _currentUser;

    public CurrentUserTool(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public object? Invoke()
    {
        return _currentUser;
    }
}
