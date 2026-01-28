using LINGYUN.Abp.AI.Models;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tokens;
public interface ITokenUsageStore
{
    Task SaveTokenUsageAsync(TokenUsageInfo tokenUsageInfo);
}
