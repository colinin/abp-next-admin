using LINGYUN.Abp.BackgroundTasks;
using LINGYUN.Abp.Saas.Features;
using LINGYUN.Abp.Saas.Tenants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.Saas.Jobs;
public class TenantUsageMonitoringJob : IJobRunnable
{
    #region Definition Paramters

    public readonly static IReadOnlyList<JobDefinitionParamter> Paramters =
        new List<JobDefinitionParamter>
        {
            new JobDefinitionParamter(PropertyAdminEmail, LocalizableStatic.Create("Saas:AdminEmail"), required: true),
            new JobDefinitionParamter(PropertyTenantId, LocalizableStatic.Create("Saas:TenantId"), required: true),
        };

    #endregion

    public const string PropertyAdminEmail = "AdminEmail";
    public const string PropertyTenantId = "TenantId";

    public async virtual Task ExecuteAsync(JobRunnableContext context)
    {
        var clock = context.GetRequiredService<IClock>();
        var currentTenant = context.GetRequiredService<ICurrentTenant>();
        var repository = context.GetRequiredService<ITenantRepository>();
        var featureChecker = context.GetRequiredService<IFeatureChecker>();
        using (currentTenant.Change(null))
        {
            var tenantId = context.GetJobData<Guid>(PropertyTenantId);
            // TODO: 租户已删除需要取消作业运行
            var tenant = await repository.GetAsync(tenantId);
            if (!tenant.DisableTime.HasValue)
            {
                return;
            }
            using (currentTenant.Change(tenantId))
            {
                var allowExpirationDays = await featureChecker.GetAsync(SaasFeatureNames.Tenant.ExpirationReminderDays, 15);
                if (tenant.DisableTime <= clock.Now.AddDays(allowExpirationDays))
                {
                    var adminEmail = context.GetString(PropertyAdminEmail);
                    var emailSender = context.GetRequiredService<IEmailSender>();
                    // TODO: 需要使用模板发送
                    await emailSender.SendAsync(adminEmail, "资源超时预警", "您好, 您的平台资源已到超时预警时间, 过期资源将会被回收, 且无法恢复, 请注意及时延长使用时间或对资源备份!");
                    return;
                }

                var expiredRecoveryTime = await featureChecker.GetAsync(SaasFeatureNames.Tenant.ExpiredRecoveryTime, 15);
                if (clock.Now > tenant.DisableTime && clock.Now.Subtract(TimeSpan.FromDays(expiredRecoveryTime)) <= tenant.DisableTime)
                {
                    // 租户已过期且达到最大宽限时间, 删除租户
                    await repository.DeleteAsync(tenant);
                }
            }
        }
    }
}
