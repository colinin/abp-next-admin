using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Authorization
{
    public interface IUserWeChatCodeFinder
    {
        Task<string> FindByUserIdAsync(Guid userId);

        Task<string> FindByUserNameAsync(string userName);
    }
}
