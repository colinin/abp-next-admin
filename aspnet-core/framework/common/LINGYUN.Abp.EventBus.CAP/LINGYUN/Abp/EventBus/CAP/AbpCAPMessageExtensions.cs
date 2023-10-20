using DotNetCore.CAP.Messages;
using System;

namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// CAP消息扩展
    /// </summary>
    public static class AbpCAPMessageExtensions
    {
        /// <summary>
        /// 尝试获取消息标头中的租户标识
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool TryGetTenantId(
            this Message message,
            out Guid? tenantId)
        {
            if (message.Headers.TryGetValue(AbpCAPHeaders.TenantId, out string tenantStr))
            {
                if (Guid.TryParse(tenantStr, out Guid id))
                {
                    tenantId = id;
                    return true;
                }
            }
            tenantId = null;
            return false;
        }
        /// <summary>
        /// 获取消息标头中的租户标识
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Guid? GetTenantIdOrNull(
            this Message message)
        {
            if (message.TryGetTenantId(out Guid? tenantId))
            {
                return tenantId;
            }
            return null;
        }
    }
}
