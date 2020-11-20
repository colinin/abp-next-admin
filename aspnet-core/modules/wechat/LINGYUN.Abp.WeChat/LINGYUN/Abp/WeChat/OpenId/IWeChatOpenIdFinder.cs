using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.OpenId
{
    public interface IWeChatOpenIdFinder
    {
        Task<WeChatOpenId> FindAsync(string code, string appId, string appSecret);
    }
}
