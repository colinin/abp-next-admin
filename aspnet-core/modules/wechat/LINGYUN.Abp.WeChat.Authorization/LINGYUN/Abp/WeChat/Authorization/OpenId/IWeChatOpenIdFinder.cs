using System;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Authorization
{
    public interface IWeChatOpenIdFinder
    {
        Task<WeChatOpenId> FindAsync(string code);
    }
}
