using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Gdpr;

namespace LINGYUN.Abp.Gdpr;

public class GdprDeleteUserAccountContext(IServiceProvider serviceProvider) : GdprUserDataProviderContext, IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
