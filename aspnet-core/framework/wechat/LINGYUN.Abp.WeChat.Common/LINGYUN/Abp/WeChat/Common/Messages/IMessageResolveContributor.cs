using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Common.Messages;
public interface IMessageResolveContributor
{
    string Name { get; }

    Task ResolveAsync(IMessageResolveContext context);
}
