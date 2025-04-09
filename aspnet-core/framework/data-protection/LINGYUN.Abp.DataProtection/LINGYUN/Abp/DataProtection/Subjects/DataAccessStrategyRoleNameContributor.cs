using LINGYUN.Abp.DataProtection.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Users;

namespace LINGYUN.Abp.DataProtection.Subjects;

/// <summary>
/// 角色数据权限策略
/// </summary>
public class DataAccessStrategyRoleNameContributor : IDataAccessStrategyContributor
{
    public string Name => RolePermissionValueProvider.ProviderName;

    public async virtual Task<DataAccessStrategyState> GetOrNullAsync(DataAccessStrategyContributorContext context)
    {
        var states = new List<DataAccessStrategyState>();
        var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
        if (!currentUser.IsAuthenticated)
        {
            return null;
        }
        var store = context.ServiceProvider.GetRequiredService<IDataProtectedStrategyStateStore>();
        foreach (var userRole in currentUser.Roles)
        {
            var strategyState = await store.GetOrNullAsync(Name, userRole);
            if (strategyState != null)
            {
                states.Add(strategyState);
            }
        }

        // 多个角色配置过策略时, 取权重最大的策略生效
        return states.OrderByDescending(x => x.Strategy).FirstOrDefault();
    }
}
