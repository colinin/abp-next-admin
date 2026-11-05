using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.OperationRateLimiting;

[DependsOn(
    typeof(LINGYUN.Abp.Tests.AbpTestsBaseModule),
    typeof(AbpOperationRateLimitingModule))]
public class AbpOperationRateLimitingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<FakeOperationRateLimitingPolicyStoreState>();
        context.Services.Replace(ServiceDescriptor.Singleton<IDynamicOperationRateLimitingPolicyStore>(
            serviceProvider => new FakeDynamicOperationRateLimitingPolicyStore(
                serviceProvider.GetRequiredService<FakeOperationRateLimitingPolicyStoreState>())));

        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            options.AddPartitionKeyResolver("ByDevice", rateLimitingContext =>
                Task.FromResult<string?>(
                    $"{rateLimitingContext.Parameter}:{rateLimitingContext.Properties.GetValueOrDefault("DeviceId")}"));

            options.AddPolicy(TestPolicyNames.Fixed, policy =>
                policy.WithFixedWindow(TimeSpan.FromMilliseconds(300), maxCount: 2)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.Sliding, policy =>
                policy.WithSlidingWindow(TimeSpan.FromMilliseconds(400), maxCount: 2, segments: 4)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.TokenBucket, policy =>
                policy.WithTokenBucket(TimeSpan.FromMilliseconds(250), tokenLimit: 2, tokensPerPeriod: 1)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.Concurrency, policy =>
                policy.WithConcurrency(permitLimit: 2, leaseDuration: TimeSpan.FromMilliseconds(300))
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.Email, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByEmail());

            options.AddPolicy(TestPolicyNames.PhoneNumber, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByPhoneNumber());

            options.AddPolicy(TestPolicyNames.MultiRule, policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 5)
                    .PartitionByCurrentTenant());
            });

            options.AddPolicy(TestPolicyNames.Combined, policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 2)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());
            });

            options.AddPolicy(TestPolicyNames.Ban, policy =>
                policy.WithFixedWindow(TimeSpan.FromHours(1), maxCount: 0)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.CustomError, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter()
                    .WithErrorCode("Test:CustomError"));

            options.AddPolicy(TestPolicyNames.CustomPartition, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionBy("ByDevice"));

            options.AddPolicy(TestPolicyNames.Tenant, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .WithMultiTenancy()
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.DisabledPolicy, policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter();
                policy.WithEnabled(false);
            });

            options.AddPolicy(TestPolicyNames.Merged, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.Attribute, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.ClassLevel, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.DynamicOnly, policy =>
                policy.WithFixedWindow(TimeSpan.FromMinutes(1), maxCount: 1)
                    .PartitionByParameter());

            options.AddPolicy(TestPolicyNames.CustomAlgorithm, policy =>
                policy.AddRule<TestCustomRateLimitingRule>(rule =>
                {
                    rule.WithMaxCount(2)
                        .WithDuration(TimeSpan.FromMinutes(1));
                }));
        });
    }
}
