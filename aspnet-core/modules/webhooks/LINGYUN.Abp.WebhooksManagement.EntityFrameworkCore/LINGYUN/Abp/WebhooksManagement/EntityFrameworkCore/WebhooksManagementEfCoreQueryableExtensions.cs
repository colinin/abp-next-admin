using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LINGYUN.Abp.WebhooksManagement.EntityFrameworkCore;

public static class WebhooksManagementEfCoreQueryableExtensions
{
    // 在此聚合仓储服务的扩展方法
    public static IQueryable<WebhookSendRecord> IncludeDetails(this IQueryable<WebhookSendRecord> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            .Include(x => x.WebhookEvent);
    }
}
