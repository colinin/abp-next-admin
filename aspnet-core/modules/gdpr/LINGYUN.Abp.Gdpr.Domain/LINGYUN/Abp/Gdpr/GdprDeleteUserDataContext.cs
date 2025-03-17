using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Gdpr;

namespace LINGYUN.Abp.Gdpr;

public class GdprDeleteUserDataContext(IServiceProvider serviceProvider) : GdprUserDataProviderContext, IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
