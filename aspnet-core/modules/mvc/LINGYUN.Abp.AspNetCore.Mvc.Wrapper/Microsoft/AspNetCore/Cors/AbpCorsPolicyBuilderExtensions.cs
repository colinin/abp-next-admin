using LINGYUN.Abp.Wrapper;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Microsoft.AspNetCore.Cors;

public static class AbpCorsPolicyBuilderExtensions
{
    public static CorsPolicyBuilder WithAbpWrapExposedHeaders(this CorsPolicyBuilder corsPolicyBuilder)
    {
        return corsPolicyBuilder
            .WithExposedHeaders(
                AbpHttpWrapConsts.AbpWrapResult,
                AbpHttpWrapConsts.AbpDontWrapResult);
    }
}
