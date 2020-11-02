using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.Authorization
{
    public class NullUserWeChatCodeFinder : IUserWeChatCodeFinder, ISingletonDependency
    {
        public Task<string> FindByUserIdAsync(Guid userId)
        {
            return Task.FromResult(userId.ToString());
        }

        public Task<string> FindByUserNameAsync(string userName)
        {
            return Task.FromResult(userName);
        }
    }
}
