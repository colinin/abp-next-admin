using JetBrains.Annotations;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI;
public interface IChatClientFactory
{
    [NotNull]
    Task<IChatClient> CreateAsync<TWorkspace>();

    [NotNull]
    Task<IChatClient> CreateAsync(string workspace);
}
