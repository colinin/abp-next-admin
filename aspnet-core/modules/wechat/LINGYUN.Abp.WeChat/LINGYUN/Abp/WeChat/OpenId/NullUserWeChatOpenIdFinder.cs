using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.WeChat.OpenId
{
    public class NullUserWeChatOpenIdFinder : IUserWeChatOpenIdFinder, ISingletonDependency
    {
        public Task<string> FindByUserIdAsync(Guid userId, string provider)
        {
            return Task.FromResult("");
        }

        public Task<string> FindByUserNameAsync(string userName, string provider)
        {
            return Task.FromResult("");
        }
    }
}
