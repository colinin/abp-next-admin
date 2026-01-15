using LINGYUN.Abp.AI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.AI.Tokens;
public interface ITokenUsageStore
{
    Task SaveTokenUsagesAsync(IEnumerable<TokenUsageInfo> usageInfos);
}
