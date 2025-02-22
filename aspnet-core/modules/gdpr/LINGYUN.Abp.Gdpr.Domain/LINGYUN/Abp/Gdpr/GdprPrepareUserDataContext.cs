using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Gdpr;

namespace LINGYUN.Abp.Gdpr;

public class GdprPrepareUserDataContext(Guid requestId, IServiceProvider serviceProvider) : GdprUserDataProviderContext, IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
    public Guid RequestId { get; } = requestId;
}
