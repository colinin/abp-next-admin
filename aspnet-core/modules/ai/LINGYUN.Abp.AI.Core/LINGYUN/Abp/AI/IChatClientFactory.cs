using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IChatClientFactory
{
    [NotNull]
    Task<IWorkspaceChatClient> CreateAsync<TWorkspace>();

    [NotNull]
    Task<IWorkspaceChatClient> CreateAsync(string workspace);
}
