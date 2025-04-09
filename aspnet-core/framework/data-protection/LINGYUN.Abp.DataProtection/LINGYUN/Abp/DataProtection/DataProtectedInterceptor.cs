using Microsoft.Extensions.Options;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace LINGYUN.Abp.DataProtection;
public class DataProtectedInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IDataFilter _dataFilter;
    private readonly IDataAccessScope _dataAccessScope;
    private readonly AbpDataProtectionOptions _options;

    public DataProtectedInterceptor(
        IDataFilter dataFilter,
        IDataAccessScope dataAccessScope,
        IOptions<AbpDataProtectionOptions> options)
    {
        _dataFilter = dataFilter;
        _options = options.Value;
        _dataAccessScope = dataAccessScope;
    }

    public async override Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (ShouldDisableDataProtected(invocation, _options))
        {
            using (_dataFilter.Disable<IDataProtected>())
            {
                await invocation.ProceedAsync();
            }
            return;
        }

        var dataProtected = invocation.Method.GetCustomAttribute<DataProtectedAttribute>();
        if (dataProtected?.Operations != null)
        {
            using (_dataAccessScope.BeginScope(dataProtected.Operations))
            {
                await invocation.ProceedAsync();
            }
            return;
        }

        await invocation.ProceedAsync();
    }

    protected virtual bool ShouldDisableDataProtected(
        IAbpMethodInvocation invocation,
        AbpDataProtectionOptions options)
    {
        if (!options.IsEnabled)
        {
            return true;
        }

        if (invocation.Method.IsDefined(typeof(DisableDataProtectedAttribute), true))
        {
            return true;
        }

        return false;
    }
}
